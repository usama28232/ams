using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ams.Models
{
    public class AMSDBContext : DbContext
    {
        public AMSDBContext() : base("name=DefaultConnection")
        {

        }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Ledger> Ledger { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetails> InvoicesDetails { get; set; }

        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<PurchaseDetails> PurchaseDetails { get; set; }

        public DbSet<PaymentAdjustment> PaymentAdjustment { get; set; }

    }
}