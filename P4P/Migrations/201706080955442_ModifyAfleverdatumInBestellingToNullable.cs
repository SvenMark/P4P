namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyAfleverdatumInBestellingToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bestellings", "Afleverdatum", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bestellings", "Afleverdatum", c => c.DateTime(nullable: false));
        }
    }
}
