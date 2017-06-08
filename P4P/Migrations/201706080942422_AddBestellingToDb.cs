namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBestellingToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bestellings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Prijs = c.Double(nullable: false),
                        Afleverdatum = c.DateTime(nullable: false),
                        Bedrijf_Id = c.Int(nullable: false),
                        Gebruiker_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bedrijfs", t => t.Bedrijf_Id, cascadeDelete: true)
                .ForeignKey("dbo.Gebruikers", t => t.Gebruiker_Id, cascadeDelete: true)
                .Index(t => t.Bedrijf_Id)
                .Index(t => t.Gebruiker_Id);
            
            AlterColumn("dbo.Bedrijfs", "Naam", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bestellings", "Gebruiker_Id", "dbo.Gebruikers");
            DropForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs");
            DropIndex("dbo.Bestellings", new[] { "Gebruiker_Id" });
            DropIndex("dbo.Bestellings", new[] { "Bedrijf_Id" });
            AlterColumn("dbo.Bedrijfs", "Naam", c => c.String());
            DropTable("dbo.Bestellings");
        }
    }
}
