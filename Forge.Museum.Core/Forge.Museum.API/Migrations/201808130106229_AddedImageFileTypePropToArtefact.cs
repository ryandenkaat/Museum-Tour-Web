namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageFileTypePropToArtefact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artefacts", "ImageFileType", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artefacts", "ImageFileType");
        }
    }
}
