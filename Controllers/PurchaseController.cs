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
    public class PurchaseController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        //
        // GET: /Purchase/

        public ActionResult Index( int id = 0 ) {
            if( id == 0 )
                return View( db.Purchase.ToList( ) );
            else
                return View( db.Purchase.Where( x => x.SupId == id ).ToList( ) );
        }

        //
        // GET: /Purchase/Details/5

        public ActionResult Details(int id = 0)
        {
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        //
        // GET: /Purchase/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Purchase/Create

        [HttpPost]
        public ActionResult Create( Purchase purchase, FormCollection fm )
        {
            if (ModelState.IsValid)
            {
                purchase.Sup = db.Suppliers.Where( x => x.SupplierID == purchase.SupId ).FirstOrDefault( );
                db.Purchase.Add(purchase);
                db.SaveChanges();
                int count = Convert.ToInt32( fm["count"] );
                if( count > 0 ) {
                    string item = fm["_item"];
                    string price = fm["_price"];
                    string qtys = fm["_quantity"];
                    string total = fm["_total"];
                    string[] arritem = item.Split( ',' );
                    string[] arrprice = price.Split( ',' );
                    string[] qty = qtys.Split( ',' );
                    string[] unt = total.Split( ',' );
                    for( int i = 1; i < unt.Length; i++ ) {
                        PurchaseDetails PurchaseDetail = new PurchaseDetails( );
                        PurchaseDetail.PurID = purchase.PurchaseID;
                        PurchaseDetail.Pur = purchase;
                        PurchaseDetail.Item = arritem[i];
                        PurchaseDetail.Price = Convert.ToDecimal( arrprice[i] );
                        PurchaseDetail.Quantity = Convert.ToDecimal( qty[i] );
                        PurchaseDetail.Total = Convert.ToDecimal( unt[i] );
                        db.PurchaseDetails.Add( PurchaseDetail );
                        db.SaveChanges( );
                    }
                }
                return RedirectToAction("Index");
            }

            return View(purchase);
        }

        //
        // GET: /Purchase/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        //
        // POST: /Purchase/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchase);
        }

        //
        // GET: /Purchase/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        //
        // POST: /Purchase/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var lst = db.PurchaseDetails.Select( s => s )
                .Where( z => z.PurID == id );
            foreach( var item in lst ) {
                db.PurchaseDetails.Remove( item );
            }
            Purchase purchase = db.Purchase.Find(id);
            db.Purchase.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult SearchSupplier( string term ) {
            var objCustomerlist = ( from r in db.Suppliers
                                    where r.Name.ToLower( ).Contains( term.ToLower( ) )
                                    select new { label = r.Name, val = r.SupplierID } ).ToList( );
            return Json( objCustomerlist, JsonRequestBehavior.AllowGet );
        }
    }
}