namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBedrijfIdToGebruiker : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs");
            DropForeignKey("dbo.Gebruikers", "Bedrijf_Id", "dbo.Bedrijfs");
            DropIndex("dbo.Bestellings", new[] { "Bedrijf_Id" });
            DropIndex("dbo.Gebruikers", new[] { "Bedrijf_Id" });
            RenameColumn(table: "dbo.Gebruikers", name: "Bedrijf_Id", newName: "BedrijfId");
            DropPrimaryKey("dbo.Bedrijfs");
            AlterColumn("dbo.Bestellings", "Bedrijf_Id", c => c.Byte());
            AlterColumn("dbo.Bedrijfs", "Id", c => c.Byte(nullable: false));
            AlterColumn("dbo.Gebruikers", "BedrijfId", c => c.Byte(nullable: false));
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
            AlterColumn("dbo.Gebruikers", "BedrijfId", c => c.Int());
            AlterColumn("dbo.Bedrijfs", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Bestellings", "Bedrijf_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Bedrijfs", "Id");
            RenameColumn(table: "dbo.Gebruikers", name: "BedrijfId", newName: "Bedrijf_Id");
            CreateIndex("dbo.Gebruikers", "Bedrijf_Id");
            CreateIndex("dbo.Bestellings", "Bedrijf_Id");
            AddForeignKey("dbo.Gebruikers", "Bedrijf_Id", "dbo.Bedrijfs", "Id");
            AddForeignKey("dbo.Bestellings", "Bedrijf_Id", "dbo.Bedrijfs", "Id", cascadeDelete: true);
        }
    }
}
