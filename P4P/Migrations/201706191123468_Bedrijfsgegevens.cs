namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bedrijfsgegevens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bedrijfs", "Adres", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Bedrijfs", "Postcode", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Bedrijfs", "Plaats", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Bedrijfs", "Telefoonnummer", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bedrijfs", "Telefoonnummer");
            DropColumn("dbo.Bedrijfs", "Plaats");
            DropColumn("dbo.Bedrijfs", "Postcode");
            DropColumn("dbo.Bedrijfs", "Adres");
        }
    }
}
