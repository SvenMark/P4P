namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBedrijfToGebruikers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gebruikers", "Bedrijf_Id", c => c.Int());
            CreateIndex("dbo.Gebruikers", "Bedrijf_Id");
            AddForeignKey("dbo.Gebruikers", "Bedrijf_Id", "dbo.Bedrijfs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gebruikers", "Bedrijf_Id", "dbo.Bedrijfs");
            DropIndex("dbo.Gebruikers", new[] { "Bedrijf_Id" });
            DropColumn("dbo.Gebruikers", "Bedrijf_Id");
        }
    }
}
