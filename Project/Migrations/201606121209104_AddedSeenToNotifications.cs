namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSeenToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Seen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "Seen");
        }
    }
}
