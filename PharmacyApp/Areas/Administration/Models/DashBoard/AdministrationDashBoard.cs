using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterSchoolApp.Areas.Administration.Models.DashBoard
{
    public class AdminDashboardViewModel
    {
        //public int ExpiredUsers { get; set; }
        //public int VerificationComplaints { get; set; }
        public int AuditLogs { get; set; }
    }

    public class SuperAdminDashboardViewModel
    {
        public int ExpiredUsers { get; set; }
        public int Institutions { get; set; }
        public int Administrators { get; set; }
    }
}