using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Models;
using System.Data;

namespace Mvc_ESM.Controllers
{
    public class CalendarActionResponseModel
    {
        public String Status;
        public Int64 Source_id;
        public Int64 Target_id;

        public CalendarActionResponseModel(String status, Int64 source_id, Int64 target_id)
        {
            Status = status;
            Source_id = source_id;
            Target_id = target_id;
        }
    }

    public class CalendarController : Controller
    {
        //
        // GET: /Calendar/
        private EventsEntities db = new EventsEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Data()
        {
            
            return View(db.Events);
        }

        public ActionResult Get()
        {
            return View();
        }
		public ActionResult Save(Event changedEvent, FormCollection actionValues)
		{
			String action_type = actionValues["!nativeeditor_status"];
			Int64 source_id = Int64.Parse(actionValues["id"]);
			Int64 target_id = source_id;
				
			try{
				switch (action_type)
				{
					case "inserted":
                        db.Events.Add(changedEvent);
						break;
					case "deleted":
                        changedEvent = db.Events.SingleOrDefault(ev => ev.id == source_id);
                        db.Events.Remove(changedEvent);
						break;
					default: // "updated"
                        //changedEvent = data.Events.SingleOrDefault(ev => ev.id == source_id);
                        db.Entry(changedEvent).State = EntityState.Modified;
						break;
				}
                db.SaveChanges();
                target_id = changedEvent.id;
			}
			catch
			{
				action_type = "error";
			}
	
			return View(new CalendarActionResponseModel(action_type, source_id, target_id));
		}
	}
}
