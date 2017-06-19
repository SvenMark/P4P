namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateWinkelwagen : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Winkelwagens",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Aantal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Product_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Winkelwagens", "Product_Id", "dbo.Products");
            DropIndex("dbo.Winkelwagens", new[] { "Product_Id" });
            DropTable("dbo.Winkelwagens");
        }
    }
}
