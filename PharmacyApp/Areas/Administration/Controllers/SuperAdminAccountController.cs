using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using EnterSchoolApp.Areas.Administration.Models.SuperAdmin;
using EnterSchoolApp.Constants;
using EnterSchoolApp.Controllers;
using EnterSchoolApp.Helper;
using EnterSchoolApp.Models;
using EnterSchoolApp.Utility;
using EnterSchoolAppAreas.Administration.DataTableCollections;
using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;

namespace EnterSchoolApp.Areas.Administration.Controllers
{
    [Authorize(Roles = UserRoles.SuperAdministrator)]

    public class SuperAdminAccountController : BaseController
    {

        public SuperAdminAccountController() { }

        public SuperAdminAccountController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
            : base(userManager, roleManager: roleManager)
        {

        }
        private SecurityPolicy GetPolicy()
        {
            return DataContext.Institutions.Find(CurrentUser.InstitutionId)
                ?.SecurityPolicy.ToObject<SecurityPolicy>() ?? new SecurityPolicy
                {
                    MaximumPasswordLength = 12,
                    MinimumPasswordLength = 6,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireSpecialCharacter = true,
                    RequireUppercase = true,
                    SpecialCharacters = "@$%&?*"
                };
        }


        // GET: Administration/SuperAdminAccount
        public ActionResult Index() => View();

        [HttpPost]
        //[Route("super-users")]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var user = new SuperAdminCollection(DataContext, User.Identity.GetUserId<int>())
                .GetEntityData(startIndex: viewModel.iDisplayStart, pageSize: viewModel.iDisplayLength,
                sortedColumns: viewModel.GetSortedColumns(), totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount, searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<SuperAdminViewModel>(items: user, totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount, sEcho: viewModel.sEcho));

        }


        public ActionResult Create()
        {
            ViewBag.Info = SecurityPolicyFactory.GenerateValidationMessage(GetPolicy());
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewSuperAdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] uploadedFile = new byte[viewModel.UserImage.InputStream.Length];
                viewModel.UserImage.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                var user = new ApplicationUser
                {
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    UserName = viewModel.UserName,
                    UserImage = uploadedFile,
                };

                string generatedPassword = new PasswordGenerator().GenerateSimple();
                var result = await UserManager.CreateAsync(user, generatedPassword);

                if (result.Succeeded)
                {
                    result = null;
                    result = await UserManager.AddToRoleAsync(user.Id,UserRoles.SuperAdministrator);

                    if (result.Succeeded)
                        return RedirectToAction("Index","SuperAdminAccount", new { area="Administration"});
                    else
                        AddErrors(result);
                }
                else
                    AddErrors(result);

            }
            ViewBag.Info = SecurityPolicyFactory.GenerateValidationMessage(GetPolicy());
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserNameExists(string userName)
        {
            return Json(await UserManager.FindByNameAsync(userName) == null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidatePassword(string password)
        {
            return Json(SecurityPolicyFactory.EnforcePolicy(password, GetPolicy()), JsonRequestBehavior.AllowGet);
        }


    }
}