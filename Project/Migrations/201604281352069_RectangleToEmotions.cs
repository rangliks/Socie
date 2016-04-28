namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RectangleToEmotions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmotionScores", "left", c => c.Double(nullable: false));
            AddColumn("dbo.EmotionScores", "top", c => c.Double(nullable: false));
            AddColumn("dbo.EmotionScores", "width", c => c.Double(nullable: false));
            AddColumn("dbo.EmotionScores", "height", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmotionScores", "height");
            DropColumn("dbo.EmotionScores", "width");
            DropColumn("dbo.EmotionScores", "top");
            DropColumn("dbo.EmotionScores", "left");
        }
    }
}
