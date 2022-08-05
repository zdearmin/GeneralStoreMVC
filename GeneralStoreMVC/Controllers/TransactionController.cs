using GeneralStoreMVC.Models.Transaction;
using GeneralStoreMVC.Services.Transaction;
using GeneralStoreMVC.Services.Product;
using GeneralStoreMVC.Services.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GeneralStoreMVC.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        public TransactionController(ITransactionService transactionService, ICustomerService customerService, IProductService productService)
        {
            _transactionService = transactionService;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionService.GetAllTransactions();
            return View(transactions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var transaction = await _transactionService.GetTransactionById(id);

            if (transaction is null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        public async Task<IActionResult> Create()
        {
            var customers = await _customerService.GetAllCustomers();
            var products = await _productService.GetAllProducts();

            IEnumerable<SelectListItem> customerSelect = customers.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            IEnumerable<SelectListItem> productSelect = products.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            TransactionCreate model = new TransactionCreate();

            model.ProductOptions = productSelect;
            model.CustomerOptions = customerSelect;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionCreate model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "Model State is Invalid";
                return View(ModelState);
            }

            bool wasCreated = await _transactionService.CreateTransaction(model);

            if (wasCreated)
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMsg"] = "Unable to save to the database. Please try again later.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            TransactionDetail transaction = await _transactionService.GetTransactionById(id);

            if (transaction == null)
            {
                return NotFound();
            }

            var transactionEdit = new TransactionEdit
            {
                Id = transaction.Id,
                CustomerId = transaction.CustomerId,
                ProductId = transaction.ProductId,
                Quantity = transaction.Quantity,
                DateOfTransaction = DateTime.Now
            };

            return View(transactionEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TransactionEdit model)
        {
            if (id != model.Id || !ModelState.IsValid)
            {
                return View(ModelState);
            }

            bool wasUpdated = await _transactionService.UpdateTransaction(model);

            if (wasUpdated)
            {
                return RedirectToAction("Details", new { id = model.Id });
            }

            ViewData["ErrorMsg"] = "Unable to save to the database. Please try again later.";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _transactionService.GetTransactionById(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TransactionDetail model)
        {
            if (await _transactionService.DeleteTransaction(model.Id))
            {
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }
    }
}
