using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EnterSchoolApp.Areas.Administration.Models.Institution;
using EnterSchoolApp.Constants;
using EnterSchoolApp.Controllers;
using EnterSchoolApp.DAL;
using EnterSchoolApp.Models;
using EnterSchoolAppAreas.Administration.DataTableCollections;
using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;

namespace EnterSchoolApp.Areas.Administration.Controllers
{
    [Authorize(Roles = UserRoles.SuperAdministrator)]
    public class InstitutionController : BaseController
    {
        public InstitutionController(): base(context: new ApplicationDbContext())
        {

        }
        // GET: Administration/Institution

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var user = new InstitutionCollection(DataContext)
                .GetEntityData(startIndex: viewModel.iDisplayStart, pageSize: viewModel.iDisplayLength,
                sortedColumns: viewModel.GetSortedColumns(), totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount, searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<InstitutionViewModel>(items: user, totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount, sEcho: viewModel.sEcho));

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewInstitutionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] uploadedFile = new byte[viewModel.File.InputStream.Length];
                viewModel.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                var institution = new Institution
                {
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now,
                    Email = viewModel.Email,
                    Name = viewModel.Name,
                    Subdomain = viewModel.Subdomain + ".EnterSchool.com",
                    Phone = viewModel.Phone,
                    IntitutionLogo = uploadedFile


                };
                DataContext.Institutions.Add(institution);
                await DataContext.SaveChangesAsync();
                return RedirectToAction("Index","Institution",new { area="Administration"});
            }

            return View(viewModel);

        }

        //Institution Check
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InstitutionNameExists(string name)
        {
            return Json(await Task.Run(() => DataContext.Institutions.FirstOrDefault(i => i.Name.ToLower().Equals(name.ToLower()))) == null);
        }

        //Subdomain check

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubdomainNameExists(string name)
        {
            return Json(await Task.Run(() => DataContext.Institutions.FirstOrDefault(i => i.Subdomain.ToLower().Equals(name.ToLower()))) == null);
        }
    }
}