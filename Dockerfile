FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Ecommerce.slnx .
COPY src/Ecommerce.Api/Ecommerce.Api.csproj src/Ecommerce.Api/
COPY src/Ecommerce.Application/Ecommerce.Application.csproj src/Ecommerce.Application/
COPY src/Ecommerce.Domain/Ecommerce.Domain.csproj src/Ecommerce.Domain/
COPY src/Ecommerce.Infra/Ecommerce.Infra.csproj src/Ecommerce.Infra/
COPY src/Ecommerce.Domain.Tests/Ecommerce.Domain.Tests.csproj src/Ecommerce.Domain.Tests/

RUN dotnet restore src/Ecommerce.Api/Ecommerce.Api.csproj

COPY src/Ecommerce.Api/ src/Ecommerce.Api/
COPY src/Ecommerce.Application/ src/Ecommerce.Application/
COPY src/Ecommerce.Domain/ src/Ecommerce.Domain/
COPY src/Ecommerce.Infra/ src/Ecommerce.Infra/

RUN dotnet publish src/Ecommerce.Api/Ecommerce.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Ecommerce.Api.dll"]
