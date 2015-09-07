using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MyFollow.DAL;

namespace MyFollow.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MyFollow.DAL.MyFollowContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

       protected override void Seed(MyFollow.DAL.MyFollowContext context)
       {
           var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
           var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
           const string name = "admin@gmail.com";
           const string password = "Admin@123";
           const string roleName = "Admin";

           //Create Role Admin if it does not exist
           var role = roleManager.FindByName(roleName);
           if (role == null)
           {
               roleManager.CreateAsync(new IdentityRole("Admin"));
               roleManager.CreateAsync(new IdentityRole("ProductOwner"));
               roleManager.CreateAsync(new IdentityRole("User"));
           }

           var user = userManager.FindByName(name);
           if (user == null)
           {
               user = new ApplicationUser { UserName = name, Email = name };
               userManager.Create(user, password);
               userManager.AddToRole(user.Id, roleName);
               userManager.SetLockoutEnabled(user.Id, false);
           }
           base.Seed(context);
        }

    }
}

