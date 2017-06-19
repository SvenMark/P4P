namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dank : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs");
            DropForeignKey("dbo.Gebruikers", "BedrijfId", "dbo.Bedrijfs");
            DropIndex("dbo.Bestellings", new[] { "Bedrijf_Id" });
            DropIndex("dbo.Gebruikers", new[] { "BedrijfId" });
            DropPrimaryKey("dbo.Bedrijfs");
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
            DropIndex("dbo.Gebruikers", new[] { "BedrijfId" });
            DropIndex("dbo.Bestellings", new[] { "Bedrijf_Id" });
            DropPrimaryKey("dbo.Bedrijfs");
            AlterColumn("dbo.Gebruikers", "BedrijfId", c => c.Byte(nullable: false));
            AlterColumn("dbo.Bedrijfs", "Id", c => c.Byte(nullable: false));
            AlterColumn("dbo.Bestellings", "Bedrijf_Id", c => c.Byte());
            AddPrimaryKey("dbo.Bedrijfs", "Id");
            CreateIndex("dbo.Gebruikers", "BedrijfId");
            CreateIndex("dbo.Bestellings", "Bedrijf_Id");
            AddForeignKey("dbo.Gebruikers", "BedrijfId", "dbo.Bedrijfs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs", "Id");
        }
    }
}
