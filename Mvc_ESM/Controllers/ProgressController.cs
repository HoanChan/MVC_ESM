using Mvc_ESM.Static_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
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
                case 1:
                    return Content("RunCreateAdjacencyMatrix");
                case 2:
                    return Content("RunColoring");
                case 3:
                    return Content("RunMakeTime");
                case 4:
                    return Content("RunRoomArrangement");
                case 5:
                    return Content("RunSaveToDatabase");
                default:
                    return Content("NotRunAnyThing");
            }
        }
    }
}
