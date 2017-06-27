namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_aanbiedingen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Aanbiedingens", "Naam", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Aanbiedingens", "Verkoopeenheid", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Aanbiedingens", "Code", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Aanbiedingens", "Specificaties", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Aanbiedingens", "Hoofdcategorie_Id", c => c.Int());
            AddColumn("dbo.Aanbiedingens", "Subcategorie_Id", c => c.Int());
            AddColumn("dbo.Products", "Beschikbaar", c => c.Boolean(nullable: false, defaultValue:true));
            AddColumn("dbo.BestellingProducts", "Aanbiedingen_Id", c => c.Int());
            CreateIndex("dbo.Aanbiedingens", "Hoofdcategorie_Id");
            CreateIndex("dbo.Aanbiedingens", "Subcategorie_Id");
            CreateIndex("dbo.BestellingProducts", "Aanbiedingen_Id");
            AddForeignKey("dbo.BestellingProducts", "Aanbiedingen_Id", "dbo.Aanbiedingens", "Id");
            AddForeignKey("dbo.Aanbiedingens", "Hoofdcategorie_Id", "dbo.Hoofdcategories", "Id");
            AddForeignKey("dbo.Aanbiedingens", "Subcategorie_Id", "dbo.Subcategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Aanbiedingens", "Subcategorie_Id", "dbo.Subcategories");
            DropForeignKey("dbo.Aanbiedingens", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropForeignKey("dbo.BestellingProducts", "Aanbiedingen_Id", "dbo.Aanbiedingens");
            DropIndex("dbo.BestellingProducts", new[] { "Aanbiedingen_Id" });
            DropIndex("dbo.Aanbiedingens", new[] { "Subcategorie_Id" });
            DropIndex("dbo.Aanbiedingens", new[] { "Hoofdcategorie_Id" });
            DropColumn("dbo.BestellingProducts", "Aanbiedingen_Id");
            DropColumn("dbo.Products", "Beschikbaar");
            DropColumn("dbo.Aanbiedingens", "Subcategorie_Id");
            DropColumn("dbo.Aanbiedingens", "Hoofdcategorie_Id");
            DropColumn("dbo.Aanbiedingens", "Specificaties");
            DropColumn("dbo.Aanbiedingens", "Code");
            DropColumn("dbo.Aanbiedingens", "Verkoopeenheid");
            DropColumn("dbo.Aanbiedingens", "Naam");
        }
    }
}
