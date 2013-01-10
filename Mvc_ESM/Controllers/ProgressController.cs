using Mvc_ESM.Static_Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProgressController : Controller
    {
        //
        // GET: /Progress/
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Run(int StepNumber)
        {
            
            switch (StepNumber)
            {
                case 0:
                    Process.Start(OutputHelper.WinAppExe, "0");
                    return Content("RunDeleteDatabase");
                case 1:
                    Process.Start(OutputHelper.WinAppExe, "1");
                    return Content("RunCreateAdjacencyMatrix");
                case 2:
                    Process.Start(OutputHelper.WinAppExe, "2");
                    return Content("RunCalc");
                case 3:
                    Process.Start(OutputHelper.WinAppExe, "5");
                    return Content("RunSaveToDatabase");
                default:
                    return Content("NotRunAnyThing");
            }
        }
    }
}
