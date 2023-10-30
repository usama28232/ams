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
    public class InvoiceController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        //
        // GET: /Invoice/

        public ActionResult Index( int id = 0 ) {
            if(id==0)
                return View( db.Invoices.ToList( ) );
            else
                return View( db.Invoices.Where( x => x.CSId == id ).ToList( ) );
        }

        //
        // GET: /Invoice/Details/5

        public ActionResult Details(int id = 0)
        {
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        //
        // GET: /Invoice/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Invoice/Create

        [HttpPost]
        public ActionResult Create(Invoice invoice, FormCollection fm)
        {
            if (ModelState.IsValid)
            {
                invoice.Cus = db.Customer.Where( x => x.CustomerID == invoice.CSId ).FirstOrDefault();
                db.Invoices.Add(invoice);
                db.SaveChanges();
                int count = Convert.ToInt32(fm["count"]);
                if (count > 0)
                {
                    string item = fm["_item"];
                    string price = fm["_price"];
                    string qtys = fm["_quantity"];
                    string total = fm["_total"];
                    string[] arritem = item.Split(',');
                    string[] arrprice = price.Split(',');
                    string[] qty = qtys.Split(',');
                    string[] unt = total.Split(',');
                    for (int i = 1; i < unt.Length; i++)
                    {
                        InvoiceDetails inv_detail = new InvoiceDetails();
                        inv_detail.Invid = invoice.InvoiceID;
                        inv_detail.Item = arritem[i];
                        inv_detail.Price = Convert.ToDecimal(arrprice[i]);
                        inv_detail.Quantity = Convert.ToDecimal(qty[i]);
                        inv_detail.Total = Convert.ToDecimal(unt[i]);
                        db.InvoicesDetails.Add(inv_detail);
                        db.SaveChanges();
                    }
                }
                // Ledger

                // Credit
                Ledger LedgerCredit = new Ledger( );
                LedgerCredit.BillNo = invoice.BillNo;
                LedgerCredit.CustomerId = invoice.CSId;
                LedgerCredit.isCheque = 0;
                LedgerCredit.Date = invoice.Date;
                LedgerCredit.Debit = 0;
                LedgerCredit.Credit = Convert.ToInt32( invoice.GAmount );
                LedgerCredit.Balance = 0;
                LedgerCredit.isSeen = 0;
                LedgerCredit.Particular = "Invoice";
                db.Ledger.Add( LedgerCredit );
                db.SaveChanges( );
                // Debit
                Ledger LedgerDebit = new Ledger( );
                LedgerDebit.BillNo = invoice.BillNo;
                LedgerDebit.CustomerId = invoice.CSId;
                LedgerDebit.isCheque = 0;
                LedgerDebit.Date = invoice.Date;
                LedgerDebit.Debit = Convert.ToInt32( invoice.Received );
                LedgerDebit.Credit = 0;
                LedgerDebit.Balance = 0;
                LedgerDebit.isSeen = 0;
                LedgerDebit.Particular = "Invoice";
                db.Ledger.Add( LedgerDebit );
                db.SaveChanges( );
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        //
        // GET: /Invoice/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        //
        // POST: /Invoice/Edit/5

        [HttpPost]
        public ActionResult Edit(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        //
        // GET: /Invoice/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        //
        // POST: /Invoice/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();            

            var lst = db.InvoicesDetails.Select(s => s)
                .Where(z => z.Invid == id);
            foreach (var item in lst)
            {
                db.InvoicesDetails.Remove(item);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);

        }

        // Ajax Methods
        //
        // POST: /Invoice/adder
        public ActionResult adder(int fkey, string itemName, decimal price, decimal qty, decimal total)
        {
            InvoiceDetails inv_detail = new InvoiceDetails();
            inv_detail.Invid = fkey;
            inv_detail.Item = itemName;
            inv_detail.Price= price;
            inv_detail.Quantity = qty;
            inv_detail.Total=total;
            db.InvoicesDetails.Add(inv_detail);
            db.SaveChanges();
            return Json(new
            {
                id = inv_detail.Id
            });
        }

        //
        // POST: /Invoice/updater
        public ActionResult updater(int id, string itemName, decimal price, decimal qty, decimal total)
        {
            InvoiceDetails inv_detail = db.InvoicesDetails.Find(id);
            inv_detail.Item = itemName;
            inv_detail.Price = price;
            inv_detail.Quantity = qty;
            inv_detail.Total = total;
            db.SaveChanges();
            return null;
        }


        //
        // POST: /Invoice/remover
        public ActionResult remover(int id)
        {
            InvoiceDetails inv_detail = db.InvoicesDetails.Find(id);
            db.InvoicesDetails.Remove(inv_detail);
            db.SaveChanges();
            return null;
        }

        //Get Customer (DropDown)
        public static List<SelectListItem> GetCustomer()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            AMSDBContext db = new AMSDBContext();

            var Cstmr = db.Customer.Select(sel => sel);
            foreach (var codes in Cstmr)
            {
                ls.Add(new SelectListItem() { Text = codes.Name, Value = codes.CustomerID.ToString() });
            }
            return ls;
        }

        public JsonResult SearchCustomer(string term)
        {
            var objCustomerlist = (from r in db.Customer
                                   where r.Name.ToLower().Contains(term.ToLower())
                                   select new { label = r.Name , val=r.CustomerID }).ToList();
            return Json(objCustomerlist,JsonRequestBehavior.AllowGet);
        }

    }
}