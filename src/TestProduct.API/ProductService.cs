using TestProduct.API.Models;

namespace TestProduct.API;

public class ProductService(IProductRepository repository)
{
    private readonly IProductRepository _repository = repository;

    public async Task<Product?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be greater than 0", nameof(id));

        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required");

        return await _repository.CreateAsync(product);
    }
}
