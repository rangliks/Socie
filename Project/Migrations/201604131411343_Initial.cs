namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmotionScores",
                c => new
                    {
                        EmotionScoresId = c.String(nullable: false, maxLength: 128),
                        anger = c.Double(nullable: false),
                        contempt = c.Double(nullable: false),
                        disgust = c.Double(nullable: false),
                        fear = c.Double(nullable: false),
                        happiness = c.Double(nullable: false),
                        neutral = c.Double(nullable: false),
                        sadness = c.Double(nullable: false),
                        surprise = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.EmotionScoresId);
            
            //CreateTable(
            //    "dbo.Person",
            //    c => new
            //        {
            //            PersonId = c.String(nullable: false, maxLength: 128),
            //            SocieId = c.String(),
            //            Token = c.String(),
            //            Name = c.String(),
            //            Relation = c.String(),
            //        })
            //    .PrimaryKey(t => t.PersonId);
            
            //CreateTable(
            //    "dbo.PhotoAlbum",
            //    c => new
            //        {
            //            AlbumId = c.String(nullable: false, maxLength: 128),
            //            PersonId = c.String(),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.AlbumId);
            
            //CreateTable(
            //    "dbo.Photos",
            //    c => new
            //        {
            //            PhotoId = c.String(nullable: false, maxLength: 128),
            //            CreationDate = c.DateTime(nullable: false),
            //            Name = c.String(),
            //            Album_AlbumId = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.PhotoId)
            //    .ForeignKey("dbo.PhotoAlbum", t => t.Album_AlbumId)
            //    .Index(t => t.Album_AlbumId);
            
            //CreateTable(
            //    "dbo.Tag",
            //    c => new
            //        {
            //            TagId = c.String(nullable: false, maxLength: 128),
            //            CreationDate = c.DateTime(nullable: false),
            //            X = c.Single(nullable: false),
            //            Y = c.Single(nullable: false),
            //            PersonTagged_PersonId = c.String(maxLength: 128),
            //            Photo_PhotoId = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.TagId)
            //    .ForeignKey("dbo.Person", t => t.PersonTagged_PersonId)
            //    .ForeignKey("dbo.Photos", t => t.Photo_PhotoId)
            //    .Index(t => t.PersonTagged_PersonId)
            //    .Index(t => t.Photo_PhotoId);
            
            //CreateTable(
            //    "dbo.AspNetRoles",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            //CreateTable(
            //    "dbo.AspNetUserRoles",
            //    c => new
            //        {
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            RoleId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.UserId, t.RoleId })
            //    .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId)
            //    .Index(t => t.RoleId);
            
            //CreateTable(
            //    "dbo.AspNetUsers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Email = c.String(maxLength: 256),
            //            EmailConfirmed = c.Boolean(nullable: false),
            //            PasswordHash = c.String(),
            //            SecurityStamp = c.String(),
            //            PhoneNumber = c.String(),
            //            PhoneNumberConfirmed = c.Boolean(nullable: false),
            //            TwoFactorEnabled = c.Boolean(nullable: false),
            //            LockoutEndDateUtc = c.DateTime(),
            //            LockoutEnabled = c.Boolean(nullable: false),
            //            AccessFailedCount = c.Int(nullable: false),
            //            UserName = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            //CreateTable(
            //    "dbo.AspNetUserClaims",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            ClaimType = c.String(),
            //            ClaimValue = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.AspNetUserLogins",
            //    c => new
            //        {
            //            LoginProvider = c.String(nullable: false, maxLength: 128),
            //            ProviderKey = c.String(nullable: false, maxLength: 128),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Tag", "Photo_PhotoId", "dbo.Photos");
            DropForeignKey("dbo.Tag", "PersonTagged_PersonId", "dbo.Person");
            DropForeignKey("dbo.Photos", "Album_AlbumId", "dbo.PhotoAlbum");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Tag", new[] { "Photo_PhotoId" });
            DropIndex("dbo.Tag", new[] { "PersonTagged_PersonId" });
            DropIndex("dbo.Photos", new[] { "Album_AlbumId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tag");
            DropTable("dbo.Photos");
            DropTable("dbo.PhotoAlbum");
            DropTable("dbo.Person");
            DropTable("dbo.EmotionScores");
        }
    }
}
