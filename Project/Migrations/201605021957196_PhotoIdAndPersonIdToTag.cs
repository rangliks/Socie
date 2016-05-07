namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoIdAndPersonIdToTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tag", "PersonTagged_PersonId", "dbo.Person");
            DropIndex("dbo.Tag", new[] { "PersonTagged_PersonId" });
            RenameColumn(table: "dbo.Tag", name: "Photo_PhotoId", newName: "PhotoId");
            RenameIndex(table: "dbo.Tag", name: "IX_Photo_PhotoId", newName: "IX_PhotoId");
            AddColumn("dbo.Tag", "PersonId", c => c.String());
            DropColumn("dbo.Tag", "PersonTagged_PersonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tag", "PersonTagged_PersonId", c => c.String(maxLength: 128));
            DropColumn("dbo.Tag", "PersonId");
            RenameIndex(table: "dbo.Tag", name: "IX_PhotoId", newName: "IX_Photo_PhotoId");
            RenameColumn(table: "dbo.Tag", name: "PhotoId", newName: "Photo_PhotoId");
            CreateIndex("dbo.Tag", "PersonTagged_PersonId");
            AddForeignKey("dbo.Tag", "PersonTagged_PersonId", "dbo.Person", "PersonId");
        }
    }
}
