namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_meldingen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meldings", "Naam", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Meldings", "Naam");
        }
    }
}
