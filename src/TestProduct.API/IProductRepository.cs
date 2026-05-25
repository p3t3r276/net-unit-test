using TestProduct.API.Models;

namespace TestProduct.API;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);

    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product> CreateAsync(Product product);
}
