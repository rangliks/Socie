namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTagsListInPhoto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tag", "PhotoId", "dbo.Photos");
            DropIndex("dbo.Tag", new[] { "PhotoId" });
            AlterColumn("dbo.Tag", "PhotoId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tag", "PhotoId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tag", "PhotoId");
            AddForeignKey("dbo.Tag", "PhotoId", "dbo.Photos", "PhotoId");
        }
    }
}
