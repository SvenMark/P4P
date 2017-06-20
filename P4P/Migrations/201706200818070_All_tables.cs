namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class All_tables : DbMigration
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
                "dbo.BestellingProducts",
                c => new
                    {
                        Bestelling_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                        Aantal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Bestelling_Id, t.Product_Id })
                .ForeignKey("dbo.Bestellings", t => t.Bestelling_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Bestelling_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Bestellings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Prijs = c.Double(nullable: false),
                        Afleverdatum = c.DateTime(),
                        Opmerking = c.String(maxLength: 255),
                        Afgerond = c.Boolean(nullable: false),
                        Bedrijf_Id = c.Int(),
                        Gebruiker_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bedrijfs", t => t.Bedrijf_Id)
                .ForeignKey("dbo.Gebruikers", t => t.Gebruiker_Id, cascadeDelete: true)
                .Index(t => t.Bedrijf_Id)
                .Index(t => t.Gebruiker_Id);
            
            CreateTable(
                "dbo.Bedrijfs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false, maxLength: 255),
                        Adres = c.String(nullable: false, maxLength: 255),
                        Postcode = c.String(nullable: false, maxLength: 255),
                        Plaats = c.String(nullable: false, maxLength: 255),
                        Telefoonnummer = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        Adres = c.String(nullable: false, maxLength: 255),
                        Postcode = c.String(nullable: false, maxLength: 255),
                        Woonplaats = c.String(nullable: false, maxLength: 255),
                        Wachtwoord = c.String(maxLength: 255),
                        Token = c.String(maxLength: 255),
                        Rol = c.String(nullable: false, maxLength: 255),
                        BedrijfId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bedrijfs", t => t.BedrijfId, cascadeDelete: true)
                .Index(t => t.BedrijfId);
            
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
                "dbo.Meldings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Winkelwagens",
                c => new
                    {
                        Gebruiker_id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                        Aantal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Gebruiker_id, t.Product_Id })
                .ForeignKey("dbo.Gebruikers", t => t.Gebruiker_id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Gebruiker_id)
                .Index(t => t.Product_Id);
            
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
            DropForeignKey("dbo.Winkelwagens", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Winkelwagens", "Gebruiker_id", "dbo.Gebruikers");
            DropForeignKey("dbo.Aanbiedingens", "ProductId_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Subcategorie_Id", "dbo.Subcategories");
            DropForeignKey("dbo.Subcategories", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropForeignKey("dbo.Products", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropForeignKey("dbo.FavorietenlijstProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.FavorietenlijstProducts", "Favorietenlijst_Id", "dbo.Favorietenlijsts");
            DropForeignKey("dbo.Favorietenlijsts", "Gebruiker_Id", "dbo.Gebruikers");
            DropForeignKey("dbo.BestellingProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.BestellingProducts", "Bestelling_Id", "dbo.Bestellings");
            DropForeignKey("dbo.Bestellings", "Gebruiker_Id", "dbo.Gebruikers");
            DropForeignKey("dbo.Gebruikers", "BedrijfId", "dbo.Bedrijfs");
            DropForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs");
            DropIndex("dbo.FavorietenlijstProducts", new[] { "Product_Id" });
            DropIndex("dbo.FavorietenlijstProducts", new[] { "Favorietenlijst_Id" });
            DropIndex("dbo.Winkelwagens", new[] { "Product_Id" });
            DropIndex("dbo.Winkelwagens", new[] { "Gebruiker_id" });
            DropIndex("dbo.Subcategories", new[] { "Hoofdcategorie_Id" });
            DropIndex("dbo.Favorietenlijsts", new[] { "Gebruiker_Id" });
            DropIndex("dbo.Gebruikers", new[] { "BedrijfId" });
            DropIndex("dbo.Bestellings", new[] { "Gebruiker_Id" });
            DropIndex("dbo.Bestellings", new[] { "Bedrijf_Id" });
            DropIndex("dbo.BestellingProducts", new[] { "Product_Id" });
            DropIndex("dbo.BestellingProducts", new[] { "Bestelling_Id" });
            DropIndex("dbo.Products", new[] { "Subcategorie_Id" });
            DropIndex("dbo.Products", new[] { "Hoofdcategorie_Id" });
            DropIndex("dbo.Aanbiedingens", new[] { "ProductId_Id" });
            DropTable("dbo.FavorietenlijstProducts");
            DropTable("dbo.Winkelwagens");
            DropTable("dbo.Meldings");
            DropTable("dbo.Contacts");
            DropTable("dbo.Subcategories");
            DropTable("dbo.Hoofdcategories");
            DropTable("dbo.Favorietenlijsts");
            DropTable("dbo.Gebruikers");
            DropTable("dbo.Bedrijfs");
            DropTable("dbo.Bestellings");
            DropTable("dbo.BestellingProducts");
            DropTable("dbo.Products");
            DropTable("dbo.Aanbiedingens");
        }
    }
}
