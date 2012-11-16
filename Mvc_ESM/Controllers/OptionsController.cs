using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Static_Helper;
using System.Text;
namespace Mvc_ESM.Controllers
{
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
        public String SelectSuccess(List<String> Name, List<long> BGTime, List<long> ETime, int DateMin, long DateStart, int NumDate)
        {
            //DateStart được tính bằng mili giây
            InputHelper.Options.StartDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(DateStart - DateStart % 86400000).AddDays(1);
            InputHelper.Options.NumDate = NumDate;
            InputHelper.Options.DateMin = DateMin;
            string paramInfo = "date:" + InputHelper.Options.StartDate.ToString() + "<br/><br/>";
            paramInfo += "DateMin:" + InputHelper.Options.DateMin + "<br/><br/>";
            paramInfo += "NumDate:" + InputHelper.Options.NumDate + "<br/><br/>";
            InputHelper.Options.DateMin = DateMin;
            InputHelper.Options.NumDate = NumDate;
            InputHelper.Options.Times = new List<ExamTime>();
            for (int i = 0; i < Name.Count(); i++)
            {
                ExamTime ET = new ExamTime();
                ET.Name = Name[i];
                ET.BGTime = InputHelper.Options.StartDate.AddMilliseconds(BGTime[i]).AddHours(7);
                ET.ETime = InputHelper.Options.StartDate.AddMilliseconds(ETime[i]).AddHours(7);
                InputHelper.Options.Times.Add(ET);
                paramInfo += "ET[" + i + "]: {Name ='" + ET.Name + "', BGTime = " + ET.BGTime.ToString() + ", ETime=" + ET.ETime.ToString() + "}<br/><br/>";
            }
            InputHelper.SaveOBJ("Options", InputHelper.Options);
            return paramInfo;
        }

    }
}
