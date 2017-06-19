namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Winkelwagen : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Winkelwagens", "Product_Id", "dbo.Products");
            DropIndex("dbo.Winkelwagens", new[] { "Gebruiker_Id" });
            DropPrimaryKey("dbo.Winkelwagens");
            AddPrimaryKey("dbo.Winkelwagens", new[] { "Gebruiker_id", "Product_Id" });
            CreateIndex("dbo.Winkelwagens", "Gebruiker_id");
            AddForeignKey("dbo.Winkelwagens", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Winkelwagens", "Product_Id", "dbo.Products");
            DropIndex("dbo.Winkelwagens", new[] { "Gebruiker_id" });
            DropPrimaryKey("dbo.Winkelwagens");
            AddPrimaryKey("dbo.Winkelwagens", "Product_Id");
            CreateIndex("dbo.Winkelwagens", "Gebruiker_Id");
            AddForeignKey("dbo.Winkelwagens", "Product_Id", "dbo.Products", "Id");
        }
    }
}
