namespace Forge.Museum.API.Migrations
{
	using Forge.Museum.API.Models;
	using Microsoft.AspNet.Identity;
	using System;
	using System.Configuration;
	using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Forge.Museum.API.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Forge.Museum.API.Models.ApplicationDbContext context)
        {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.

			var sysAdminPassword = ConfigurationManager.AppSettings["System_Admin_Password"];

			context.AspNetUsers.AddOrUpdate(new AspNetUser
			{
				Id = "13e2f031-1293-4a53-a0ad-fb24f1776817",
				FirstName = "System",
				LastName = "Administrator",
				Email = "admin@redlandmuseum.com.au",
				EmailConfirmed = true,
				PasswordHash = new PasswordHasher().HashPassword(sysAdminPassword),
				UserName = "admin@redlandmuseum.com.au",
				SecurityStamp = Guid.NewGuid().ToString(),
				LockoutEndDateUtc = DateTime.Now,
				LockoutEnabled = false
			});
		}
    }
}
