using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterSchoolApp.Areas.Administration.Models.Teacher
{
    public class TeacherViewModel
    {
        public int Totalteachers { get; set; }
        public int RecentlyHired { get; set; }

        public string Date { get; set; }
        public string TeacherName { get; set; }
        public string TeacherNo { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
    }

}