namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPictureidToPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "ProfilePicId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "ProfilePicId");
        }
    }
}
