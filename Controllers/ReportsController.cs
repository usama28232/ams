using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using ams.Models;
using System.Data.SqlClient;
using System.Data.Objects;

namespace ams.Controllers
{
    public class ReportsController : Controller
    {
        private AMSDBContext db = new AMSDBContext();

        //
        // GET: /Reports/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Reporting/CustomerList

        public ActionResult CustomerList()
        {
            LocalReport lr = new LocalReport();
            lr.ReportPath = Server.MapPath("~/Reports/rptCustomers.rdlc");

            List<Customer> ls = new List<Customer>();
            ls = db.Customer.ToList();

            ReportDataSource rd = new ReportDataSource("dtCustomers", ls);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        //
        // GET: /Reporting/CustomerList

        public ActionResult Customer(int id = 0)
        {
            LocalReport lr = new LocalReport();
            lr.ReportPath = Server.MapPath("~/Reports/rptCustomer.rdlc");

            var ls = db.Customer.Select(x => x).Where(z => z.CustomerID == id);
            ReportDataSource rd = new ReportDataSource("dtCustomer", ls.ToList());
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);

            //List<object> dataList = (List<object>)ls;
            //return Print("~/Reports/rptCustomer.rdlc", "dtCustomer", dataList);
        }

        //
        // GET: /Reports/CustomerLedger

        [HttpGet]
        public ActionResult CustomerLedger()
        {
            //--- Get Parameters
            int id = Convert.ToInt32(this.Request.QueryString["CustomerId"]);
            string fromd = this.Request.QueryString["From"];
            string tod = this.Request.QueryString["To"];

            Customer cs = db.Customer.Select(x => x).Where(x => x.CustomerID == id).FirstOrDefault();

            if (cs.Address == null)
            {
                cs.Address = "-";
            }
            if (cs.City == null)
            {
                cs.City = "-";
            }

            //--- Set Report Parameters
            ReportParameter rp1 = new ReportParameter("From", fromd);
            ReportParameter rp2 = new ReportParameter("To", tod);
            ReportParameter rp3 = new ReportParameter("Name", cs.Name);
            ReportParameter rp4 = new ReportParameter("Address", cs.Address);
            ReportParameter rp5 = new ReportParameter("Contact", cs.Contact + ", " + cs.Contact1 + ", " + cs.Contact2 + ", " + cs.Contact3);
            ReportParameter rp6 = new ReportParameter("City", cs.City);
            
            LocalReport lr = new LocalReport();
            lr.ReportPath = Server.MapPath("~/Reports/rptCustomerLedger.rdlc");
            DateTime fromDate, toDate;

            try
            {
                fromDate = Convert.ToDateTime(fromd);
                toDate = Convert.ToDateTime(tod);
            }
            catch (Exception)
            {
                return HttpNotFound("From or To Date cannot be null");
            }

            var param_CustomerId = new SqlParameter("@CustomerId", id);
            var ledger = db.Database.SqlQuery<Ledger>("SP_CustomerLedger @CustomerId", param_CustomerId).ToList();

            var ls = ledger.Select(x => x).Where(z => z.Date >= fromDate && z.Date <= toDate);

            ReportDataSource rd = new ReportDataSource("dtCustomerLedger", ls.ToList());
            lr.DataSources.Add(rd);
            lr.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6 });

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.4in</MarginLeft>" +
                "  <MarginRight>0.4in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }


        //
        // GET: /Reports/RptInvoice

        [HttpGet]
        public ActionResult RptInvoice( ) {

            try {
                int id = 0;
                try {
                    string[] Url = this.Request.Path.Split( '/' );
                    id = Convert.ToInt32( Url[( Url.Length - 1 )] );
                } catch( Exception ) {
                    return HttpNotFound( "There was an Error in your Request, Please Contact Software Vendor" );
                }

                Invoice inv = db.Invoices.Select( x => x ).Where( x => x.InvoiceID == id ).FirstOrDefault( );
                if( inv==null ) {
                    return HttpNotFound( "Could Not Find Invoice #" + id );
                }

                ////--- Set Report Parameters
                //ReportParameter rp1 = new ReportParameter( "InvoiceId", inv.InvoiceID.ToString( ) );
                //ReportParameter rp2 = new ReportParameter( "BillNo", inv.BillNo );
                //ReportParameter rp3 = new ReportParameter( "Name", inv.Cus.Name );
                //ReportParameter rp4 = new ReportParameter( "Address", inv.Cus.Address );
                //ReportParameter rp5 = new ReportParameter( "Contact", inv.Cus.Contact + ", " + inv.Cus.Contact1 + ", " +
                //        inv.Cus.Contact2 + ", " + inv.Cus.Contact3 );
                //ReportParameter rp6 = new ReportParameter( "City", inv.Cus.City );
                //ReportParameter rp7 = new ReportParameter( "Date", inv.Date.ToString( ) );
                //ReportParameter rp8 = new ReportParameter( "OBalance", inv.Cus.OpeningBalance.ToString( ) );
                //ReportParameter rp9 = new ReportParameter( "Total", inv.GAmount.ToString( ) );
                //ReportParameter rp0 = new ReportParameter( "Rece", ( inv.Received * -1 ).ToString( ) );
                //ReportParameter rpx = new ReportParameter( "Balance", inv.Balance.ToString( ) );

                LocalReport lr = new LocalReport( );
                lr.ReportPath = Server.MapPath( "~/Reports/rptCustomerInv.rdlc" );

                 //db.InvoicesDetails.Select(x=>x)

                var ls = db.InvoicesDetails.Where( x => x.Invid == inv.InvoiceID ).Join(
                        db.Invoices,
                        comp => comp.Invid,
                        sect => sect.InvoiceID,
                        ( comp, sect ) => new { comp, sect } )
                    .Join(
                        db.Customer.Where( dsi => dsi.CustomerID == inv.CSId ),
                        cs => cs.sect.CSId,
                        dsi => dsi.CustomerID,
                        ( cs, dsi ) => new { cs, dsi } )
                    .Select( c => new {
                        c.cs.comp.Id, c.cs.comp.Inv, c.cs.comp.Invid, c.cs.comp.Item, c.cs.comp.Price, c.cs.comp.Quantity,
                        c.cs.comp.Total, c.cs.sect.Balance, c.cs.sect.BillNo, c.cs.sect.CSId, c.cs.sect.Cus, c.cs.sect.Date,
                        c.cs.sect.GAmount, c.cs.sect.InvoiceID, c.cs.sect.Received, c.dsi.Address, c.dsi, c.dsi.City,
                        c.dsi.CustomerID, c.dsi.Name, c.dsi.OpeningBalance, c.dsi.AlertStatus, c.dsi.AlertDays, c.dsi.PendingDate,
                        c.dsi.Contact, c.dsi.Contact1, c.dsi.Contact2, c.dsi.Contact3
                    } );



                ReportDataSource rd = new ReportDataSource( "dsCustInv", ls.ToList( ) );
                lr.DataSources.Add( rd );

                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>0.4in</MarginLeft>" +
                    "  <MarginRight>0.4in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render( reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings );
                return File( renderedBytes, mimeType );
            } catch( Exception ex) {
                return HttpNotFound( "Could Not generate Report. Error: " + ex.ToString());
            }
            //--- Get Parameters
        }


        //
        // GET: /Reports/RptPurchase

        [HttpGet]
        public ActionResult RptPurchase( ) {

            try {
                int id = 0;
                try {
                    string[] Url = this.Request.Path.Split( '/' );
                    id = Convert.ToInt32( Url[( Url.Length - 1 )] );
                } catch( Exception ) {
                    return HttpNotFound( "There was an Error in your Request, Please Contact Software Vendor" );
                }

                Purchase inv = db.Purchase.Select( x => x ).Where( x => x.PurchaseID == id ).FirstOrDefault( );
                if( inv == null ) {
                    return HttpNotFound( "Could Not Find Invoice #" + id );
                }

                LocalReport lr = new LocalReport( );
                lr.ReportPath = Server.MapPath( "~/Reports/rptSupPurchase.rdlc" );

                var ls = db.PurchaseDetails.Where( x => x.PurID == inv.PurchaseID ).Join(
                        db.Purchase,
                        comp => comp.PurID,
                        sect => sect.PurchaseID,
                        ( comp, sect ) => new { comp, sect } )
                    .Join(
                        db.Suppliers.Where( dsi => dsi.SupplierID == inv.SupId ),
                        cs => cs.sect.SupId,
                        dsi => dsi.SupplierID,
                        ( cs, dsi ) => new { cs, dsi } )
                    .Select( c => new {
                        c.cs.comp.Id, c.cs.comp.Pur, c.cs.comp.PurID, c.cs.comp.Item, c.cs.comp.Price, c.cs.comp.Quantity,
                        c.cs.comp.Total, c.cs.sect.Balance, c.cs.sect.BillNo, c.cs.sect.SupId, c.cs.sect.Sup, c.cs.sect.Date,
                        c.cs.sect.GAmount, c.cs.sect.PurchaseID, c.cs.sect.Received, c.dsi.Address, c.dsi, c.dsi.City,
                        c.dsi.SupplierID, c.dsi.Name, c.dsi.AlertStatus, c.dsi.AlertDays,
                        c.dsi.Contact, c.dsi.Contact1, c.dsi.Contact2, c.dsi.Contact3
                    } );



                ReportDataSource rd = new ReportDataSource( "dsSupPurchase", ls.ToList( ) );
                lr.DataSources.Add( rd );

                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>8.5in</PageWidth>" +
                    "  <PageHeight>11in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>0.4in</MarginLeft>" +
                    "  <MarginRight>0.4in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render( reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings );
                return File( renderedBytes, mimeType );
            } catch( Exception ex ) {
                return HttpNotFound( "Could Not generate Report. Error: " + ex.ToString( ) );
            }
            //--- Get Parameters
        }


        //
        // GET: /Reports/CustomerAlerts

        public ActionResult CustomerAlerts()
        {
            //--- Update Pending Customer Dates
            

            //--- Set Report Parameters
            //ReportParameter rp1 = new ReportParameter("To", DateTime.Now.ToString());
            //ReportParameter rp2 = new ReportParameter("From", DateTime.Now.AddDays(-30).ToString());

            LocalReport lr = new LocalReport();
            lr.ReportPath = Server.MapPath("~/Reports/rptCustomerAlerts.rdlc");

            DateTime dateLastBill = DateTime.Now.AddMonths(-6);
            DateTime dateSixMonthsBefore = dateLastBill.AddMonths(-6);

            List<int> cids = (List<int>)TempData["cids"];

            var lstCustomerAlert = db.Ledger
                                .Join(db.Customer, l => l.CustomerId, c => c.CustomerID, (x, y) => new
                                {
                                    CustomerId = y.CustomerID,
                                    Name = y.Name,
                                    Address = y.Address,
                                    Contact = y.Contact,
                                    Date = x.Date,
                                    BillNo = x.BillNo,
                                    Particular = x.Particular,
                                    Debit = x.Debit,
                                    Credit = x.Credit,
                                    Balance = x.Balance
                                })
                                .Where(a => cids.Contains(a.CustomerId) && 
                                    a.Date >= EntityFunctions.AddMonths(db.Ledger.Where(g => g.CustomerId == a.CustomerId).Max(g => g.Date), -6)
                                    )
                                .ToList();

            ReportDataSource rd = new ReportDataSource("dtCustomerAlerts", lstCustomerAlert.ToList());
            lr.DataSources.Add(rd);
            //lr.SetParameters(new ReportParameter[] { rp1, rp2 });

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                            "<DeviceInfo>" +
                            "  <OutputFormat>PDF</OutputFormat>" +
                            "  <PageWidth>8.5in</PageWidth>" +
                            "  <PageHeight>11in</PageHeight>" +
                            "  <MarginTop>0.5in</MarginTop>" +
                            "  <MarginLeft>0.4in</MarginLeft>" +
                            "  <MarginRight>0.4in</MarginRight>" +
                            "  <MarginBottom>0.5in</MarginBottom>" +
                            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }


        //
        // Method for Printing Reports

        //private FileContentResult Print(string _path, string _dtName, List<object> _ls)
        //{
        //    LocalReport lr = new LocalReport();
        //    lr.ReportPath = Server.MapPath(_path);

        //    ReportDataSource rd = new ReportDataSource(_dtName, _ls);
        //    lr.DataSources.Add(rd);

        //    string reportType = "pdf";
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;
        //    string deviceInfo =
        //        "<DeviceInfo>" +
        //        "  <OutputFormat>PDF</OutputFormat>" +
        //        "  <PageWidth>8.5in</PageWidth>" +
        //        "  <PageHeight>11in</PageHeight>" +
        //        "  <MarginTop>0.5in</MarginTop>" +
        //        "  <MarginLeft>0.5in</MarginLeft>" +
        //        "  <MarginRight>0.5in</MarginRight>" +
        //        "  <MarginBottom>0.5in</MarginBottom>" +
        //        "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        //    return File(renderedBytes, mimeType);
        //}
    }
}
