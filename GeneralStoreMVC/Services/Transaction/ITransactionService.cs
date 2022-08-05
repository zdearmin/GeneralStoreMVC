using GeneralStoreMVC.Models.Transaction;

namespace GeneralStoreMVC.Services.Transaction
{
    public interface ITransactionService
    {
        Task<bool> CreateTransaction(TransactionCreate model);
        Task<IEnumerable<TransactionListItem>> GetAllTransactions();
        Task<TransactionDetail> GetTransactionById(int id);
        Task<bool> UpdateTransaction(TransactionEdit model);
        Task<bool> DeleteTransaction(int id);
    }
}
