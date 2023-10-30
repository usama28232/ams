using ams.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Controllers
{
    public class BackupController : Controller
    {
        //
        // GET: /Backup/
        private AMSDBContext db = new AMSDBContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create() 
        {
            switch (Request["action"].ToString()) 
            { 
                case "Create":
                    var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    var dbName = sqlDB.InitialCatalog;
                    string path = @"D:\BACKUP";
                    
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    
                    path += @"\" + dbName.Replace('-', '_') + " (" + DateTime.Now.ToString("ddMMMMyyyy - H.mm.ss") + ")";
                    Directory.CreateDirectory(path);
                    path += @"\"+dbName.Replace('-', '_') + ".bak";
                    string command = "BACKUP DATABASE \"" + dbName + "\" TO DISK = '"+ path + "' WITH INIT, NAME= 'abcd1', NOSKIP, NOFORMAT";
                    SqlCommand oCommand = null;
                    SqlConnection oConnection = new SqlConnection(sqlDB.ConnectionString);
                    
                    if (oConnection.State != ConnectionState.Open)
                        oConnection.Open();
                    
                    oCommand = new SqlCommand(command, oConnection);
                    oCommand.ExecuteNonQuery();
                    return RedirectToAction("Done", "Backup");
                    break;
                
                case "Cancel":
                    return RedirectToAction("Index", "Home");
                    break;

                default:
                    return RedirectToAction("Index", "Users");
                    break;
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Done()
        {
            return View();
        }
    }
}
