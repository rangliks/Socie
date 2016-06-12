namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCreationDateToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "CreationDate");
        }
    }
}
