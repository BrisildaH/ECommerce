using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interface { 
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task<Product> GetProductById(int productId);
        Task<Product> GetProductByDescription(string description);
        IQueryable<Product> GetAllProducts();
        Task UpdateProduct(Product product);
        Task DeleteProduct(int productId);
        Task<List<Product>> GetFilteredProducts(string? name, decimal? minPrice, decimal? maxPrice, bool? isAvailable, bool isAdmin);

    }

}
