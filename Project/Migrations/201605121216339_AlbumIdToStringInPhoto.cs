namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlbumIdToStringInPhoto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "Album_AlbumId", "dbo.PhotoAlbum");
            DropIndex("dbo.Photos", new[] { "Album_AlbumId" });
            AddColumn("dbo.Photos", "AlbumId", c => c.String());
            DropColumn("dbo.Photos", "Album_AlbumId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "Album_AlbumId", c => c.String(maxLength: 128));
            DropColumn("dbo.Photos", "AlbumId");
            CreateIndex("dbo.Photos", "Album_AlbumId");
            AddForeignKey("dbo.Photos", "Album_AlbumId", "dbo.PhotoAlbum", "AlbumId");
        }
    }
}
