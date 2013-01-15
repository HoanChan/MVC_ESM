using Model;
using Mvc_ESM.Static_Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Mvc_ESM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SelectIgnoreStudentsController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public String SelectSuccess(List<String> StudentID, List<String> SubjectID)
        {
            InputHelper.IgnoreStudents = new Dictionary<String,List<String>>();
            string paramInfo = "";
            if (StudentID != null)
            {
                for (int i = 0; i < StudentID.Count(); i++)
                {
                    if (InputHelper.IgnoreStudents.ContainsKey(SubjectID[i]))
                    {
                        InputHelper.IgnoreStudents[SubjectID[i]].Add(StudentID[i]);
                    }
                    else
                    {
                        InputHelper.IgnoreStudents.Add(SubjectID[i], new List<String> { StudentID[i] });
                    }
                    paramInfo += "MH:" + SubjectID[i] + " SV: " + StudentID[i] + "<br /><br />";
                }
            }
            OutputHelper.SaveOBJ("IgnoreStudents", InputHelper.IgnoreStudents);
            return paramInfo;
        }

    }
}
