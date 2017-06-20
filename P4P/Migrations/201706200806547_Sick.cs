namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sick : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs");
            DropForeignKey("dbo.Gebruikers", "BedrijfId", "dbo.Bedrijfs");
            DropIndex("dbo.Bestellings", new[] { "Bedrijf_Id" });
            DropIndex("dbo.Gebruikers", new[] { "BedrijfId" });
            DropPrimaryKey("dbo.Bedrijfs");
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
            
            AddColumn("dbo.Bedrijfs", "Adres", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Bedrijfs", "Postcode", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Bedrijfs", "Plaats", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Bedrijfs", "Telefoonnummer", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Bestellings", "Bedrijf_Id", c => c.Int());
            AlterColumn("dbo.Bedrijfs", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Gebruikers", "BedrijfId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Bedrijfs", "Id");
            CreateIndex("dbo.Bestellings", "Bedrijf_Id");
            CreateIndex("dbo.Gebruikers", "BedrijfId");
            AddForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs", "Id");
            AddForeignKey("dbo.Gebruikers", "BedrijfId", "dbo.Bedrijfs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gebruikers", "BedrijfId", "dbo.Bedrijfs");
            DropForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs");
            DropForeignKey("dbo.Winkelwagens", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Winkelwagens", "Gebruiker_id", "dbo.Gebruikers");
            DropIndex("dbo.Winkelwagens", new[] { "Product_Id" });
            DropIndex("dbo.Winkelwagens", new[] { "Gebruiker_id" });
            DropIndex("dbo.Gebruikers", new[] { "BedrijfId" });
            DropIndex("dbo.Bestellings", new[] { "Bedrijf_Id" });
            DropPrimaryKey("dbo.Bedrijfs");
            AlterColumn("dbo.Gebruikers", "BedrijfId", c => c.Byte(nullable: false));
            AlterColumn("dbo.Bedrijfs", "Id", c => c.Byte(nullable: false));
            AlterColumn("dbo.Bestellings", "Bedrijf_Id", c => c.Byte());
            DropColumn("dbo.Bedrijfs", "Telefoonnummer");
            DropColumn("dbo.Bedrijfs", "Plaats");
            DropColumn("dbo.Bedrijfs", "Postcode");
            DropColumn("dbo.Bedrijfs", "Adres");
            DropTable("dbo.Winkelwagens");
            AddPrimaryKey("dbo.Bedrijfs", "Id");
            CreateIndex("dbo.Gebruikers", "BedrijfId");
            CreateIndex("dbo.Bestellings", "Bedrijf_Id");
            AddForeignKey("dbo.Gebruikers", "BedrijfId", "dbo.Bedrijfs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs", "Id");
        }
    }
}
