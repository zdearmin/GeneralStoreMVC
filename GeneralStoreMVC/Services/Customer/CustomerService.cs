using GeneralStoreMVC.Data;
using GeneralStoreMVC.Models.Customer;
using GeneralStoreMVC.Models.Transaction;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreMVC.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly GeneralStoreDbMVCContext _dbContext;
        public CustomerService(GeneralStoreDbMVCContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateCustomer(CustomerCreate model)
        {
            if (model == null)
            {
                return false;
            }

            _dbContext.Customers.Add(new Data.Customer
            {
                Name = model.Name,
                Email = model.Email
            });

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<CustomerDetail> GetCustomerById(int customerId)
        {
            var customer = await _dbContext.Customers.FindAsync(customerId);

            if (customer is null)
            {
                return null;
            }

            return new CustomerDetail
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Transactions = customer.Transactions.Select(t => new TransactionListItem
                {
                    Id = t.Id,
                    CustomerName = t.Customer.Name,
                    ProductName = t.Product.Name,
                    Quantity = t.Quantity,
                    TransactionTotal = t.Quantity * t.Product.Price,
                    DateOfTransaction = t.DateOfTransaction
                }).ToList()
            };
        }

        public async Task<IEnumerable<CustomerListItem>> GetAllCustomers()
        {
            var customers = await _dbContext.Customers.Select(customer => new CustomerListItem
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            })
                .ToListAsync();
            return customers;
        }

        public async Task<bool> UpdateCustomer(CustomerEdit model)
        {
            var customer = await _dbContext.Customers.FindAsync(model.Id);

            if (customer is null)
            {
                return false;
            }

            customer.Name = model.Name;
            customer.Email = model.Email;

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCustomer(int customerId)
        {
            var customer = _dbContext.Customers.Find(customerId);

            if (customer is null)
            {
                return false;
            }

            _dbContext.Customers.Remove(customer);

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }
    }
}
