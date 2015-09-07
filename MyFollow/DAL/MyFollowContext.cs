using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyFollow.Migrations;
using MyFollow.Models;
using System.Threading.Tasks;

namespace MyFollow.DAL
{
    //public class MyFollowUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<MyFollowUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}
    public class ApplicationUserLogin : IdentityUserLogin<int> { }
    public class ApplicationUserClaim : IdentityUserClaim<int> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }

    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
    {
        public string Description { get; set; }

        public ApplicationRole()  { }
        public ApplicationRole(string name) : this()
        {
            Name = name;
        }

        public ApplicationRole(string name, string description): this(name)
        {
            Description = description;
        }
    }


    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        [Required]
        public string Name { get; set; }

        public string CompanyName { get; set; }

        public async Task<ClaimsIdentity>  GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class MyFollowContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public MyFollowContext(): base("MyFollowContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyFollowContext, Configuration>("MyFollowContext"));
            Database.SetInitializer(new CreateDatabaseIfNotExists<MyFollowContext>());
        }

        public DbSet<ProductOwner> ProductOwners { get; set; }

        public DbSet<Products> Productses { get; set; }

        public DbSet<UploadImages> UploadImageses { get; set; }

        public DbSet<MainProduct> MainProducts { get; set; }

        public DbSet<FollowProducts> FollowProducts { get; set; }

        public static MyFollowContext Create()
        {
            return new MyFollowContext();
        }

    }
    
    public class ApplicationUserStore :UserStore<ApplicationUser, ApplicationRole, int,ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore(): this(new IdentityDbContext())
        {
            DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {

        }
    }

    public class ApplicationRoleStore: RoleStore<ApplicationRole, int, ApplicationUserRole>
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {

        }
    }
    
  
}