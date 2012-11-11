using Mvc_ESM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM;
namespace Mvc_ESM.Controllers
{
    public class CalendarResultsController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        public ActionResult RoomsResult()
        {
            return View();
        }
        public ActionResult StudentsResult()
        {
            return View();
        }
        public ActionResult RoomsData()
        {
            List<Static_Helper.Event> SubjectTime = (from s in db.This
                                                     where s.MaPhong == "A103"
                                                     select new Static_Helper.Event(){  id = s.MaMonHoc, 
                                                                                        text = s.monhoc.TenMonHoc, 
                                                                                        start_date = s.CaThi.NgayThi,
                                                                                        end_date = s.CaThi.NgayThi,
                                                                                        Step = s.CaThi.TietBD}).Distinct().ToList<Static_Helper.Event>();
            int[] Step = new int[] { 7 * 60, 9 * 60 + 30, 13 * 60, 15 * 60 + 30 };
            for (int i = 0; i < SubjectTime.Count(); i++)
            {
                SubjectTime[i].start_date = SubjectTime[i].start_date.AddMinutes(Step[SubjectTime[i].Step]);
                SubjectTime[i].end_date = SubjectTime[i].end_date.AddMinutes(Step[SubjectTime[i].Step]).AddHours(2);
            }
            
            return Content(Static_Helper.Calendar.DataFormater(SubjectTime), "text/xml");
        }

        public ActionResult StudentsData()
        {
            List<Static_Helper.Event> SubjectTime = (from s in db.This
                                                     where s.MaSinhVien == "09910015"
                                                     select new Static_Helper.Event()
                                                     {
                                                         id = s.MaMonHoc,
                                                         text = s.monhoc.TenMonHoc,
                                                         start_date = s.CaThi.NgayThi,
                                                         end_date = s.CaThi.NgayThi,
                                                         Step = s.CaThi.TietBD
                                                     }).ToList<Static_Helper.Event>();
            int[] Step = new int[] { 7 * 60, 9 * 60 + 30, 13 * 60, 15 * 60 + 30 };
            for (int i = 0; i < SubjectTime.Count(); i++)
            {
                SubjectTime[i].start_date = SubjectTime[i].start_date.AddMinutes(Step[SubjectTime[i].Step]);
                SubjectTime[i].end_date = SubjectTime[i].end_date.AddMinutes(Step[SubjectTime[i].Step]).AddHours(2);
            }

            return Content(Static_Helper.Calendar.DataFormater(SubjectTime), "text/xml");
        }

        public ActionResult Save(Event changedEvent, FormCollection actionValues)
        {
            String action_type = actionValues["!nativeeditor_status"];
            Int64 source_id = Int64.Parse(actionValues["id"]);
            Int64 target_id = source_id;

            try
            {
                //switch (action_type)
                //{
                //    case "inserted":
                //        db.Events.Add(changedEvent);
                //        break;
                //    case "deleted":
                //        changedEvent = db.Events.SingleOrDefault(ev => ev.id == source_id);
                //        db.Events.Remove(changedEvent);
                //        break;
                //    default: // "updated"
                //        //changedEvent = data.Events.SingleOrDefault(ev => ev.id == source_id);
                //        db.Entry(changedEvent).State = EntityState.Modified;
                //        break;
                //}
                //db.SaveChanges();
                target_id = changedEvent.id;
            }
            catch
            {
                action_type = "error";
            }

            //return View(new CalendarActionResponseModel(action_type, source_id, target_id));
            String Result = "<data>"
                          + "<action type='" + action_type + "' sid='" + source_id + "' tid='" + target_id + "'></action></data>";
            return Content(Result, "text/xml");
        }

    }
}
