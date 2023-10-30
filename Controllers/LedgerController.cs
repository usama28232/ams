using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ams.Models;
using System.Data.SqlClient;

namespace ams.Controllers
{
    public class LedgerController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        //
        // GET: /Ledger/

        public ActionResult Index()
        {
            return View(db.Ledger.ToList());
        }


        //
        // GET: /ChequesToday/

        public ActionResult ChequesToday()
        {
            var lstCheques = from a in db.Ledger
                             where a.isCheque == 1 && a.isSeen == 0 && a.Date <= DateTime.Now
                             join b in db.Customer on a.CustomerId equals b.CustomerID
                             orderby a.Date
                             select new Cheques { Id = a.LedgerID, Date = a.Date, BillNo = a.BillNo, Particular = a.Particular, 
                                 CustomerName = b.Name, isCheque = a.isCheque, amount = a.Debit };
            return View(lstCheques.ToList());
        }

        //
        // Sends Selected Customer Alerts To Pending

        public ActionResult Seen(List<int> cids)
        {
            List<Ledger> lstLedg = db.Ledger.Select(x => x).Where(a => cids.Contains(a.LedgerID)).ToList();

            foreach (Ledger c in lstLedg)
            {
                c.isSeen = 1;
            }
            db.SaveChanges();

            return Json(new
            {
                statu = "1"
            });
        }

        //
        // GET: /PendingCheques/

        public ActionResult PendingCheques()
        {
            DateTime dateToday = DateTime.Now.Date;
            var lstPendingCheques = from a in db.Ledger
                             where a.isCheque == 1 && a.Date > dateToday
                             join b in db.Customer on a.CustomerId equals b.CustomerID
                             orderby a.Date
                             select new Cheques { Date = a.Date, BillNo = a.BillNo, Particular = a.Particular, CustomerName = b.Name, isCheque = a.isCheque,
                                 amount = a.Debit };
            return View(lstPendingCheques.ToList());
        }


        //
        // GET: /Ledger/RptLedger/1
        public ActionResult RptLedger(int id = 0)
        {
            if (id > 0)
            {
                ViewBag.Customerid = id;
                return View();
            }
            else {
                return HttpNotFound();
            }
        }

        //
        // GET: /Ledger/CustomerLedger/1
        public ActionResult CustomerLedger(int id = 0)
        {
            Customer cs = db.Customer.Select(x => x).Where(x => x.CustomerID == id).FirstOrDefault();
            if (cs != null)
            {
                ViewBag.Customerid = id;
                ViewBag.Name = cs.Name;
                ViewBag.Address = cs.Address;
                ViewBag.Contacts = cs.Contact + ' ' + cs.Contact1 + ' ' + cs.Contact2 + ' ' + cs.Contact3;
                ViewBag.OpeningBalance = cs.OpeningBalance;

                var param_CustomerId = new SqlParameter("@CustomerId", id);
                var ledger = db.Database.SqlQuery<Ledger>("SP_CustomerLedger @CustomerId", param_CustomerId).ToList();

                if (ledger == null)
                {
                    return HttpNotFound();
                }
                return View(ledger.ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // GET: /Ledger/Details/5

        public ActionResult Details(int id = 0)
        {
            Ledger ledger = db.Ledger.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            return View(ledger);
        }

        //
        // GET: /Ledger/Create/{id}
        public ActionResult Create(int id = 0)
        {
            if (id > 0)
            {
                DbSqlQuery<Ledger> lg = db.Ledger.SqlQuery("select * from Ledgers l where l.LedgerID = (select max(le.LedgerID) from Ledgers le where le.CustomerId=" + id + ")");

                if (lg.Count() > 0)
                {
                    ViewBag.Balance = lg.Select(x => x.Balance).First();
                    ViewBag.Customerid = id;
                }
                else
                {
                    ViewBag.Balance = "0";
                    ViewBag.Customerid = id;
                }
                return View();
            }
            else {
                return HttpNotFound();
            }
        }

        //
        // POST: /Ledger/Create

        [HttpPost]
        public ActionResult Create(Ledger ledger)
        {
            if (ModelState.IsValid)
            {
                db.Ledger.Add(ledger);
                db.SaveChanges();
                return RedirectToAction("CustomerLedger/" + ledger.CustomerId);
            }

            return View(ledger);
        }

        //
        // GET: /Ledger/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Ledger ledger = db.Ledger.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            return View(ledger);
        }

        //
        // POST: /Ledger/Edit/5

        [HttpPost]
        public ActionResult Edit(Ledger ledger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ledger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CustomerLedger/" + ledger.CustomerId, "Ledger");
            }
            return View(ledger);
        }

        //
        // GET: /Ledger/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Ledger ledger = db.Ledger.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            return View(ledger);
        }

        //
        // POST: /Ledger/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ledger ledger = db.Ledger.Find(id);
            db.Ledger.Remove(ledger);
            db.SaveChanges();
            return RedirectToAction("CustomerLedger/" + ledger.CustomerId, "Ledger");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}