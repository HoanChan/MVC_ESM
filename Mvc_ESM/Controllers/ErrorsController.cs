using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Http404()
        {
            return View();
        }

        public ActionResult Http403()
        {
            return View();
        }

        public ActionResult Http401()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
