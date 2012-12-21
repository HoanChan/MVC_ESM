using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Static_Helper;
using System.Text;
namespace Mvc_ESM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OptionsController : Controller
    {
        //
        // GET: /Options/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public String SelectSuccess(List<long> BGTime, int DateMin, long DateStart, int NumDate, int StepTime)
        {
            //DateStart được tính bằng mili giây
            InputHelper.Options.StartDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(DateStart).Date;
            InputHelper.Options.NumDate = NumDate;
            InputHelper.Options.DateMin = DateMin;
            InputHelper.Options.StepTime = StepTime;
            string paramInfo = "date:" + InputHelper.Options.StartDate.ToString() + "<br/><br/>";
            paramInfo += "DateMin:" + InputHelper.Options.DateMin + "<br/><br/>";
            paramInfo += "NumDate:" + InputHelper.Options.NumDate + "<br/><br/>";
            paramInfo += "StepTime:" + InputHelper.Options.StepTime + "<br/><br/>";
            InputHelper.Options.Times = new List<DateTime>();
            for (int i = 0; i < BGTime.Count(); i++)
            {
                DateTime Time = InputHelper.Options.StartDate.AddMilliseconds(BGTime[i]);
                InputHelper.Options.Times.Add(Time);
                paramInfo += " BGTime = " + Time.ToString() + "<br/><br/>";
            }
            InputHelper.Options.Times.Sort();
            OutputHelper.SaveOBJ("Options", InputHelper.Options);
            return paramInfo;
        }

    }
}
