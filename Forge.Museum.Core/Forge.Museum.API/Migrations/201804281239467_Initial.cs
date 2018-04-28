namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArtefactCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Image = c.Binary(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Artefacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Image = c.Binary(),
                        AdditionalComments = c.String(maxLength: 4000),
                        AcquisitionDate = c.DateTime(nullable: false),
                        Measurement_Length = c.Int(nullable: false),
                        Measurement_Width = c.Int(nullable: false),
                        Measurement_Height = c.Int(nullable: false),
                        ArtefactStatus = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ArtefactCategory_Id = c.Int(),
                        Zone_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArtefactCategories", t => t.ArtefactCategory_Id)
                .ForeignKey("dbo.Zones", t => t.Zone_Id)
                .Index(t => t.ArtefactCategory_Id)
                .Index(t => t.Zone_Id);
            
            CreateTable(
                "dbo.ArtefactInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 4000),
                        File = c.Binary(),
                        ArtefactInfoType = c.Int(nullable: false),
                        Content = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Artefact_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artefacts", t => t.Artefact_Id, cascadeDelete: false)
                .Index(t => t.Artefact_Id);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false, maxLength: 512),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(nullable: false),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: false)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: false)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.AspNetUserAspNetRoles",
                c => new
                    {
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                        AspNetRole_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AspNetUser_Id, t.AspNetRole_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: false)
                .ForeignKey("dbo.AspNetRoles", t => t.AspNetRole_Id, cascadeDelete: false)
                .Index(t => t.AspNetUser_Id)
                .Index(t => t.AspNetRole_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserLogins", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserAspNetRoles", "AspNetRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserAspNetRoles", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Artefacts", "Zone_Id", "dbo.Zones");
            DropForeignKey("dbo.ArtefactInfoes", "Artefact_Id", "dbo.Artefacts");
            DropForeignKey("dbo.Artefacts", "ArtefactCategory_Id", "dbo.ArtefactCategories");
            DropIndex("dbo.AspNetUserAspNetRoles", new[] { "AspNetRole_Id" });
            DropIndex("dbo.AspNetUserAspNetRoles", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "AspNetUser_Id" });
            DropIndex("dbo.ArtefactInfoes", new[] { "Artefact_Id" });
            DropIndex("dbo.Artefacts", new[] { "Zone_Id" });
            DropIndex("dbo.Artefacts", new[] { "ArtefactCategory_Id" });
            DropTable("dbo.AspNetUserAspNetRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Zones");
            DropTable("dbo.ArtefactInfoes");
            DropTable("dbo.Artefacts");
            DropTable("dbo.ArtefactCategories");
        }
    }
}
