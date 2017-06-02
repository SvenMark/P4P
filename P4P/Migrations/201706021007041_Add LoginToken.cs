namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoginToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gebruikers", "LoginToken", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gebruikers", "LoginToken");
        }
    }
}
