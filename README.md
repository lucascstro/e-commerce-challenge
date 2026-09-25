# E-commerce Challenge

Desafio técnico para a posição .NET Core Pleno — plataforma de e-commerce onde clientes podem reservar produtos temporariamente indisponíveis ou em alta demanda.

## Arquitetura

Clean Architecture, dependência sempre apontando para dentro (em direção ao Domínio):

```
src/
├── Ecommerce.Domain        # Entidades ricas, exceções de domínio, interfaces de repositório
├── Ecommerce.Application   # Casos de uso (services), DTOs
├── Ecommerce.Infra         # EF Core InMemory, repositórios, background service, seed
└── Ecommerce.Api           # Controllers, exception handler global, DI
```

`Domain` não depende de nada; `Application` fala só com as interfaces de repositório do `Domain`; `Infra` implementa essas interfaces; `Api` só conhece as interfaces de `Application`.

## Decisões técnicas

### Mecanismo de expiração: background service (não verificação sob demanda)
A reserva expira automaticamente após **72 horas** (`Reservation.ExpiresAt = CreatedAt + 72h`). A expiração é verificada por um `IHostedService` (`ReservationExpirationValidationBackgroundService`), que roda em um loop a cada **5 minutos**: busca todas as reservas com status `Active`, e para cada uma com `ExpiresAt` já vencido, marca a reserva como `Expired` e o produto correspondente volta para `Available`. Optei por background service em vez de verificação sob demanda porque reflete melhor o cenário real de e-commerce (o produto deve "aparecer" disponível para qualquer cliente que liste o catálogo, mesmo que ninguém tenha acabado de consultar aquela reserva específica).

### Concorrência: optimistic concurrency com `RowVersion`
`Product` tem uma coluna `RowVersion` (token de concorrência do EF Core, configurado em `ProductConfiguration`). Quando duas requisições tentam reservar o mesmo produto ao mesmo tempo, a primeira a persistir vence; a segunda recebe um `DbUpdateConcurrencyException` do EF Core, que é traduzido para `ConcurrencyException` (`UnitOfWork.SaveChangesAsync`) e mapeado para **HTTP 409 Conflict** pelo exception handler global. Isso foi validado manualmente disparando duas requisições `POST /products/{id}/reserve` simultâneas contra o mesmo produto — só uma delas retorna 200, a outra 409.

> Observação: como o provedor InMemory do EF Core não incrementa `RowVersion` sozinho, `AppDbContext` faz isso manualmente em `SaveChanges`/`SaveChangesAsync` (`BumpRowVersions()`), gerando um novo valor a cada `Add`/`Update` rastreado.

### Regra de disponibilidade
Um produto só pode ser reservado se estiver com status `Available`. Isso é garantido pelo próprio domínio: `Product.MarkAsReserved()` lança exceção se o produto já estiver `Reserved` ou `Unavailable`. Também não é possível **criar** um produto já com status `Reserved` via API (`POST /products`) — só é possível chegar nesse estado através do fluxo real de reserva.

### Regra adicional: bloqueio de alteração/exclusão com reserva ativa
Produtos e clientes com reserva ativa não podem ser atualizados (`PUT`) nem excluídos (`DELETE`) — a tentativa retorna 409. Isso evita, por exemplo, apagar um cliente ou produto que está no meio de uma reserva em andamento.

### Tratamento de erros centralizado
Um `GlobalExceptionHandler` (`IExceptionHandler`, `Ecommerce.Api/ExceptionHandling`) captura qualquer exceção não tratada e converte em uma resposta `ProblemDetails` consistente, sem derrubar a aplicação:

| Exceção | HTTP Status |
|---|---|
| `ConcurrencyException` | 409 Conflict |
| `DomainException` (regra de negócio/domínio) | 400 Bad Request |
| `ArgumentException` (validação de entrada) | 400 Bad Request |
| `InvalidOperationException` (conflito de estado / não encontrado) | 409 Conflict |
| Qualquer outra exceção | 500 Internal Server Error (mensagem genérica ao cliente, detalhe completo só no log) |

### Sem autenticação
Conforme o desafio, não há autenticação. O endpoint de reserva (`POST /products/{id}/reserve`) recebe o `customer_id` via query string, já que não existe um cliente "logado" — essa é a única divergência do contrato literal do desafio (que descreve o endpoint recebendo apenas o ID do produto).

### Dados fixos no seed
Os 3 primeiros itens de cada lista do seed (`Ecommerce.Infra/Database/Seed.cs`) usam GUIDs fixos, para facilitar testes manuais sem precisar olhar o log a cada execução (veja a seção de [casos de teste](#casos-de-teste-práticos) abaixo). Os demais produtos continuam com GUID aleatório.

## Como executar

Requer **.NET SDK 10** (para rodar localmente) ou **Docker** (para rodar em container).

### Opção 1 — `dotnet run`
```bash
dotnet run --project src/Ecommerce.Api
```
A API sobe em `http://localhost:5286` (perfil `http` do `launchSettings.json`). Em ambiente de desenvolvimento, o seed roda automaticamente na inicialização e a UI do Scalar fica disponível em `http://localhost:5286/scalar/v1`.

### Opção 2 — Docker / docker compose
```bash
docker compose up --build
```
A API sobe em `http://localhost:8080` (mapeada no `docker-compose.yml`), com `ASPNETCORE_ENVIRONMENT=Development` (seed + Scalar habilitados). Para derrubar:
```bash
docker compose down
```

### Rodando os testes automatizados
```bash
dotnet test src/Ecommerce.Domain.Tests
```

## Casos de teste práticos

O seed cria os seguintes dados com **GUID fixo**, para facilitar os testes manuais abaixo (substitua `$BASE` por `http://localhost:5286` ou `http://localhost:8080`):

| Entidade | Nome | ID |
|---|---|---|
| Cliente | Marina Silva | `10000000-0000-0000-0000-000000000001` |
| Cliente | Cabo Daciolo | `10000000-0000-0000-0000-000000000002` |
| Cliente | Padre Kelmon | `10000000-0000-0000-0000-000000000003` |
| Produto | Carro (`Reserved`, reservado por Padre Kelmon) | `20000000-0000-0000-0000-000000000001` |
| Produto | Moto (`Reserved`, reservado por Cabo Daciolo) | `20000000-0000-0000-0000-000000000002` |
| Produto | Bicicleta (`Reserved`, reservado por Marina Silva) | `20000000-0000-0000-0000-000000000003` |
| Reserva | Marina Silva → Bicicleta | `30000000-0000-0000-0000-000000000001` |
| Reserva | Cabo Daciolo → Moto | `30000000-0000-0000-0000-000000000002` |
| Reserva | Padre Kelmon → Carro | `30000000-0000-0000-0000-000000000003` |

Os demais 7 produtos do seed têm GUID aleatório (visível no console ao subir a aplicação) — 5 `Available`, 2 `Unavailable`.

### 1. Listar todos os produtos e seus status
```bash
curl $BASE/api/products
```

### 2. Listar as reservas de um cliente
```bash
curl $BASE/api/customer/10000000-0000-0000-0000-000000000003/reservations
```

### 3. Reservar um produto disponível
```bash
# cria um produto Available para o teste
curl -X POST $BASE/api/products -H "Content-Type: application/json" \
  -d '{"name":"Console","description":"Video game novo","status":0}'
# reserva (troque {productId} pelo id retornado acima)
curl -X POST "$BASE/api/products/{productId}/reserve?customer_id=10000000-0000-0000-0000-000000000001"
```

### 4. Tentar reservar um produto já reservado → 409
```bash
curl -X POST "$BASE/api/products/20000000-0000-0000-0000-000000000001/reserve?customer_id=10000000-0000-0000-0000-000000000001"
```

### 5. Cancelar uma reserva ativa (o produto volta a `Available`)
```bash
curl -X DELETE "$BASE/api/products/30000000-0000-0000-0000-000000000003/reserve"
curl $BASE/api/products/20000000-0000-0000-0000-000000000001   # status volta para "Available"
```

### 6. Concorrência — duas reservas simultâneas no mesmo produto
```bash
curl -X POST $BASE/api/products -H "Content-Type: application/json" \
  -d '{"name":"Concorrencia","description":"Teste de concorrencia","status":0}'
# troque {productId} pelo id retornado; dispare as duas em paralelo
curl -X POST "$BASE/api/products/{productId}/reserve?customer_id=10000000-0000-0000-0000-000000000001" &
curl -X POST "$BASE/api/products/{productId}/reserve?customer_id=10000000-0000-0000-0000-000000000002" &
wait
```
Resultado esperado: uma resposta `200` e uma `409 Conflito de concorrência`.

### 7. Tentar criar um produto já como "reservado" → 400
```bash
curl -X POST $BASE/api/products -H "Content-Type: application/json" \
  -d '{"name":"Invalido","description":"teste","status":2}'
```

### 8. Tentar alterar/excluir um produto ou cliente com reserva ativa → 409
```bash
curl -X DELETE $BASE/api/products/20000000-0000-0000-0000-000000000001
curl -X DELETE $BASE/api/customer/10000000-0000-0000-0000-000000000003
```

### 9. CRUD completo de cliente e produto
```bash
# cliente
curl -X POST $BASE/api/customer -H "Content-Type: application/json" -d '{"name":"Novo Cliente"}'
curl $BASE/api/customer
curl -X PUT $BASE/api/customer/{id} -H "Content-Type: application/json" -d '{"name":"Nome Atualizado"}'
curl -X DELETE $BASE/api/customer/{id}

# produto
curl -X PUT $BASE/api/products/{id} -H "Content-Type: application/json" \
  -d '{"name":"Nome Atualizado","description":"Nova descricao","status":0}'
```

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/products` | Lista todos os produtos |
| GET | `/api/products/{id}` | Detalhe de um produto |
| POST | `/api/products` | Cria um produto |
| PUT | `/api/products/{id}` | Atualiza nome/descrição (bloqueado se houver reserva ativa) |
| DELETE | `/api/products/{id}` | Remove um produto (bloqueado se houver reserva ativa) |
| POST | `/api/products/{id}/reserve?customer_id={id}` | Reserva um produto disponível |
| DELETE | `/api/products/{reservation_id}/reserve` | Cancela uma reserva ativa |
| GET | `/api/customer` | Lista todos os clientes |
| GET | `/api/customer/{id}` | Detalhe de um cliente |
| POST | `/api/customer` | Cria um cliente |
| PUT | `/api/customer/{id}` | Atualiza o nome (bloqueado se houver reserva ativa) |
| DELETE | `/api/customer/{id}` | Remove um cliente (bloqueado se houver reserva ativa) |
| GET | `/api/customer/{id}/reservations` | Lista as reservas de um cliente |
| GET | `/api/reservation` | Lista todas as reservas |

> O `{id}` do `DELETE /api/products/{reservation_id}/reserve` é o **ID da reserva**, não do produto — diferente do `POST` de reserva (que usa o ID do produto). Optei por essa abordagem porque o mesmo produto pode ter múltiplas reservas ao longo do tempo (histórico), e a operação de cancelamento precisa identificar exatamente qual reserva está sendo cancelada.
