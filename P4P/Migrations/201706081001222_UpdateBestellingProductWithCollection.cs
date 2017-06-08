namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBestellingProductWithCollection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BestellingProducts",
                c => new
                    {
                        Bestelling_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Bestelling_Id, t.Product_Id })
                .ForeignKey("dbo.Bestellings", t => t.Bestelling_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Bestelling_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BestellingProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.BestellingProducts", "Bestelling_Id", "dbo.Bestellings");
            DropIndex("dbo.BestellingProducts", new[] { "Product_Id" });
            DropIndex("dbo.BestellingProducts", new[] { "Bestelling_Id" });
            DropTable("dbo.BestellingProducts");
        }
    }
}
