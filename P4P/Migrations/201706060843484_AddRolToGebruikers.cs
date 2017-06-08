namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRolToGebruikers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gebruikers", "Rol", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gebruikers", "Rol");
        }
    }
}
