using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ams.Models;

namespace ams.Controllers
{
    public class CustomerController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        //
        // GET: /Customer/

        public ActionResult Index()
        {
            return View(db.Customer.OrderBy(x => x.City).ThenBy(y => y.Name).ToList());
        }

        //
        // GET: /Customer/Details/5

        public ActionResult Details(int id = 0)
        {
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.PendingDate = DateTime.Now;
                db.Customer.Add(customer);
                db.SaveChanges();

                ////-- ADDING OPENING BALANCE
                //Ledger ledger = new Ledger();
                //ledger.Balance = customer.OpeningBalance;
                //ledger.Date = DateTime.Now;
                //ledger.CustomerId = db.Customer.Max(u => u.CustomerID);
                //ledger.BillNo = "";
                //ledger.Particular = "Opening Balance";
                //ledger.Debit = 0;
                //ledger.Credit = 0;
                //ledger.isCheque = 0;

                //db.Ledger.Add(ledger);
                //db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        
        /*
         * 
            AJAX FUNCTIONS HERE
         * 
         */
        public ActionResult TurnOnNotifications(int id)
        {
            Customer c = db.Customer.Find(id);
            c.AlertStatus = 1;
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Request Successful, Notifications for " + c.Name + " are On now");
        }

        public ActionResult TurnOffNotifications(int id)
        {
            Customer c = db.Customer.Find(id);
            c.AlertStatus = 0;
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Request Successful, Notifications for " + c.Name + " are Off now");
        }

        public ActionResult IsNameAvailble(string _Name)
        {
            try
            {
                var tag = db.Customer.Single(x => x.Name == _Name);
                if (tag == null)
                {
                    return Json("Y");
                }
                else
                {
                    return Json("N");
                }
            }
            catch (Exception)
            {
                return Json("Y");
            }
        }
    }
}