using ams.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Controllers
{
    public class HomeController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        public ActionResult Index()
        {
            DateTime dateToday = DateTime.Now.Date;

            int cntChequesToday = db.Ledger.Count(x => x.isCheque == 1 && x.Date == dateToday && x.isSeen==0);
            int cntChequesPending = db.Ledger.Count(x => x.isCheque == 1 && x.Date > dateToday && x.isSeen == 0);

            ViewBag.ChequesToday = cntChequesToday.ToString();
            ViewBag.ChequesPending = cntChequesPending.ToString();
            ViewBag.User = "Admin";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
