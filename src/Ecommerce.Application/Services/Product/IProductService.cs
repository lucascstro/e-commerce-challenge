using Ecommerce.Application.Dtos.Request;
using Ecommerce.Application.Dtos.Response;

namespace Ecommerce.Application.Services.Product
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductResponse>> GetAllProductsAsync(CancellationToken ct);
        public Task<ProductResponse> GetProductByIdAsync(Guid productId, CancellationToken ct);
        public Task<ProductResponse> CreateProductAsync(ProductRequest request, CancellationToken ct);
        public Task<ProductResponse> UpdateProductAsync(Guid productId, ProductRequest request, CancellationToken ct);
        public Task DeleteProductAsync(Guid productId, CancellationToken ct);
        public Task ReserveProductAsync(Guid productId, CancellationToken ct);
        public Task MakeAvailableProductAsync(Guid productId, CancellationToken ct);
    }
}