
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

    }
}