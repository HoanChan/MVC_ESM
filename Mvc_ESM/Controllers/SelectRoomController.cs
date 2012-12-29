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
    [Authorize(Roles = "Admin")]
    public class SelectRoomController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public String SelectSuccess(long DateMilisecond, int Shift, List<String> RoomID, List<int> Container, List<String> Check)
        {
            return OutputHelper.SaveRooms(DateMilisecond, Shift, RoomID, Container, Check);
        }
    }
}