namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdInScores : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.EmotionScores");
            AddColumn("dbo.EmotionScores", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.EmotionScores", "Id");
            DropColumn("dbo.EmotionScores", "EmotionScoresId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmotionScores", "EmotionScoresId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.EmotionScores");
            DropColumn("dbo.EmotionScores", "Id");
            AddPrimaryKey("dbo.EmotionScores", "EmotionScoresId");
        }
    }
}
