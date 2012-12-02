using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Collections.Specialized;
using System.Collections;
using Mvc_ESM.Static_Helper;
using System.Text;

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
            return View();
        }

        [HttpGet]
        public ViewResult IgnoreList()
        {
            return View();
        }

        [HttpPost]
        public String SelectSuccess(List<String> SubjectID, List<String> Class, List<int> Group)
        {
            return InputHelper.SaveGroups(SubjectID, Class, Group);
        }

        [HttpPost]
        public String SelectSuccess(List<String> SubjectID, List<String> Class, List<Boolean> Check)
        {
            return "";
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}