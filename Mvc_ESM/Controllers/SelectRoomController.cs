using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Models;

namespace Mvc_ESM.Controllers
{ 
    public class SelectRoomController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /SelectRoom/

        public ViewResult Index()
        {
            var phongs = db.phongs.Include(p => p.khoa);
            return View(phongs.ToList());
        }

        [HttpPost]
        public String SelectSuccess(List<String> RoomID)
        {
            Static_Helper.InputHelper.Rooms = RoomID;
            string paramInfo = "";
            foreach (String si in RoomID)
            {
                paramInfo += "Value:" + si + "<br /><br />";
            }
            return paramInfo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}