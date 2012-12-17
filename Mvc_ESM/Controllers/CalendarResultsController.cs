
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM;
using Mvc_ESM.Models;
using Newtonsoft.Json;
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

        public ActionResult RoomsData(String id)
        {
            List<Static_Helper.Event> SubjectTime = (from s in db.This
                                                     where s.MaPhong == id
                                                     select new Static_Helper.Event()
                                                     {
                                                         id = s.MaMonHoc + s.Nhom,
                                                         text = s.monhoc.TenMonHoc + s.MaPhong,
                                                         start_date = s.CaThi.GioThi,
                                                         end_date = s.CaThi.GioThi,
                                                     }).Distinct().ToList<Static_Helper.Event>();
            for (int i = 0; i < SubjectTime.Count(); i++)
            {
                SubjectTime[i].start_date = SubjectTime[i].start_date;
                SubjectTime[i].end_date = SubjectTime[i].end_date.AddHours(2);
            }

            return Content(Static_Helper.Calendar.DataFormater(SubjectTime, false), "text/xml");
        }

        public ActionResult StudentsData(String id)
        {
            List<Static_Helper.Event> SubjectTime = (from s in db.This
                                                     where s.MaSinhVien == id
                                                     select new Static_Helper.Event()
                                                     {
                                                         id = s.MaMonHoc + s.Nhom,
                                                         text = s.monhoc.TenMonHoc + s.MaPhong,
                                                         start_date = s.CaThi.GioThi,
                                                         end_date = s.CaThi.GioThi,
                                                         MaPhong = s.MaPhong
                                                     }).ToList<Static_Helper.Event>();
            for (int i = 0; i < SubjectTime.Count(); i++)
            {
                SubjectTime[i].start_date = SubjectTime[i].start_date;
                SubjectTime[i].end_date = SubjectTime[i].end_date.AddHours(2);
            }

            return Content(Static_Helper.Calendar.DataFormater(SubjectTime, true), "text/xml");
        }


        [HttpGet]
        public ActionResult StudentsOfSubjects()
        {
            var sv = (from s in db.sinhviens
                      join m in db.This on s.MaSinhVien equals m.MaSinhVien
                      where m.MaMonHoc == ""
                      select s).Distinct();
            InitViewBag(false, 0);
            return View(sv.ToList());
        }

        [HttpPost]
        public ActionResult StudentsOfSubjects(String MonHoc)
        {
            var sv = (from s in db.sinhviens
                      join m in db.This on s.MaSinhVien equals m.MaSinhVien
                      where m.MaMonHoc == MonHoc
                      select s).Distinct();
            InitViewBag(false, 0);
            return View(sv.OrderBy(s => s.Ten + s.Ho).ToList());
        }

        [HttpGet]
        public ActionResult StudentsOfRooms()
        {
            var sv = (from s in db.sinhviens
                      join m in db.This on s.MaSinhVien equals m.MaSinhVien
                      where m.MaMonHoc == ""
                      select s).Distinct();
            InitViewBag(false, 1);
            return View(sv.OrderBy(s => s.Ten + s.Ho).ToList());
        }

        [HttpPost]
        public ActionResult StudentsOfRooms(String MonHoc, String Phong)
        {
            Static_Helper.SubjectHelper.SearchString = MonHoc;
            var sv = (from s in db.sinhviens
                      join m in db.This on s.MaSinhVien equals m.MaSinhVien
                      where m.MaMonHoc == MonHoc && (m.MaPhong == Phong || Phong == "")
                      select s).Distinct();
            InitViewBag(true, 1);
            return View(sv.OrderBy(s => s.Ten + s.Ho).ToList());
        }

        private void InitViewBag(Boolean IsPost, int k)
        {
            var MonQry = (from d in db.This
                          select new { MaMH = d.MaMonHoc, TenMH = (from m in db.monhocs where m.MaMonHoc == d.MaMonHoc select m.TenMonHoc).FirstOrDefault() }).Distinct().OrderBy(d => d.TenMH);
            ViewBag.MonHoc = new SelectList(MonQry.ToArray(), "MaMH", "TenMH");
            if (k == 1)
            {
                var PhongQry = (from b in db.This
                                where b.MaMonHoc == (IsPost ? Static_Helper.SubjectHelper.SearchString : MonQry.FirstOrDefault().MaMH)
                                select new { MaPhong = b.MaPhong, TenPhong = b.MaPhong }).Distinct();
                ViewBag.Phong = new SelectList(PhongQry.ToArray(), "MaPhong", "TenPhong");
            }
        }


        public ActionResult RoomList()
        {
            //STT	Phòng	Mã môn thi	Tên môn thi	SLSV	Ngày thi	Tiết bắt đầu	Số tiết
            var rooms = (from m in db.This
                         select new
                         {
                             m.MaPhong,
                             m.MaMonHoc,
                             m.monhoc.TenMonHoc,
                             m.CaThi.GioThi,
                             m.Nhom
                         }).Distinct();
            List<string[]> Result = new List<string[]>();
            int stt = 0;
            foreach (var r in rooms)
            {
                string[] s = new string[7];
                s[0] = ++stt + "";
                s[1] = r.MaPhong;
                s[2] = r.MaMonHoc;
                s[3] = r.TenMonHoc;

                s[4] = (from t in db.This
                        where t.MaMonHoc == r.MaMonHoc && t.MaPhong == r.MaPhong && t.Nhom == r.Nhom
                        select t.MaSinhVien).Count() + "";
                s[5] = r.GioThi.Date.ToShortDateString();
                s[6] = r.GioThi.TimeOfDay.Hours + "h" + r.GioThi.TimeOfDay.Minutes;
                Result.Add(s);
            }
            return View(Result);
        }

        public ActionResult OpenRooms()
        {
            var rooms = (from r in db.This
                         select new
                         {
                             r.MaPhong,
                             r.CaThi.GioThi
                         });
            List<string[]> Result = new List<string[]>();
            int stt = 0;
            String Thu2 = "oooooooooooo";
            String Thu3 = "oooooooooooo";
            String Thu4 = "oooooooooooo";
            String Thu5 = "oooooooooooo";
            String Thu6 = "oooooooooooo";
            String Thu7 = "oooooooooooo";

            foreach (var r in rooms)
            {
                string[] s = new string[4];
                s[0] = ++stt + "";
                s[1] = r.GioThi.Date.ToShortDateString();
                s[2] = r.GioThi.TimeOfDay.Hours + "h" + r.GioThi.TimeOfDay.Minutes;
                s[3] = r.MaPhong;
                Result.Add(s);
            }
            return View(Result);
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
