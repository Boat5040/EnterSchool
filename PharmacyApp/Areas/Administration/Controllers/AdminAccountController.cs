
using EnterSchoolApp;
using EnterSchoolApp.Constants;
using EnterSchoolApp.Controllers;
using EnterSchoolApp.DAL;
using EnterSchoolApp.Areas.Administration.DataTableCollections;
using EnterSchoolApp.Areas.Administration.Models.Admin;
using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EnterSchoolApp.Models;
using EnterSchoolApp.Utility;
using System.Net.Mail;
using System.Diagnostics;

namespace EnterSchoolApp.Areas.Administration.Controllers
{
    [Authorize(Roles = UserRoles.SuperAdministrator)]
    public class AdminAccountController : BaseController
    {

        public AdminAccountController() { }

        public AdminAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager, context: new ApplicationDbContext())
        {

        }

        public ActionResult IndexAdmin()
        {
            return View();
        }
        // GET: Administration/Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var user = new AdminCollection(DataContext, User.Identity.GetUserId<int>())
                .GetEntityData(startIndex: viewModel.iDisplayStart, pageSize: viewModel.iDisplayLength,
                sortedColumns: viewModel.GetSortedColumns(), totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount, searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<AdminAccountViewModel>(items: user, totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount, sEcho: viewModel.sEcho));

        }


        public async Task<ActionResult> Create()
        {
            ViewBag.Institutions = new SelectList(await Task.Run(() => DataContext.Institutions.ToList()), "InstitutionId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewAdminAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] uploadedFile = new byte[viewModel.File.InputStream.Length];
                viewModel.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                var user = new ApplicationUser
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    UserName = viewModel.UserName,
                    UserImage = uploadedFile,
                    Email = viewModel.Email,
                    InstitutionId = viewModel.InstitutionId.Value,
                    ForceChangePassword = true,
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now
                };

                string generatedPassword = new PasswordGenerator().GenerateSimple();
                var result = await UserManager.CreateAsync(user, generatedPassword);

                if (result.Succeeded)
                {
                    await SendPasswordMailAsync(user, generatedPassword);
                    result = null;
                    result = await UserManager.AddToRoleAsync(user.Id, UserRoles.Administrator);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "AdminAccount", new { area = "Administration" });
                    }
                    else
                        AddErrors(result);
                }
                else
                    AddErrors(result);

            }
            ViewBag.Institutions = new SelectList(DataContext.Institutions.ToList(), "InstitutionId", "Name", viewModel.InstitutionId.Value);
            return View(viewModel);

        }


        private Task SendPasswordMailAsync(ApplicationUser user, string generatedPassword)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var smtp = new SmtpClient())
                    {
                        var mail = new MailMessage();
                        mail.To.Add(user.Email);
                        mail.Subject = "Pharmacy App";
                        mail.IsBodyHtml = true;
                        mail.Body = $"Hello {user.FirstName + "" + user.LastName},<br /> Your password: <b>{generatedPassword}</b>.<br />Best regards.";
                        smtp.Send(mail);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    Trace.TraceError(ex.ToString());
                }
            });
        }
    }
}