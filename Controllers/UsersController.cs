using ams.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Controllers
{
    public class UsersController : Controller
    {
        private AMSDBContext db = new AMSDBContext();
        //
        // GET: /Users/

        public ActionResult Index()
        {
            Session["user"] = db.User.Select(x=>x).Single().ToString();
            return RedirectToAction( "Index", "Home" );
            //return View();
        }

        public ActionResult Validate(string usern, string pw)
        {
            string status = null;
            User u = db.User.Select(x => x).Where(x => x.Username == usern && x.Password == pw).FirstOrDefault();
            if (u == null)
            {
                status = "N";
            }
            else
            {
                Session["user"] = u.ID;
                status = "Y";
            }
            return Json(status);
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Users");
        }

        public ActionResult Changepassword()
        {
            User usr = db.User.Find(Convert.ToInt32(Session["user"]));
            if(usr == null){
                return RedirectToAction("Index", "Users");
            }
            return View(usr);
        }

        public ActionResult Change(string pwo, string pwn)
        {
            string sts = null;
            int id = Convert.ToInt32(Session["user"]);
            User usr = db.User.Where(x => x.ID == id && x.Password == pwo).FirstOrDefault();

            if (usr == null)
            {
                //sts = "N :- Session Id: " + id + ", Old Password: " + pwo + ", New Password: " + pwn;
                sts = "N";
                return Json(sts);
            }

            // Code here
            usr.Password = pwn;
            //db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            sts = "Y";
            return Json(sts);
        }
    }
}
