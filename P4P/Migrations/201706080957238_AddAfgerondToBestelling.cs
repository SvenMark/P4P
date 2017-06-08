namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAfgerondToBestelling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bestellings", "Afgerond", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bestellings", "Afgerond");
        }
    }
}
