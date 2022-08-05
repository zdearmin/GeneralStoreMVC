using System.ComponentModel.DataAnnotations;

namespace GeneralStoreMVC.Models.Transaction
{
    public class TransactionListItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double TransactionTotal { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}
