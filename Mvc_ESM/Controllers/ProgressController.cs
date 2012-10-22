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
            Thread thread = new Thread(new ThreadStart(Static_Helper.CreateAdjacencyMatrix.Run));
            thread.Name = "CreateAdjacencyMatrix";
            thread.Start();
            return View();
        }

    }
}
