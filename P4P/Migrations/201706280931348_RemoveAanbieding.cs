namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAanbieding : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BestellingProducts", "Aanbiedingen_Id", "dbo.Aanbiedingens");
            DropForeignKey("dbo.Aanbiedingens", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropForeignKey("dbo.Aanbiedingens", "ProductId_Id", "dbo.Products");
            DropForeignKey("dbo.Aanbiedingens", "Subcategorie_Id", "dbo.Subcategories");
            DropIndex("dbo.Aanbiedingens", new[] { "Hoofdcategorie_Id" });
            DropIndex("dbo.Aanbiedingens", new[] { "ProductId_Id" });
            DropIndex("dbo.Aanbiedingens", new[] { "Subcategorie_Id" });
            DropIndex("dbo.BestellingProducts", new[] { "Aanbiedingen_Id" });
            DropColumn("dbo.BestellingProducts", "Aanbiedingen_Id");
            DropTable("dbo.Aanbiedingens");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Aanbiedingens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Prijs = c.Double(nullable: false),
                        Beschrijving = c.String(nullable: false, maxLength: 255),
                        Naam = c.String(nullable: false, maxLength: 255),
                        Verkoopeenheid = c.String(nullable: false, maxLength: 255),
                        Code = c.String(nullable: false, maxLength: 255),
                        Specificaties = c.String(nullable: false, maxLength: 255),
                        Hoofdcategorie_Id = c.Int(),
                        ProductId_Id = c.Int(),
                        Subcategorie_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BestellingProducts", "Aanbiedingen_Id", c => c.Int());
            CreateIndex("dbo.BestellingProducts", "Aanbiedingen_Id");
            CreateIndex("dbo.Aanbiedingens", "Subcategorie_Id");
            CreateIndex("dbo.Aanbiedingens", "ProductId_Id");
            CreateIndex("dbo.Aanbiedingens", "Hoofdcategorie_Id");
            AddForeignKey("dbo.Aanbiedingens", "Subcategorie_Id", "dbo.Subcategories", "Id");
            AddForeignKey("dbo.Aanbiedingens", "ProductId_Id", "dbo.Products", "Id");
            AddForeignKey("dbo.Aanbiedingens", "Hoofdcategorie_Id", "dbo.Hoofdcategories", "Id");
            AddForeignKey("dbo.BestellingProducts", "Aanbiedingen_Id", "dbo.Aanbiedingens", "Id");
        }
    }
}
