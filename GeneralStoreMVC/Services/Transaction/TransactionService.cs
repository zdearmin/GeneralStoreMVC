using GeneralStoreMVC.Data;
using GeneralStoreMVC.Models.Transaction;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreMVC.Services.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly GeneralStoreDbMVCContext _dbContext;
        public TransactionService(GeneralStoreDbMVCContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateTransaction(TransactionCreate model)
        {
            if (model == null)
            {
                return false;
            }

            _dbContext.Transactions.Add(new Data.Transaction
            {
                CustomerId = model.CustomerId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                DateOfTransaction = DateTime.Now
            });

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<TransactionDetail> GetTransactionById(int id)
        {
            var transaction = await _dbContext.Transactions
                .Include(c => c.Customer)
                .Include(p => p.Product)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (transaction is null)
            {
                return null;
            }

            return new TransactionDetail
            {
                Id = transaction.Id,
                CustomerId = transaction.CustomerId,
                CustomerName = transaction.Customer.Name,
                ProductId = transaction.ProductId,
                ProductName = transaction.Product.Name,
                Quantity = transaction.Quantity,
                TransactionTotal = transaction.Quantity * transaction.Product.Price,
                DateOfTransaction = transaction.DateOfTransaction
            };
        }
        
        public async Task<IEnumerable<TransactionListItem>> GetAllTransactions()
        {
            var transactions = await _dbContext.Transactions
                .Include(c => c.Customer)
                .Include(p => p.Product)
                .Select(transaction => new TransactionListItem
            {
                Id = transaction.Id,
                CustomerName = transaction.Customer.Name,
                ProductName = transaction.Product.Name,
                Quantity = transaction.Quantity,
                TransactionTotal = transaction.Quantity * transaction.Product.Price,
                DateOfTransaction = transaction.DateOfTransaction
            })
                .ToListAsync();
            return transactions;
        }

        public async Task<bool> UpdateTransaction(TransactionEdit model)
        {
            var transaction = await _dbContext.Transactions.FindAsync(model.Id);

            if (transaction is null)
            {
                return false;
            }

            transaction.CustomerId = model.CustomerId;
            transaction.ProductId = model.ProductId;
            transaction.Quantity = model.Quantity;
            transaction.DateOfTransaction = model.DateOfTransaction;

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteTransaction(int id)
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);

            if (transaction is null)
            {
                return false;
            }
            
            _dbContext.Transactions.Remove(transaction); 

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return true;
            }
            return false;
        }
    }
}
