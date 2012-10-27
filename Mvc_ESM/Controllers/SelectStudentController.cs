using Mvc_ESM.Models;
using Mvc_ESM.Static_Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Mvc_ESM.Controllers
{
    public class SelectStudentController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /SelectSubject/
        [HttpGet]
        public ViewResult Index()
        {
            //var Students = (from d in db.pdkmhs
            //               join s in db.sinhviens on d.MaSinhVien equals s.MaSinhVien
            //               select s).Distinct();
            //return View(Students.ToList());
            return View();
        }

        [HttpPost]
        public String SelectSuccess(List<String> StudentID, List<String> SubjectID)
        {
            InputHelper.Students = new Hashtable();
            string paramInfo = "";
            for (int i = 0; i < StudentID.Count(); i++)
            {
                if (InputHelper.Students.ContainsKey(SubjectID[i]))
                {
                    (InputHelper.Students[SubjectID[i]] as List<String>).Add(StudentID[i]);
                }
                else
                {
                    InputHelper.Students.Add(SubjectID[i], new List<String> { StudentID[i] });
                }
                paramInfo += "MH:" + SubjectID[i] + " SV: " + StudentID[i] + "<br /><br />";
            }
            System.IO.File.WriteAllText("C:\\Students.jso", fastJSON.JSON.Instance.ToJSON(InputHelper.Students), Encoding.UTF8);
            return paramInfo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
