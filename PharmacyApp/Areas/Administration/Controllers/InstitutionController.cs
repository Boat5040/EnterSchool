﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnterSchoolApp.Areas.Administration.Models.Institution;
using EnterSchoolApp.Constants;
using EnterSchoolApp.Controllers;
using EnterSchoolApp.DAL;
using EnterSchoolAppAreas.Administration.DataTableCollections;
using jQuery.DataTables.Mvc;

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
    }
}