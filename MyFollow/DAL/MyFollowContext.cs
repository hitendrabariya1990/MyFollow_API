using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
//using MyFollow.Migrations;
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

    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>, IRole<int>
    {
        public string Description { get; set; }

        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : this()
        {
            this.Name = name;
        }

        public ApplicationRole(string name, string description)
            : this(name)
        {
            this.Description = description;
        }
    }


    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<int>
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
        public MyFollowContext()
            : base("MyFollowContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyFollowContext, MyFollow.Migrations.Configuration>("MyFollowContext"));
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyFollowContext,Configuration>("MyFollowContext"));
            Database.SetInitializer<MyFollowContext>(new CreateDatabaseIfNotExists<MyFollowContext>());
           // Database.SetInitializer<MyFollowContext>(new ApplicationDbInitializer());
        }

       
        static MyFollowContext()
        {
            //Database.SetInitializer<MyFollowContext>(new DropCreateDatabaseIfModelChanges<MyFollowContext>());
            Database.SetInitializer<MyFollowContext>(new ApplicationDbInitializer());
        }

       

        public DbSet<ProductOwner> ProductOwners { get; set; }

        public DbSet<Products> Productses { get; set; }

        public DbSet<UploadImages> UploadImageses { get; set; }


        public DbSet<FollowProducts> FollowProducts { get; set; }
      //  public DbSet<T> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static MyFollowContext Create()
        {
            return new MyFollowContext();
        }

       // public System.Data.Entity.DbSet<MyFollow.DAL.ApplicationUser> ApplicationUsers { get; set; }

    }
    public class ApplicationUserStore :UserStore<ApplicationUser, ApplicationRole, int,ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUserStore<ApplicationUser, int>, IDisposable
    {
        public ApplicationUserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }


    public class ApplicationRoleStore: RoleStore<ApplicationRole, int, ApplicationUserRole>, IQueryableRoleStore<ApplicationRole, int>, IRoleStore<ApplicationRole, int>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }
    
  
}