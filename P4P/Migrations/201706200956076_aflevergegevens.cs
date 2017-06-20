namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aflevergegevens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bestellings", "AfleverAdres", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Bestellings", "AfleverPostcode", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Bestellings", "AfleverPlaats", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bestellings", "AfleverPlaats");
            DropColumn("dbo.Bestellings", "AfleverPostcode");
            DropColumn("dbo.Bestellings", "AfleverAdres");
        }
    }
}
