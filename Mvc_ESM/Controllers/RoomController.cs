using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Collections;
using Mvc_ESM.Static_Helper;

namespace Mvc_ESM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            var phongs = (from r in InputHelper.db.phongs
                          select r);
            return View(phongs.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Phong)
        {
            var phongs = (from r in InputHelper.db.phongs
                          select r);
            return View(phongs.ToList());
        }
    }
}