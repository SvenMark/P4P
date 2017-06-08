namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOpmerkingToBestelling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bestellings", "Opmerking", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bestellings", "Opmerking");
        }
    }
}
