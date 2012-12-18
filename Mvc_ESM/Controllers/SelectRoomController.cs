using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Mvc_ESM.Static_Helper;
using System.Text;

namespace Mvc_ESM.Controllers
{
    public class SelectRoomController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /SelectRoom/

        public ViewResult Index()
        {
            var phongs = db.phongs.Where(p => p.SucChua > 0).Include(p => p.khoa);
            return View(phongs.ToList());
        }

        [HttpPost]
        public String SelectSuccess(List<String> RoomID, List<int> Container)
        {
            InputHelper.Rooms = new List<Room>();
            string paramInfo = "";
            for (int i = 0; i < RoomID.Count; i++)
            {
                InputHelper.Rooms.Add(new Room() { RoomID = RoomID[i], Container = Container[i], IsBusy = false });
                paramInfo += "MP:" + RoomID[i] + " SC: " + Container[i] + "<br /><br />";
            }
            OutputHelper.SaveOBJ("Rooms", InputHelper.Rooms);
            return paramInfo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}