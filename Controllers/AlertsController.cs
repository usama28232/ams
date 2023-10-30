using ams.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Controllers
{
    public class AlertsController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        //
        // GET: /Alerts/

        public ActionResult Index()
        {
            List<Customer> cust = db.Customer.Select(x => x).ToList();
            List<Ledger> inledg = db.Ledger.Select(x => x).ToList();

            // cust contains updatedlist
            cust = cust.Where(l1 => !inledg.Any(l2 => l2.CustomerId == l1.CustomerID && l2.Debit != 0 && l2.Balance <= 0 && l2.Date >= DateTime.Now.AddDays(-(l1.AlertDays)))).ToList();
            cust = cust.Where(l3 => l3.PendingDate < DateTime.Now.AddDays(-(l3.AlertDays)) && l3.AlertStatus == 1).ToList();
            return View(cust);
        }


        //
        // Redirects To Customer Alerts Reports

        public ActionResult Print(List<int> cids)
        {
            //--- UPDATE PENDING DATES OF CUSTOMERS
            List<Customer> lstCustomers = db.Customer.Select(x => x).Where(a => cids.Contains(a.CustomerID)).ToList();

            foreach (Customer c in lstCustomers)
            {
                c.PendingDate = DateTime.Now;
            }
            db.SaveChanges();

            //--- SEND LIST TO REPORT
            TempData["cids"] = cids;
            RedirectToAction("CustomerAlerts", "Reports");

            return Json(new
            {
                statu = "1"
            });
        }


        //
        // Sends Selected Customer Alerts To Pending

        public ActionResult Pending(List<int> cids)
        {
            List<Customer> lstCustomers = db.Customer.Select(x => x).Where(a => cids.Contains(a.CustomerID)).ToList();

            foreach (Customer c in lstCustomers)
            {
                c.PendingDate = DateTime.Now; 
            }
            db.SaveChanges();

            return Json(new
            {
                statu = "1"
            });
        }
    }
}
