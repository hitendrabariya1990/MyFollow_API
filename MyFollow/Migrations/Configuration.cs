namespace MyFollow.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using MyFollow.DAL;
    using MyFollow.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MyFollow.DAL.MyFollowContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MyFollow.DAL.MyFollowContext context)
        {
           // var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
           // var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
           // const string name = "admin@gmail.com";
           // const string password = "Admin@123";
           // const string roleName = "Admin";

           // //Create Role Admin if it does not exist
           // var role = roleManager.FindByName(roleName);
           // if (role == null)
           // {
           //     role = new ApplicationRole(roleName);
           //     var roleresult = roleManager.Create(role);
           // }

           // var user = userManager.FindByName(name);
           // if (user == null)
           // {
           //     user = new ApplicationUser { UserName = name, Email = name };
           //     var result = userManager.Create(user, password);
           //     result = userManager.SetLockoutEnabled(user.Id, false);
           // }

           // // Add user admin to Role Admin if not already added
           // var rolesForUser = userManager.GetRoles(user.Id);
           // if (!rolesForUser.Contains(role.Name))
           // {
           //     var result = userManager.AddToRole(user.Id, role.Name);
           // }
           //// InitializeIdentityForEF(context);
           // base.Seed(context);
        }

        }
    }

