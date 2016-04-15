namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScoreIdToInt : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.EmotionScores");
            AlterColumn("dbo.EmotionScores", "EmotionScoresId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.EmotionScores", "EmotionScoresId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.EmotionScores");
            AlterColumn("dbo.EmotionScores", "EmotionScoresId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.EmotionScores", "EmotionScoresId");
        }
    }
}
