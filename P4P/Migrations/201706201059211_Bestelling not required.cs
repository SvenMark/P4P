namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bestellingnotrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bestellings", "AfleverAdres", c => c.String(maxLength: 255));
            AlterColumn("dbo.Bestellings", "AfleverPostcode", c => c.String(maxLength: 255));
            AlterColumn("dbo.Bestellings", "AfleverPlaats", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bestellings", "AfleverPlaats", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Bestellings", "AfleverPostcode", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Bestellings", "AfleverAdres", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
