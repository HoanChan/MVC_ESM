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
            return OutputHelper.SaveIgnoreGroups(SubjectID, Class, Check, true);
        }

        [HttpPost]
        public String SelectSuccess(List<String> SubjectID, List<String> Class, List<int> Group)
        {
            return OutputHelper.SaveGroups(SubjectID, Class, Group, true);
        }
    }
}