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
        
        public ActionResult ViewSummary(string UEN, int dtYear, int dtMonth, int dtDay)
        {
            UEN = HttpContext.Session.GetInt32("UEN");
            Transaction transaction = transactionContext.GetTransactionsByDay(loginId)
        }
    }
}