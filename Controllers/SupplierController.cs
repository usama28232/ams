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
    public class SupplierController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        //
        // GET: /Supplier/

        public ActionResult Index()
        {
            return View(db.Suppliers.ToList());
        }

        //
        // GET: /Supplier/Details/5

        public ActionResult Details(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Supplier/Create

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        //
        // GET: /Supplier/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /Supplier/Edit/5

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /Supplier/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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
            Supplier supplier = db.Suppliers.Find(id);
            supplier.AlertStatus = 1;
            db.Entry(supplier).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Request Successful, Notifications for " + supplier.Name + " are On now");
        }

        public ActionResult TurnOffNotifications(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            supplier.AlertStatus = 0;
            db.Entry(supplier).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Request Successful, Notifications for " + supplier.Name + " are Off now");
        }


        public ActionResult IsNameAvailble(string _Name)
        {
            try
            {
                var tag = db.Suppliers.Single(x => x.Name == _Name);
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