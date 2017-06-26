namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromSubcategorie : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subcategories", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropIndex("dbo.Subcategories", new[] { "Hoofdcategorie_Id" });
            AlterColumn("dbo.Subcategories", "Hoofdcategorie_Id", c => c.Int());
            CreateIndex("dbo.Subcategories", "Hoofdcategorie_Id");
            AddForeignKey("dbo.Subcategories", "Hoofdcategorie_Id", "dbo.Hoofdcategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subcategories", "Hoofdcategorie_Id", "dbo.Hoofdcategories");
            DropIndex("dbo.Subcategories", new[] { "Hoofdcategorie_Id" });
            AlterColumn("dbo.Subcategories", "Hoofdcategorie_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Subcategories", "Hoofdcategorie_Id");
            AddForeignKey("dbo.Subcategories", "Hoofdcategorie_Id", "dbo.Hoofdcategories", "Id", cascadeDelete: true);
        }
    }
}
