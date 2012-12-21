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
    [Authorize(Roles = "Admin")]
    public class SelectGroupController : Controller
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
        public String IgnoreSuccess(List<String> SubjectID, List<String> Class, List<String> Check)
        {
            return InputHelper.SaveIgnoreGroups(SubjectID, Class, Check);
        }

        [HttpPost]
        public String SelectSuccess(List<String> SubjectID, List<String> Class, List<int> Group)
        {
            return InputHelper.SaveGroups(SubjectID, Class, Group);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}