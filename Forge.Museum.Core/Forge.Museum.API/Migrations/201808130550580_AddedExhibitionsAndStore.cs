namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExhibitionsAndStore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exhibitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Description = c.String(maxLength: 4000),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        Organiser = c.String(maxLength: 256),
                        Price_Adult = c.Double(nullable: false),
                        Price_Concession = c.Double(nullable: false),
                        Price_Child = c.Double(nullable: false),
                        Image = c.Binary(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreItemImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.Binary(),
                        FileType = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        StoreItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreItems", t => t.StoreItem_Id, cascadeDelete: false)
                .Index(t => t.StoreItem_Id);
            
            CreateTable(
                "dbo.StoreItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Description = c.String(maxLength: 4000),
                        Cost = c.Double(nullable: false),
                        InStock = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreItemImages", "StoreItem_Id", "dbo.StoreItems");
            DropIndex("dbo.StoreItemImages", new[] { "StoreItem_Id" });
            DropTable("dbo.StoreItems");
            DropTable("dbo.StoreItemImages");
            DropTable("dbo.Exhibitions");
        }
    }
}
