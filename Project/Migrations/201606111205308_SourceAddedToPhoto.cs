namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SourceAddedToPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "Source");
        }
    }
}
