using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnterSchoolApp.Controllers;

namespace EnterSchoolApp.Areas.Administration.Controllers
{
    public class SecurityPolicyController : BaseController
    {
        // GET: SecurityPolicy
        public ActionResult Index()
        {
            return View();
        }
    }
}