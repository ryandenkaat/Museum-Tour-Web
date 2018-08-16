using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forge.Museum.API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>(true));
        }

        #region Tables
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<Artefact> Artefacts { get; set; }
        public virtual DbSet<ArtefactInfo> ArtefactInfos { get; set; }
        public virtual DbSet<ArtefactCategory> ArtefactCategories { get; set; }
		public virtual DbSet<Exhibition> Exhibitions { get; set; }
		public virtual DbSet<StoreItem> StoreItems { get; set; }
		public virtual DbSet<StoreItemImage> StoreItemImages { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<Tour> Tours { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tour>()
                .HasMany(m => m.Artefacts).WithMany(m => m.Tours);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        #endregion
    }

    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}