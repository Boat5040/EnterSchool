using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnterSchoolApp.Constants;

namespace EnterSchoolApp.Areas.Administration.Controllers
{
    public class InstitutionController : Controller
    {
        // GET: Administration/Institution

        public ActionResult Index()
        {
            return View();
        }
    }
}