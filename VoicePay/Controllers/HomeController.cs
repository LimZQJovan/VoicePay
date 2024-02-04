using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VoicePay.DAL;
using VoicePay.Models;
using QRCoder;
using System.Drawing;
using Newtonsoft.Json;
using System.Text;
using Stripe;
using Stripe.Checkout;
using System.Reflection.Metadata.Ecma335;

namespace VoicePay.Controllers
{

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        TransactionDAL transactionContext = new TransactionDAL();
        StaffDAL staffContext = new StaffDAL();
        InventoryDAL inventoryContext = new InventoryDAL();
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
            string UEN = "";
            string stallName = "";
            string location = "";

            StaffDAL staffContext = new StaffDAL();

            if (staffContext.Login(loginID, password, out UEN, out stallName, out location))
            {
                HttpContext.Session.SetString("UEN", UEN);
                HttpContext.Session.SetString("AccId", loginID);
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

        public ActionResult NumberPadCustomise()
        {
            return View("NumberPadCustomise");
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

        public ActionResult Admin()
        {
            return View("Admin");
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
        public ActionResult Inventory()
        {
            string accId = HttpContext.Session.GetString("AccId");
            List<Inventory> inventoryList = inventoryContext.GetInventoryDetails(accId);
            return View(inventoryList);
        }
		public ActionResult CreateInventory()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateInventory(Inventory inventory)
		{

            string accId = HttpContext.Session.GetString("AccId");
            
            if (ModelState.IsValid)
			{
                Inventory inventoryModel = new Inventory
                {
                    InventoryID = inventoryContext.CountItems(accId) + 1,
                    ItemName = inventory.ItemName,
                    Quantity = inventory.Quantity,
                    SupplierName = inventory.SupplierName,
                    SupplierContactNo = inventory.SupplierContactNo,
                    AccId = accId
                };
                inventoryContext.Add(inventoryModel);                
                // Redirect user to Staff/Index view
                return RedirectToAction("Inventory");
			}
			else
			{
				// Input validation fails, return to the Create view
				// to display error message
				return View(inventory);
			}
		}
		//public ActionResult EditInventory(int? id)
		//{
		//    string accId = HttpContext.Session.GetString("AccId");
		//    if (id == null)
		//    {
		//        return RedirectToAction("Inventory");
		//    }
		//    Inventory inventory = inventoryContext.GetInventoryItem(id.Value, accId);
		//    if (inventory == null)
		//    {
		//        // Return to the listing page, not allowed to edit
		//        return RedirectToAction("Inventory");
		//    }
		//    return View(inventory);
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult EditInventory(Inventory inventory)
		//{

		//    //Update staff record to database
		//    inventoryContext.Update(inventory);
		//    return RedirectToAction("Inventory");
		//}

		private const string StripeSecretKey = "sk_test_51OfEkCD61euiwXOhBEh5cBgv3ETAxJ8PIyjRGEhpwizCQxqlIZYKudcvbgFgOl6WbfgrCAyXu8vmW8ZCgY9Rngdz00rgwaxCsy";
        private const string StripePublishableKey = "pk_test_51OfEkCD61euiwXOhBPjJPrNCF3ecfexHkZsBupWqFjAZTWK5VzD1qnQTvXlaRPBgXjCZsq1cZQr1Bfx8zYktTCKf00FPyCDhm9";

        private string GenerateStripeCheckoutSession(float amount)
        {
            StripeConfiguration.ApiKey = StripeSecretKey;

            var lineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(amount * 100), // Convert amount to cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Your Product Name",
                    },
                },
                Quantity = 1,
            },
        };

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    SetupFutureUsage = "off_session",
                },
                SuccessUrl = "https://stripe.com/en-sg",

            };

            var service = new SessionService();
            var session = service.Create(options);

            return session.Url;
        }
        public IActionResult QR()
        {

            // Retrieve the decimal amount from the query string parameter named "amount"
            string amountStr = HttpContext.Request.Query["amount"];

            HttpContext.Session.SetString("amount", amountStr);

            // Convert the amount to float (modify as needed based on your requirements)
            float amount = float.TryParse(amountStr, out float parsedAmount) ? parsedAmount : 0.0f;

            // Generate a new Checkout Session dynamically
            string sessionUrl = GenerateStripeCheckoutSession(amount);

            // Create QR code generator
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(sessionUrl, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            // Convert QR code to bitmap
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            // Save the QR code image or send it to the view
            // For simplicity, let's save it to a file in this example
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "qrcode.png");
            qrCodeImage.Save(filePath);

            // Return a view or redirect to the next page
            return View("QR");
        }

        public ActionResult Confirm()
        {
            Debug.WriteLine("BYE");
            float amount = float.TryParse(HttpContext.Session.GetString("amount"), out float parsedAmount) ? parsedAmount : 0.0f;
            transactionContext.AddTransaction(HttpContext.Session.GetString("UEN"), "5501234567", amount, DateTime.Now, "BNKREF6543210987");
            ViewData["amount"] = HttpContext.Session.GetString("amount");
            return View("Confirm");
        }

    }
}