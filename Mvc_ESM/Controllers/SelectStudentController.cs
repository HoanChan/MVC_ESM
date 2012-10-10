using Mvc_ESM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Static_Helper.InputHelper.Student = new Hashtable();
            string paramInfo = "";
            for (int i = 0; i < StudentID.Count(); i++)
            {
                Static_Helper.InputHelper.Student.Add(SubjectID[i], StudentID[i]);
                paramInfo += "MH:" + SubjectID[i] + " SV: " + StudentID[i] + "<br /><br />";
            }
            return paramInfo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
