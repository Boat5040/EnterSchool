using EnterSchoolApp;
using EnterSchoolApp.Areas.Administration.DataTableCollections;
using EnterSchoolApp.Areas.Administration.Models.Branch;
using EnterSchoolApp.Constants;
using EnterSchoolApp.Controllers;
using EnterSchoolApp.DAL;
using EnterSchoolApp.Models;
using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EnterSchoolyApp.Areas.Administration.Controllers
{
    [Authorize(Roles = UserRoles.Administrator)]
    public class BranchController : BaseController
    {
        public BranchController()
        {

        }

        public BranchController(ApplicationUserManager userManager) : base(userManager, context: new ApplicationDbContext())
        {

        }
        // GET: Administration/Branch
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var branches = new BranchCollection(DataContext, CurrentUser.InstitutionId.Value)
                .GetEntityData(startIndex: viewModel.iDisplayStart,
                pageSize: viewModel.iDisplayLength, sortedColumns: viewModel.GetSortedColumns(),
                totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount,
                searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<BranchViewModel>(items: branches,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: viewModel.sEcho));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewBranchViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var branch = new Branch
                {
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now,
                    Name = viewModel.Name,
                    Phone = viewModel.Phone,
                    InstitutionId = CurrentUser.InstitutionId.Value
                };

                DataContext.Branches.Add(branch);
                await DataContext.SaveChangesAsync();
                return RedirectToAction("Index","Branch",new { area="Administration"});
            }

            return View(viewModel);
        }
    }
}