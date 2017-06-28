namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AanbiedingPrijs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Aanbiedingprijs", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Aanbiedingprijs");
        }
    }
}
