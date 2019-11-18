using EnterSchoolApp;
using EnterSchoolApp.Controllers;
using EnterSchoolApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterSchoolyApp.Areas.Administration.Controllers
{
    public class TeacherController : BaseController
    {
        public TeacherController()
        {

        }

        public TeacherController(ApplicationUserManager userManager) : base(userManager, context: new ApplicationDbContext())
        {

        }
        // GET: Administration/Teacher
        public ActionResult Index()
        {
            return View();
        }
    }
}