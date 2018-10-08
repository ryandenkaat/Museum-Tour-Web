namespace Forge.Museum.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRolesTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetUserAspNetRoles", newName: "AspNetUserRoles");
            RenameColumn(table: "dbo.AspNetUserRoles", name: "AspNetUser_Id", newName: "UserId");
            RenameColumn(table: "dbo.AspNetUserRoles", name: "AspNetRole_Id", newName: "RoleId");
            RenameIndex(table: "dbo.AspNetUserRoles", name: "IX_AspNetUser_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.AspNetUserRoles", name: "IX_AspNetRole_Id", newName: "IX_RoleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUserRoles", name: "IX_RoleId", newName: "IX_AspNetRole_Id");
            RenameIndex(table: "dbo.AspNetUserRoles", name: "IX_UserId", newName: "IX_AspNetUser_Id");
            RenameColumn(table: "dbo.AspNetUserRoles", name: "RoleId", newName: "AspNetRole_Id");
            RenameColumn(table: "dbo.AspNetUserRoles", name: "UserId", newName: "AspNetUser_Id");
            RenameTable(name: "dbo.AspNetUserRoles", newName: "AspNetUserAspNetRoles");
        }
    }
}
