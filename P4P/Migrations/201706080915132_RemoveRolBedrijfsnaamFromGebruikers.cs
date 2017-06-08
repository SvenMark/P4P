namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRolBedrijfsnaamFromGebruikers : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Gebruikers", "Bedrijfsnaam");
            DropColumn("dbo.Gebruikers", "LoginToken");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gebruikers", "LoginToken", c => c.String(maxLength: 255));
            AddColumn("dbo.Gebruikers", "Bedrijfsnaam", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
