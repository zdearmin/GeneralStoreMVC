using GeneralStoreMVC.Models.Product;
using GeneralStoreMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreMVC.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly GeneralStoreDbMVCContext _dbContext;

        public ProductService(GeneralStoreDbMVCContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateProduct(ProductCreate model)
        {
            if (model == null)
            {
                return false;
            }

            _dbContext.Products.Add(new Data.Product
            {
                Name = model.Name,
                QuantityInStock = model.QuantityInStock,
                Price = model.Price
            });

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<ProductDetail> GetProductById(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);

            if (product is null)
            {
                return null;
            }

            return new ProductDetail
            {
                Id = product.Id,
                Name = product.Name,
                QuantityInStock = product.QuantityInStock,
                Price = product.Price
            };
        }

        public async Task<IEnumerable<ProductListItem>> GetAllProducts()
        {
            var products = await _dbContext.Products.Select(products => new ProductListItem
            {
                Id = products.Id,
                Name = products.Name,
                QuantityInStock = products.QuantityInStock,
                Price = products.Price
            })
                .ToListAsync();
            return products;
        }

        public async Task<bool> UpdateProduct(ProductEdit model)
        {
            var product = await _dbContext.Products.FindAsync(model.Id);

            if (product is null)
            {
                return false;
            }

            product.Name = model.Name;
            product.QuantityInStock = model.QuantityInStock;
            product.Price = model.Price;

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = _dbContext.Products.Find(productId);

            if (product is null)
            {
                return false;
            }

            _dbContext.Products.Remove(product);

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }
    }
}
