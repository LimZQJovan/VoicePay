﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using VoicePay.DAL;
using VoicePay.Models;

namespace VoicePay.Controllers
{
    public class ReportController : Controller
    {

        public IActionResult ViewReport()
        {
            return View();
        }
       
        //TransactionDAL transactionContext = new TransactionDAL();
        
        //public ActionResult ViewReport(string UEN, int dtYear, int dtMonth, int dtDay)
        //{
        //    dtYear = 2021;
        //    dtMonth = -1;
        //    dtDay = -1;
        //    UEN = HttpContext.Session.GetString("UEN");
        //    List <Transaction> transactionList= transactionContext.GetTransactions(UEN, dtYear, dtMonth, dtDay);
        //    return View(transactionList);
        //}
        //[HttpPost]
        //public IActionResult FilterData(FormModel model)
        //{
        //    Extract selected values from the model
        //    int selectedDay = model.SelectedDay;
        //    int selectedMonth = model.SelectedMonth;
        //    int selectedYear = model.SelectedYear;

        //    Apply filters using the selected date components
        //     ... (Filtering logic using DAL)

        //     Return an appropriate response
        //    return Json(new { message = "Data filtered successfully" });
        //}
    }
}