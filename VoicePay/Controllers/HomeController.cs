﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VoicePay.DAL;
using VoicePay.Models;

namespace VoicePay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

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
            string AccUEN = "";
            string stallName = "";
            string location = "";

            StaffDAL staffContext = new StaffDAL();

            if (staffContext.Login(loginID, password, out AccUEN, out stallName, out location))
            {
                HttpContext.Session.SetString("UEN", AccUEN);
                HttpContext.Session.SetString("Name", stallName);
                HttpContext.Session.SetString("Location", location);

                // Store Login ID in session with the key "LoginID"
                // Store user role "Staff" as a string in session with the key "Role"
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
	}
}