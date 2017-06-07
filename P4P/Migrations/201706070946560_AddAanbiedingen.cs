namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAanbiedingen : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductFavorietenlijsts", newName: "FavorietenlijstProducts");
            DropPrimaryKey("dbo.FavorietenlijstProducts");
            CreateTable(
                "dbo.Aanbiedingens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Prijs = c.Double(nullable: false),
                        Beschrijving = c.String(nullable: false, maxLength: 255),
                        ProductId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId_Id)
                .Index(t => t.ProductId_Id);
            
            AddPrimaryKey("dbo.FavorietenlijstProducts", new[] { "Favorietenlijst_Id", "Product_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Aanbiedingens", "ProductId_Id", "dbo.Products");
            DropIndex("dbo.Aanbiedingens", new[] { "ProductId_Id" });
            DropPrimaryKey("dbo.FavorietenlijstProducts");
            DropTable("dbo.Aanbiedingens");
            AddPrimaryKey("dbo.FavorietenlijstProducts", new[] { "Product_Id", "Favorietenlijst_Id" });
            RenameTable(name: "dbo.FavorietenlijstProducts", newName: "ProductFavorietenlijsts");
        }
    }
}
