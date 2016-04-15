namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoIdInEmotionsScores : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmotionScores", "PhotoId_PhotoId", c => c.String(maxLength: 128));
            CreateIndex("dbo.EmotionScores", "PhotoId_PhotoId");
            AddForeignKey("dbo.EmotionScores", "PhotoId_PhotoId", "dbo.Photos", "PhotoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmotionScores", "PhotoId_PhotoId", "dbo.Photos");
            DropIndex("dbo.EmotionScores", new[] { "PhotoId_PhotoId" });
            DropColumn("dbo.EmotionScores", "PhotoId_PhotoId");
        }
    }
}
