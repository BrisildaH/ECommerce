using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Ecommerce.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> GetProductById(int productId)
        {
           return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }
        public async Task<Product> GetProductByDescription(string description)
        {
            return await _context.Products
                          .Where(p => p.Description.ToLower() == description.ToLower())
                          .FirstOrDefaultAsync();
        }

        public IQueryable<Product> GetAllProducts()
        {
            return _context.Products;
        }


        public async Task AddProduct(Product product)
        { 
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

      
        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int productId)
        {
            var product = await GetProductById(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<List<Product>> GetFilteredProducts(string? name, decimal? minPrice, decimal? maxPrice, bool? isAvailable, bool isAdmin)
        {
            var productQuery = _context.Products.AsQueryable();

            if (!isAdmin)
            {
                productQuery = productQuery.Where(p => p.IsPublic == true);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                productQuery = productQuery.Where(p => p.Name.Contains(name));
            }

            if (minPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.Price <= maxPrice.Value);
            }

            if (isAvailable.HasValue)
            {
                productQuery = productQuery.Where(p => p.IsAvailable == isAvailable.Value);
            }

            return await productQuery.ToListAsync();
        }
    }
}