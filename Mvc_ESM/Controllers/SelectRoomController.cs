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
        public String SelectSuccess(List<String> RoomID, List<int> Container)
        {
            Static_Helper.InputHelper.Rooms = new List<Static_Helper.InputHelper.Room>();
            string paramInfo = "";
            for (int i = 0; i < RoomID.Count; i++)
            {
                Static_Helper.InputHelper.Rooms.Add(new Static_Helper.InputHelper.Room(RoomID[i], Container[i]));
                paramInfo += "MP:" + RoomID[i] + " SC: " + Container[i] + "<br /><br />";
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