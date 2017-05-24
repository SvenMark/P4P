namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hoofdcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false, maxLength: 255),
                        Hoofdcategorie_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hoofdcategories", t => t.Hoofdcategorie_Id)
                .Index(t => t.Hoofdcategorie_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subcategories", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropIndex("dbo.Subcategories", new[] { "Hoofdcategorie_Id" });
            DropTable("dbo.Subcategories");
            DropTable("dbo.Hoofdcategories");
        }
    }
}
