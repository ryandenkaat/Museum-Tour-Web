namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUniqueArtefactCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artefacts", "UniqueCode", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artefacts", "UniqueCode");
        }
    }
}
