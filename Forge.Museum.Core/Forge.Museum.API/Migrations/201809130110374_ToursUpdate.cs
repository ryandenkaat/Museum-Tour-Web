namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToursUpdate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TourArtefacts");
            AddColumn("dbo.TourArtefacts", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.TourArtefacts", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.TourArtefacts", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TourArtefacts", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TourArtefacts", "IsDeleted", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.TourArtefacts", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TourArtefacts");
            DropColumn("dbo.TourArtefacts", "IsDeleted");
            DropColumn("dbo.TourArtefacts", "ModifiedDate");
            DropColumn("dbo.TourArtefacts", "CreatedDate");
            DropColumn("dbo.TourArtefacts", "Order");
            DropColumn("dbo.TourArtefacts", "Id");
            AddPrimaryKey("dbo.TourArtefacts", new[] { "Tour_Id", "Artefact_Id" });
        }
    }
}
