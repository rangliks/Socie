namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedAutoIncrementId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Notification");
            AlterColumn("dbo.Notification", "NotificationId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Notification", "NotificationId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Notification");
            AlterColumn("dbo.Notification", "NotificationId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Notification", "NotificationId");
        }
    }
}
