using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            Static_Helper.InputHelper.StartDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(DateStart - DateStart % 86400000).AddDays(1);
            Static_Helper.InputHelper.NumDate = NumDate;
            Static_Helper.InputHelper.DateMin = DateMin;
            string paramInfo = "date:" + Static_Helper.InputHelper.StartDate.ToString() + "<br/><br/>";
            paramInfo += "DateMin:" + Static_Helper.InputHelper.DateMin + "<br/><br/>";
            paramInfo += "NumDate:" + Static_Helper.InputHelper.NumDate + "<br/><br/>";
            Static_Helper.InputHelper.DateMin = DateMin;
            Static_Helper.InputHelper.NumDate = NumDate;
            Static_Helper.InputHelper.Times = new List<Static_Helper.InputHelper.ExamTime>();
            for (int i = 0; i < Name.Count(); i++)
            {
                Static_Helper.InputHelper.ExamTime ET = new Static_Helper.InputHelper.ExamTime();
                ET.Name = Name[i];
                ET.BGTime = Static_Helper.InputHelper.StartDate.AddMilliseconds(BGTime[i]).AddHours(7);
                ET.ETime = Static_Helper.InputHelper.StartDate.AddMilliseconds(ETime[i]).AddHours(7);
                Static_Helper.InputHelper.Times.Add(ET);
                paramInfo += "ET[" + i + "]: {Name ='" + ET.Name + "', BGTime = " + ET.BGTime.ToString() + ", ETime=" + ET.ETime.ToString() + "}<br/><br/>";
            }
            
            return paramInfo;
        }

    }
}
