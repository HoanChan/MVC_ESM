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
    [Authorize(Roles = "Admin")]
    public class HandmadeController : Controller
    {
        //
        // GET: /HandMade/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SelectSuccess(String SubjectID, List<String> Class, string Time, List<String> Room, List<int> Num)
        {   
            var Date = DateTime.ParseExact(Time, "dd/MM/yyyy HH:mm", new CultureInfo("en-US"));
            OutputHelper.SaveOBJ("HandMade", new { SubjectID, Class, Date, Room, Num });
            Process.Start(OutputHelper.WinAppExe, "4"); // Handmade
            foreach (String aClass in Class)
            {
                byte aByte = Convert.ToByte(aClass);
                InputHelper.Groups.FirstOrDefault(m => m.Value.MaMonHoc == SubjectID && m.Value.Nhom == aByte).Value.IsIgnored = false;
            }
            OutputHelper.SaveOBJ("Groups", InputHelper.Groups);
            return Json(new { SubjectID, Class, Date, Room, Num }, JsonRequestBehavior.AllowGet);
        }
    }
}
