using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ams.Models;
using System.Data.SqlClient;

namespace ams.Controllers
{
    public class PaymentAdjustmentController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        //
        // GET: /PaymentAdjustment/

        public ActionResult Index()
        {
            var lst = ( from pa in db.PaymentAdjustment
                        select new {
                            AdjustmentID = pa.AdjustmentID,
                            Amount = pa.Amount,
                            CusSupID = pa.CusSupID,
                            Date = pa.Date,
                            Remarks = pa.Remarks,
                            type = pa.type.Equals( "c" ) ? ( from c in db.Customer where pa.CusSupID.Equals( c.CustomerID ) select c.Name ).FirstOrDefault( ) :
                            ( from s in db.Suppliers where pa.CusSupID.Equals( s.SupplierID ) select s.Name ).FirstOrDefault( )
                        } )
                        .AsEnumerable( ).Select( t => new PaymentAdjustment {
                            AdjustmentID = t.AdjustmentID,
                            Amount = t.Amount,
                            CusSupID = t.CusSupID,
                            Date = t.Date,
                            Remarks = t.Remarks,
                            type = t.type
                        } )
                        .ToList( );
            return View( lst );
        }

        //
        // GET: /PaymentAdjustment/Details/5

        public ActionResult Details(int id = 0)
        {
            PaymentAdjustment paymentadjustment = db.PaymentAdjustment.Find(id);
            if (paymentadjustment == null)
            {
                return HttpNotFound();
            }
            return View(paymentadjustment);
        }

        //
        // GET: /PaymentAdjustment/Create

        public ActionResult Create()
        {
            //ViewBag.Balance = "0.00";
            return View();
        }

        //
        // POST: /PaymentAdjustment/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( PaymentAdjustment paymentadjustment, FormCollection fm ) {
            if( ModelState.IsValid ) {
                string[] arr = Request.Form.AllKeys;
                string type = "F";
                if( fm["Customer"].ToString( ).Trim( ) != "" || fm["Supplier"].ToString( ).Trim( ) != "" ) {
                    if( fm["Customer"].ToString( ).Trim( ) != "" ) {
                        type = "c";
                    }
                    if( fm["Supplier"].ToString( ).Trim( ) != "" ) {
                        type = "s";
                    }
                } else {
                    return HttpNotFound( "Invalid Request, Please Contact software vendor" );
                }
                int Fkey = Convert.ToInt32( fm["CusSupID"].ToString( ) );
                paymentadjustment.CusSupID = Fkey;
                paymentadjustment.Date = DateTime.Now;
                paymentadjustment.type = type;
                db.PaymentAdjustment.Add( paymentadjustment );
                db.SaveChanges( );
                if( type == "c" ) {
                    Ledger LedgerDebit = new Ledger( );
                    LedgerDebit.BillNo = "PAY_ADJ_" + paymentadjustment.AdjustmentID;
                    LedgerDebit.CustomerId = Fkey;
                    LedgerDebit.isCheque = 0;
                    LedgerDebit.Date = DateTime.Now;
                    LedgerDebit.Debit = Convert.ToInt32( paymentadjustment.Amount );
                    LedgerDebit.Credit = 0;
                    LedgerDebit.Balance = 0;
                    LedgerDebit.isSeen = 0;
                    LedgerDebit.Particular = "Payment Adjustment";
                    db.Ledger.Add( LedgerDebit );
                    db.SaveChanges( );
                }
                return RedirectToAction( "Index" );
            }

            return View( paymentadjustment );
        }

        //
        // GET: /PaymentAdjustment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PaymentAdjustment paymentadjustment = db.PaymentAdjustment.Find(id);
            if (paymentadjustment == null)
            {
                return HttpNotFound();
            }
            return View(paymentadjustment);
        }

        //
        // POST: /PaymentAdjustment/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaymentAdjustment paymentadjustment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentadjustment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentadjustment);
        }

        //
        // GET: /PaymentAdjustment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PaymentAdjustment paymentadjustment = db.PaymentAdjustment.Find(id);
            if (paymentadjustment == null)
            {
                return HttpNotFound();
            }
            return View(paymentadjustment);
        }

        //
        // POST: /PaymentAdjustment/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentAdjustment paymentadjustment = db.PaymentAdjustment.Find(id);
            db.PaymentAdjustment.Remove(paymentadjustment);
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

        public JsonResult GetCustomerBalance( int CustomerID ) {
            double Balance;
            var param_CustomerId = new SqlParameter( "@CustomerId", CustomerID );
            try {
                Balance = db.Database.SqlQuery<Ledger>( "SP_CustomerLedger @CustomerId", param_CustomerId ).Last( ).Balance;
            } catch( Exception ) {
                Balance = 0.00;
            }
            //.ToList( ).Last( ).Balance;
            return Json( Balance );
        }
    }
}