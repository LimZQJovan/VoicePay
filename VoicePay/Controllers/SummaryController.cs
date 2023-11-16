using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VoicePay.DAL;
using VoicePay.Models;

namespace VoicePay.Controllers
{
    public class SummaryController : Controller
    {
        private readonly ILogger<SummaryController> _logger;

        public SummaryController(ILogger<SummaryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        private TransactionDAL transactionContext = new TransactionDAL();
        
        public ActionResult ViewSummary(string? UEN, int dtYear, int dtMonth, int dtDay)
        {
            dtYear = DateTime.Now.Year;
            dtMonth = DateTime.Now.Month;
            dtDay = DateTime.Now.Day;

            UEN = HttpContext.Session.GetString("UEN");
            Transaction transaction = transactionContext.GetTransactionsByDay(UEN, dtYear, dtMonth, dtDay);
            return View(transaction);
        }
    }
}