namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFileExtensionsToArtefactInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArtefactInfoes", "FileExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArtefactInfoes", "FileExtension");
        }
    }
}
