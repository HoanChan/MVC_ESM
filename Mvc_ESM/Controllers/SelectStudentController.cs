using Mvc_ESM.Models;
using System;
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
        public String SelectSuccess(List<String> StudentID)
        {
            Static_Helper.InputHelper.Student = StudentID;
            string paramInfo = "";
            foreach (String si in StudentID)
            {
                paramInfo += "Value:" + si + "<br /><br />";
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
