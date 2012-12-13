using Mvc_ESM.Static_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    public class BusyShiftController : Controller
    {
        //
        // GET: /BusyShift/

        public ActionResult Index()
        {
            ViewBag.BeginDay = InputHelper.Options.StartDate;
            ViewBag.NumDay = InputHelper.Options.NumDate;
            ViewBag.NumShift = InputHelper.Options.Times.Count();
            return View();
        }

        [HttpPost]
        public ActionResult SelectSuccess(List<String> Shift)
        {
            var IsBusyShifts = new List<bool>();
            for (int i = 0; i < Shift.Count; i++)
            {
                IsBusyShifts.Add(Shift[i] == "checked");
            }
            OutputHelper.SaveOBJ("BusyShifts", IsBusyShifts);
            return Content("OK");

        }
    }
}
