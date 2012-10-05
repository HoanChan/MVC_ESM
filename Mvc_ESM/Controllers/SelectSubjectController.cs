using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Models;
using System.Collections.Specialized;
using System.Collections;

namespace Mvc_ESM.Controllers
{ 
    public class SelectSubjectController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /SelectSubject/
        [HttpGet]
        public ViewResult Index()
        {
            var monhocs = (from d in db.pdkmhs
                           join m in db.monhocs on d.MaMonHoc equals m.MaMonHoc
                           select m).Distinct();
            return View(monhocs.ToList());
        }

        [HttpPost]
        public String SelectSuccess(List<String> SubjectID)
        {
            Static_Helper.InputHelper.Subjects = SubjectID;
            string paramInfo = "";
            foreach (String si in SubjectID)
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