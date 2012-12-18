using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Static_Helper;
using System.Diagnostics;

namespace Mvc_ESM.Controllers
{
    public class HandmadeController : Controller
    {
        //
        // GET: /HandMade/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SelectSuccess(String MSMH, List<String> Class, string Time, List<String> Room, List<int> Num)
        {   
            var Date = DateTime.ParseExact(Time, "dd/MM/yyyy HH:mm", new CultureInfo("en-US"));
            OutputHelper.SaveOBJ("HandMade", new { MSMH, Class, Date, Room, Num });
            Process.Start(OutputHelper.WinAppExe, "6"); // Handmade
            return Json(new { MSMH, Class, Date, Room, Num}, JsonRequestBehavior.AllowGet);
        }
    }
}
