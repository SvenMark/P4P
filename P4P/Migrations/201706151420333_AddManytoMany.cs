namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManytoMany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BestellingProducts", "Aantal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BestellingProducts", "Aantal");
        }
    }
}
