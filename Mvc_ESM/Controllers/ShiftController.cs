using Mvc_ESM.Static_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShiftController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SelectSuccess(List<String> Shift)
        {
            InputHelper.Shifts = new List<Shift>();
            
            for (int i = 0; i < Shift.Count; i++)
            {
                int days = i / InputHelper.Options.Times.Count;
                int time = i % InputHelper.Options.Times.Count;
                DateTime ShiftTime = InputHelper.Options.StartDate.AddDays(days)
                                                                  .AddHours(InputHelper.Options.Times[time].Hour)
                                                                  .AddMinutes(InputHelper.Options.Times[time].Minute);
                InputHelper.Shifts.Add(new Shift() { IsBusy = Shift[i] == "checked", Time = ShiftTime });
            }
            OutputHelper.SaveOBJ("Shift", InputHelper.Shifts);
            return Content("OK");

        }
    }
}
