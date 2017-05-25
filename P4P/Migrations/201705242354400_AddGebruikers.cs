namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGebruikers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gebruikers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Voornaam = c.String(nullable: false, maxLength: 255),
                        Tussenvoegsel = c.String(nullable: false, maxLength: 255),
                        Achternaam = c.String(nullable: false, maxLength: 255),
                        Telefoonnummer = c.String(nullable: false, maxLength: 255),
                        Emailadres = c.String(nullable: false, maxLength: 255),
                        Bedrijfsnaam = c.String(nullable: false, maxLength: 255),
                        Adres = c.String(nullable: false, maxLength: 255),
                        Postcode = c.String(nullable: false, maxLength: 255),
                        Woonplaats = c.String(nullable: false, maxLength: 255),
                        Wachtwoord = c.String(maxLength: 255),
                        Token = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Gebruikers");
        }
    }
}
