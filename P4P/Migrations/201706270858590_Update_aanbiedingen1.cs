namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_aanbiedingen1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Aanbiedingen", c => c.Boolean(nullable: false));
            DropColumn("dbo.Products", "Beschikbaar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Beschikbaar", c => c.Boolean(nullable: false));
            DropColumn("dbo.Products", "Aanbiedingen");
        }
    }
}
