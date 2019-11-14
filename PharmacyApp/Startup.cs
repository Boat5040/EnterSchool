using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using EnterSchoolApp;
using EnterSchoolApp.Constants;
using EnterSchoolApp.Models;
using EnterSchoolApp.Services;
using System;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(EnterSchoolApp.Startup))]
namespace EnterSchoolApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureContainer(app);
            ConfigureAuth(app);
            CreateUserRoles();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

        }


        private void CreateUserRoles()
        {
            var roleManager = ApplicationRoleManager.Instance;

            if (!roleManager.RoleExists(UserRoles.SuperAdministrator))
                roleManager.Create(GetRole(UserRoles.SuperAdministrator, "Creates Pharmacies and other super administrators."));

            if (!roleManager.RoleExists(UserRoles.Administrator))
                roleManager.Create(GetRole(UserRoles.Administrator, "Creates branches and users."));

            if (!roleManager.RoleExists(UserRoles.Principle))
                roleManager.Create(GetRole(UserRoles.Principle, "Manage the shops"));

            if (!roleManager.RoleExists(UserRoles.Teacher))
                roleManager.Create(GetRole(UserRoles.Teacher, "Manage the shops"));

            if (!roleManager.RoleExists(UserRoles.Parent))
                roleManager.Create(GetRole(UserRoles.Parent, "Manage the shops"));


            if (!roleManager.RoleExists(UserRoles.Student))
                roleManager.Create(GetRole(UserRoles.Student, "Manage the shops"));


            //if (!roleManager.RoleExists(PharmacyUserRoles.User))
            //    roleManager.Create(GetRole(PharmacyUserRoles.User, "Manage the shops"));

        }

        private void LogError(IdentityResult result)
        {
            if (!result.Succeeded)
                DependencyResolver.Current.GetService<ILoggingService>().Log(new Exception(string.Join("\r\n", result.Errors)));
        }


        private ApplicationRole GetRole(string name, string description) => new ApplicationRole { Name = name, Description = description };

    }


}
