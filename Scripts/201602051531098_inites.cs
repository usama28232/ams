namespace ams.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Contact = c.String(),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            CreateTable(
                "dbo.Ledgers",
                c => new
                    {
                        LedgerID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Particular = c.String(),
                        Debit = c.Int(nullable: false),
                        Credit = c.Int(nullable: false),
                        Balance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LedgerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ledgers");
            DropTable("dbo.Customers");
        }
    }
}
