using Ecommerce.Application.Dtos.Request;
using Ecommerce.Application.Dtos.Response;
using Ecommerce.Domain.Repositories;

namespace Ecommerce.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest request, CancellationToken ct)
        {
            var product = new Domain.Entities.Product(
                request.Name,
                request.Description,
                request.Status
            );

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return new ProductResponse(
                product.ProductId,
                product.Name,
                product.Description,
                product.Status.ToString()
            );
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync(CancellationToken ct)
        {
            var products = await _productRepository.GetAllAsync();
            return await Task.FromResult(
                products.Select(p => new ProductResponse(
                    p.ProductId,
                    p.Name,
                    p.Description,
                    p.Status.ToString()
                ))
            );
        }

        public async Task<ProductResponse> GetProductByIdAsync(Guid productId, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            return new ProductResponse(
                product.ProductId,
                product.Name,
                product.Description,
                product.Status.ToString()
            );
        }

        public async Task<ProductResponse> UpdateProductAsync(Guid productId, ProductRequest request, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException("Produto não encontrado.");

            product.UpdateName(request.Name);
            product.UpdateDescription(request.Description);

            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return new ProductResponse(
                product.ProductId,
                product.Name,
                product.Description,
                product.Status.ToString()
            );
        }
        public async Task ReserveProductAsync(Guid productId, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException("Produto não encontrado.");

            product.MarkAsUnavailable();
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        public async Task MakeAvailableProductAsync(Guid productId, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException("Produto não encontrado.");

            product.MarkAsAvailable();
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteProductAsync(Guid productId, CancellationToken ct)
        {
            await _productRepository.DeleteAsync(productId);
            await _unitOfWork.SaveChangesAsync(ct);
        }

    }
}