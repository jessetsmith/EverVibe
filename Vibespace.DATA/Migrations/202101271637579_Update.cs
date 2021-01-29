namespace Vibespace.DATA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentsAndReactions",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        VibeID = c.Int(nullable: false),
                        Username = c.String(),
                        CommentText = c.String(),
                        DateCreated = c.DateTimeOffset(nullable: false, precision: 7),
                        DateModified = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.ApplicationUser", t => t.Id)
                .ForeignKey("dbo.Vibe", t => t.VibeID, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.VibeID);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.Vibe",
                c => new
                    {
                        VibeID = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        Username = c.String(),
                        Title = c.String(),
                        Location = c.String(),
                        Image = c.Binary(),
                        Description = c.String(),
                        Private = c.Boolean(nullable: false),
                        DateCreated = c.DateTimeOffset(nullable: false, precision: 7),
                        DateModified = c.DateTimeOffset(nullable: false, precision: 7),
                        UserInfo_UserInfoID = c.Int(),
                    })
                .PrimaryKey(t => t.VibeID)
                .ForeignKey("dbo.ApplicationUser", t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.UserInfo_UserInfoID)
                .Index(t => t.Id)
                .Index(t => t.UserInfo_UserInfoID);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                        Vibe_VibeID = c.Int(),
                        UserInfo_UserInfoID = c.Int(),
                    })
                .PrimaryKey(t => t.TagID)
                .ForeignKey("dbo.Vibe", t => t.Vibe_VibeID)
                .ForeignKey("dbo.UserInfo", t => t.UserInfo_UserInfoID)
                .Index(t => t.Vibe_VibeID)
                .Index(t => t.UserInfo_UserInfoID);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        UserInfoID = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        Name = c.String(),
                        Username = c.String(),
                        Bio = c.String(),
                        Location = c.String(),
                        ProfileImage = c.Binary(),
                        DateModified = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.UserInfoID)
                .ForeignKey("dbo.ApplicationUser", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vibe", "UserInfo_UserInfoID", "dbo.UserInfo");
            DropForeignKey("dbo.Tag", "UserInfo_UserInfoID", "dbo.UserInfo");
            DropForeignKey("dbo.UserInfo", "Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.CommentsAndReactions", "VibeID", "dbo.Vibe");
            DropForeignKey("dbo.Vibe", "Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.Tag", "Vibe_VibeID", "dbo.Vibe");
            DropForeignKey("dbo.CommentsAndReactions", "Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserLogin", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserClaim", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropIndex("dbo.UserInfo", new[] { "Id" });
            DropIndex("dbo.Tag", new[] { "UserInfo_UserInfoID" });
            DropIndex("dbo.Tag", new[] { "Vibe_VibeID" });
            DropIndex("dbo.Vibe", new[] { "UserInfo_UserInfoID" });
            DropIndex("dbo.Vibe", new[] { "Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CommentsAndReactions", new[] { "VibeID" });
            DropIndex("dbo.CommentsAndReactions", new[] { "Id" });
            DropTable("dbo.UserInfo");
            DropTable("dbo.IdentityRole");
            DropTable("dbo.Tag");
            DropTable("dbo.Vibe");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.CommentsAndReactions");
        }
    }
}
