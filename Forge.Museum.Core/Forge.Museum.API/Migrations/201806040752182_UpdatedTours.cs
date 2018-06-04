namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTours : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 512),
                        Description = c.String(maxLength: 4000),
                        AgeGroup = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TourArtefacts",
                c => new
                    {
                        Tour_Id = c.Int(nullable: false),
                        Artefact_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tour_Id, t.Artefact_Id })
                .ForeignKey("dbo.Tours", t => t.Tour_Id, cascadeDelete: true)
                .ForeignKey("dbo.Artefacts", t => t.Artefact_Id, cascadeDelete: true)
                .Index(t => t.Tour_Id)
                .Index(t => t.Artefact_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TourArtefacts", "Artefact_Id", "dbo.Artefacts");
            DropForeignKey("dbo.TourArtefacts", "Tour_Id", "dbo.Tours");
            DropIndex("dbo.TourArtefacts", new[] { "Artefact_Id" });
            DropIndex("dbo.TourArtefacts", new[] { "Tour_Id" });
            DropTable("dbo.TourArtefacts");
            DropTable("dbo.Tours");
        }
    }
}
