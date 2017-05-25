namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProductWithFavorietenlijsts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Favorietenlijst_Id", "dbo.Favorietenlijsts");
            DropIndex("dbo.Products", new[] { "Favorietenlijst_Id" });
            CreateTable(
                "dbo.ProductFavorietenlijsts",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Favorietenlijst_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Favorietenlijst_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Favorietenlijsts", t => t.Favorietenlijst_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Favorietenlijst_Id);
            
            DropColumn("dbo.Products", "Favorietenlijst_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Favorietenlijst_Id", c => c.Int());
            DropForeignKey("dbo.ProductFavorietenlijsts", "Favorietenlijst_Id", "dbo.Favorietenlijsts");
            DropForeignKey("dbo.ProductFavorietenlijsts", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductFavorietenlijsts", new[] { "Favorietenlijst_Id" });
            DropIndex("dbo.ProductFavorietenlijsts", new[] { "Product_Id" });
            DropTable("dbo.ProductFavorietenlijsts");
            CreateIndex("dbo.Products", "Favorietenlijst_Id");
            AddForeignKey("dbo.Products", "Favorietenlijst_Id", "dbo.Favorietenlijsts", "Id");
        }
    }
}
