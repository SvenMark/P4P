namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBedrijfToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bedrijfs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bedrijfs");
        }
    }
}
