namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBounds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artefacts", "XBound", c => c.Double(nullable: false));
            AddColumn("dbo.Artefacts", "YBound", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artefacts", "YBound");
            DropColumn("dbo.Artefacts", "XBound");
        }
    }
}
