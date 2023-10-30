namespace ams.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        DateAdded = c.DateTime(),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Contact = c.String(),
                        Contact1 = c.String(),
                        Contact2 = c.String(),
                        Contact3 = c.String(),
                        AlertStatus = c.Int(nullable: false),
                        AlertDays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SupplierID);

            //CreateTable( "dbo.PaymentAdjustments",
            //    x => new {
            //        AdjustmentID = x.Int( nullable: false, identity: true ),
            //        CusSupID = x.Int( nullable: false ),
            //        Amount = x.Decimal( precision: 18, scale: 2 ),
            //        Remarks = x.String( ),
            //        type = x.String( ),
            //        Date = x.DateTime( )
            //    } );

            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false, identity: true),
                        BillNo = c.String(),
                        Date = c.DateTime(),
                        CSId = c.Int(nullable: false),
                        GAmount = c.Decimal(precision: 18, scale: 2),
                        Received = c.Decimal(precision: 18, scale: 2),
                        Balance = c.Decimal(precision: 18, scale: 2),
                        Cus_CustomerID = c.Int(),
                    })
                .PrimaryKey(t => t.InvoiceID)
                .ForeignKey("dbo.Customers", t => t.Cus_CustomerID)
                .Index(t => t.Cus_CustomerID);
            
            CreateTable(
                "dbo.InvoiceDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Invid = c.Int(nullable: false),
                        Item = c.String(),
                        Quantity = c.Decimal(precision: 18, scale: 2),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Inv_InvoiceID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Inv_InvoiceID)
                .Index(t => t.Inv_InvoiceID);
            
            AddColumn("dbo.Customers", "City", c => c.String());
            AddColumn("dbo.Customers", "Contact1", c => c.String());
            AddColumn("dbo.Customers", "Contact2", c => c.String());
            AddColumn("dbo.Customers", "Contact3", c => c.String());
            AddColumn("dbo.Customers", "AlertStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "AlertDays", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "OpeningBalance", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "PendingDate", c => c.DateTime());
            AddColumn("dbo.Ledgers", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.Ledgers", "BillNo", c => c.String());
            AddColumn("dbo.Ledgers", "isCheque", c => c.Int(nullable: false));
            AddColumn("dbo.Ledgers", "isSeen", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.InvoiceDetails", new[] { "Inv_InvoiceID" });
            DropIndex("dbo.Invoices", new[] { "Cus_CustomerID" });
            DropForeignKey("dbo.InvoiceDetails", "Inv_InvoiceID", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Cus_CustomerID", "dbo.Customers");
            DropColumn("dbo.Ledgers", "isSeen");
            DropColumn("dbo.Ledgers", "isCheque");
            DropColumn("dbo.Ledgers", "BillNo");
            DropColumn("dbo.Ledgers", "CustomerId");
            DropColumn("dbo.Customers", "PendingDate");
            DropColumn("dbo.Customers", "OpeningBalance");
            DropColumn("dbo.Customers", "AlertDays");
            DropColumn("dbo.Customers", "AlertStatus");
            DropColumn("dbo.Customers", "Contact3");
            DropColumn("dbo.Customers", "Contact2");
            DropColumn("dbo.Customers", "Contact1");
            DropColumn("dbo.Customers", "City");
            DropTable("dbo.InvoiceDetails");
            DropTable("dbo.Invoices");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Users");
        }
    }
}
