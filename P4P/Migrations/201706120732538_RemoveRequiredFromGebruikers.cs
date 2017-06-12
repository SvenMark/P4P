namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromGebruikers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Gebruikers", "Wachtwoord", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Gebruikers", "Wachtwoord", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
