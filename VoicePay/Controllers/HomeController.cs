using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VoicePay.DAL;
using VoicePay.Models;

namespace VoicePay.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        TransactionDAL transactionContext = new TransactionDAL();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StallLogin(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string loginID = formData["txtLoginID"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();
            //string UEN = "";
            //string stallName = "";
            //string location = "";

            StaffDAL staffContext = new StaffDAL();

            if (staffContext.Login(loginID, password))
            {
                HttpContext.Session.SetString("UEN", UEN);
                HttpContext.Session.SetString("Name", stallName);
                HttpContext.Session.SetString("Location", location);
                // Redirect user to the "StaffMain" view through an action
                return RedirectToAction("StallMain");
            }
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";
                // Redirect user back to the index view through an action
                return RedirectToAction("Index");
            }
        }
		public ActionResult StallMain()
		{
			// Stop accessing the action if not logged in
			// or account not in the "Staff" role

			return View();
		}

        public ActionResult Language()
        {
            return View("Language");
        }

        public ActionResult NumberPad()
        {
            return View("NumberPad");
        }

        public ActionResult VoicePay()
        {
            return View("VoicePay");
        }
        public ActionResult LogOut()
		{
			// Clear all key-values pairs stored in session state
			// Call the Index action of Home controller
			return RedirectToAction("Index");
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        public ActionResult Report(string UEN, int selectedYear, int selectedMonth, int selectedDay)
        {
            int dtYear = selectedYear == -1 ? DateTime.Now.Year : selectedYear;
            int dtMonth = selectedMonth;
            int dtDay = selectedDay;

            UEN = HttpContext.Session.GetString("UEN");
            List<Transaction> transactionList = transactionContext.GetTransactions(UEN, dtYear, dtMonth, dtDay);
            return View(transactionList);
        }
    }
}