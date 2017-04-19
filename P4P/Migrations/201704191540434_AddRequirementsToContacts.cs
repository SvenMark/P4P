namespace P4P.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequirementsToContacts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "LastName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Contacts", "Message", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Contacts", "Email", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Contacts", "PhoneNumber", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Contacts", "Email", c => c.String());
            AlterColumn("dbo.Contacts", "Message", c => c.String());
            AlterColumn("dbo.Contacts", "LastName", c => c.String());
        }
    }
}
