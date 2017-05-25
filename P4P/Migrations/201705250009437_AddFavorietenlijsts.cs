namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFavorietenlijsts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Favorietenlijsts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(),
                        Gebruiker_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gebruikers", t => t.Gebruiker_Id)
                .Index(t => t.Gebruiker_Id);
            
            AddColumn("dbo.Products", "Favorietenlijst_Id", c => c.Int());
            CreateIndex("dbo.Products", "Favorietenlijst_Id");
            AddForeignKey("dbo.Products", "Favorietenlijst_Id", "dbo.Favorietenlijsts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Favorietenlijst_Id", "dbo.Favorietenlijsts");
            DropForeignKey("dbo.Favorietenlijsts", "Gebruiker_Id", "dbo.Gebruikers");
            DropIndex("dbo.Products", new[] { "Favorietenlijst_Id" });
            DropIndex("dbo.Favorietenlijsts", new[] { "Gebruiker_Id" });
            DropColumn("dbo.Products", "Favorietenlijst_Id");
            DropTable("dbo.Favorietenlijsts");
        }
    }
}
