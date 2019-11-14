using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterSchoolApp.Areas.Administration.Models.SuperAdmin
{
    public class SuperAdminDashboardViewModel
    {
        public int ExpiredUsers { get; set; }
        public int Institutions { get; set; }
        public int Administrators { get; set; }
    

    }
}