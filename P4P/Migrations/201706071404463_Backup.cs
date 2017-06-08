namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Backup : DbMigration
    {
        public override void Up()
        {
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
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false, maxLength: 255),
                        Prijs = c.Double(nullable: false),
                        Verkoopeenheid = c.String(nullable: false, maxLength: 255),
                        Beschrijving = c.String(nullable: false),
                        Code = c.String(nullable: false, maxLength: 255),
                        Specificaties = c.String(nullable: false),
                        Hoofdcategorie_Id = c.Int(),
                        Subcategorie_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hoofdcategories", t => t.Hoofdcategorie_Id)
                .ForeignKey("dbo.Subcategories", t => t.Subcategorie_Id)
                .Index(t => t.Hoofdcategorie_Id)
                .Index(t => t.Subcategorie_Id);
            
            CreateTable(
                "dbo.Favorietenlijsts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(),
                        Gebruiker_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gebruikers", t => t.Gebruiker_Id)
                .Index(t => t.Gebruiker_Id);
            
            CreateTable(
                "dbo.Gebruikers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Voornaam = c.String(nullable: false, maxLength: 255),
                        Tussenvoegsel = c.String(maxLength: 255),
                        Achternaam = c.String(nullable: false, maxLength: 255),
                        Telefoonnummer = c.String(nullable: false, maxLength: 255),
                        Emailadres = c.String(nullable: false, maxLength: 255),
                        Bedrijfsnaam = c.String(nullable: false, maxLength: 255),
                        Adres = c.String(nullable: false, maxLength: 255),
                        Postcode = c.String(nullable: false, maxLength: 255),
                        Woonplaats = c.String(nullable: false, maxLength: 255),
                        Wachtwoord = c.String(maxLength: 255),
                        LoginToken = c.String(maxLength: 255),
                        Token = c.String(maxLength: 255),
                        Rol = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hoofdcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false, maxLength: 255),
                        Hoofdcategorie_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hoofdcategories", t => t.Hoofdcategorie_Id)
                .Index(t => t.Hoofdcategorie_Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        Message = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FavorietenlijstProducts",
                c => new
                    {
                        Favorietenlijst_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Favorietenlijst_Id, t.Product_Id })
                .ForeignKey("dbo.Favorietenlijsts", t => t.Favorietenlijst_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Favorietenlijst_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Aanbiedingens", "ProductId_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Subcategorie_Id", "dbo.Subcategories");
            DropForeignKey("dbo.Subcategories", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropForeignKey("dbo.Products", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropForeignKey("dbo.FavorietenlijstProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.FavorietenlijstProducts", "Favorietenlijst_Id", "dbo.Favorietenlijsts");
            DropForeignKey("dbo.Favorietenlijsts", "Gebruiker_Id", "dbo.Gebruikers");
            DropIndex("dbo.FavorietenlijstProducts", new[] { "Product_Id" });
            DropIndex("dbo.FavorietenlijstProducts", new[] { "Favorietenlijst_Id" });
            DropIndex("dbo.Subcategories", new[] { "Hoofdcategorie_Id" });
            DropIndex("dbo.Favorietenlijsts", new[] { "Gebruiker_Id" });
            DropIndex("dbo.Products", new[] { "Subcategorie_Id" });
            DropIndex("dbo.Products", new[] { "Hoofdcategorie_Id" });
            DropIndex("dbo.Aanbiedingens", new[] { "ProductId_Id" });
            DropTable("dbo.FavorietenlijstProducts");
            DropTable("dbo.Contacts");
            DropTable("dbo.Subcategories");
            DropTable("dbo.Hoofdcategories");
            DropTable("dbo.Gebruikers");
            DropTable("dbo.Favorietenlijsts");
            DropTable("dbo.Products");
            DropTable("dbo.Aanbiedingens");
        }
    }
}
