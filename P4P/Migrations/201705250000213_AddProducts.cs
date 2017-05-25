namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProducts : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Subcategorie_Id", "dbo.Subcategories");
            DropForeignKey("dbo.Products", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropIndex("dbo.Products", new[] { "Subcategorie_Id" });
            DropIndex("dbo.Products", new[] { "Hoofdcategorie_Id" });
            DropTable("dbo.Products");
        }
    }
}
