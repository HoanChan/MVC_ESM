
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
using Mvc_ESM.Static_Helper;
namespace Mvc_ESM.Controllers
{
    [Authorize]
    public class CalendarResultsController : Controller
    {

        [Authorize(Roles = "Admin, GiaoVien")]
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
            List<Event> SubjectTime = (from s in InputHelper.db.This
                                                     where s.MaPhong == id
                                                     select new Event()
                                                     {
                                                         id = s.MaMonHoc + s.Nhom,
                                                         text = s.monhoc.TenMonHoc,
                                                         start_date = s.CaThi.GioThi,
                                                         end_date = s.CaThi.GioThi,
                                                         MaPhong = s.Nhom
                                                     }).Distinct().ToList<Event>();
            for (int i = 0; i < SubjectTime.Count(); i++)
            {
                SubjectTime[i].start_date = SubjectTime[i].start_date;
                SubjectTime[i].end_date = SubjectTime[i].end_date.AddHours(2);
            }

            return Content(Calendar.DataFormater(SubjectTime, true), "text/xml");
        }

        public ActionResult StudentsData(String id)
        {
            List<Event> SubjectTime = (from s in InputHelper.db.This
                                                     where s.MaSinhVien == id
                                                     select new Event()
                                                     {
                                                         id = s.MaMonHoc + s.Nhom,
                                                         text = s.monhoc.TenMonHoc,
                                                         start_date = s.CaThi.GioThi,
                                                         end_date = s.CaThi.GioThi,
                                                         MaPhong = s.MaPhong
                                                     }).ToList<Event>();
            for (int i = 0; i < SubjectTime.Count(); i++)
            {
                SubjectTime[i].start_date = SubjectTime[i].start_date;
                SubjectTime[i].end_date = SubjectTime[i].end_date.AddHours(2);
            }

            return Content(Calendar.DataFormater(SubjectTime, true), "text/xml");
        }


        [HttpGet]
        public ActionResult StudentsOfSubjects()
        {
            var sv = (from s in InputHelper.db.sinhviens
                      join m in InputHelper.db.This on s.MaSinhVien equals m.MaSinhVien
                      where m.MaMonHoc == ""
                      select s).Distinct();
            InitViewBag(false, 0);
            return View(sv.ToList());
        }

        [HttpPost]
        public ActionResult StudentsOfSubjects(String MonHoc)
        {
            var sv = (from s in InputHelper.db.sinhviens
                      join m in InputHelper.db.This on s.MaSinhVien equals m.MaSinhVien
                      where m.MaMonHoc == MonHoc
                      select s).Distinct();
            InitViewBag(false, 0);
            return View(sv.OrderBy(s => s.Ten + s.Ho).ToList());
        }

        [HttpGet]
        public ActionResult StudentsOfRooms()
        {
            var sv = (from s in InputHelper.db.sinhviens
                      join m in InputHelper.db.This on s.MaSinhVien equals m.MaSinhVien
                      where m.MaMonHoc == ""
                      select s).Distinct();
            InitViewBag(false, 1);
            return View(sv.OrderBy(s => s.Ten + s.Ho).ToList());
        }

        [HttpPost]
        public ActionResult StudentsOfRooms(String MonHoc, String Phong)
        {
            var sv = (from s in InputHelper.db.sinhviens
                      join m in InputHelper.db.This on s.MaSinhVien equals m.MaSinhVien
                      where m.MaMonHoc == MonHoc && (m.MaPhong == Phong || Phong == "")
                      select s).Distinct();
            InitViewBag(true, 1, MonHoc);
            return View(sv.OrderBy(s => s.Ten + s.Ho).ToList());
        }

        private void InitViewBag(Boolean IsPost, int k, string SearchString = "")
        {
            var MonQry = (from d in InputHelper.db.This
                          select new { MaMH = d.MaMonHoc, TenMH = (from m in InputHelper.db.monhocs where m.MaMonHoc == d.MaMonHoc select m.TenMonHoc).FirstOrDefault() }).Distinct().OrderBy(d => d.TenMH);
            ViewBag.MonHoc = new SelectList(MonQry.ToArray(), "MaMH", "TenMH");
            if (k == 1)
            {
                var PhongQry = (from b in InputHelper.db.This
                                where b.MaMonHoc == (IsPost ? SearchString : MonQry.FirstOrDefault().MaMH)
                                select new { MaPhong = b.MaPhong, TenPhong = b.MaPhong }).Distinct();
                ViewBag.Phong = new SelectList(PhongQry.ToArray(), "MaPhong", "TenPhong");
            }
        }


        public ActionResult RoomList()
        {
            //STT	Phòng	Mã môn thi	Tên môn thi	SLSV	Ngày thi	Tiết bắt đầu	Số tiết
            var rooms = (from m in InputHelper.db.This
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

                s[4] = (from t in InputHelper.db.This
                        where t.MaMonHoc == r.MaMonHoc && t.MaPhong == r.MaPhong && t.Nhom == r.Nhom
                        select t.MaSinhVien).Count() + "";
                s[5] = r.GioThi.Date.ToShortDateString();
                // s[6] = r.GioThi.TimeOfDay.Hours + "h" + r.GioThi.TimeOfDay.Minutes;
                s[6] = r.GioThi.ToString("HH:mm");
                Result.Add(s);
            }
            return View(Result);
        }

        static char[,] Thu;

        static void Init()
        {
            Thu = new char[8, 13];
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 12; j++)
                    Thu[i, j] = 'o';

        }

        static void Check(DateTime time, int i)
        {
            //  String s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11;
            //  s1 = s2 = s3 = s4 = s5 = s6 = s7 = s8 = s9 = s10 = s11 = "o";
            if (time.Hour >= 7 && time.Hour <= 9)
            {
                Thu[i, 0] = Thu[i, 1] = Thu[i, 2] = 'x';
            }
            else
                if (time.Hour > 9 && time.Hour < 12)
                {
                    Thu[i, 3] = Thu[i, 4] = Thu[i, 5] = 'x';
                }
                else
                    if (time.Hour > 12 && time.Hour <= 15)
                    {
                        Thu[i, 6] = Thu[i, 7] = Thu[i, 8] = 'x';
                    }
                    else
                    {
                        Thu[i, 9] = Thu[i, 10] = 'x';
                    }

        }
        [Authorize(Roles = "Admin, GiaoVien")]
        public ActionResult OpenRooms()
        {
            var rooms = (from r in InputHelper.db.This
                         select new
                         {
                             r.MaPhong,
                             r.CaThi.GioThi
                         }).ToList();

            List<string[]> Result = new List<string[]>();
            int stt = 0;
            DateTime StartWeek, EndWeek;
            EndWeek = new DateTime(2012, 1, 1);
            for (int k = 0; k < (InputHelper.Options.NumDate / 7); k++)
            {
                if (k == 0)
                    StartWeek = InputHelper.Options.StartDate;
                else
                    StartWeek = EndWeek.AddDays(2);
                EndWeek = StartWeek.AddDays((OutputHelper.DayOffWeekVN(StartWeek) == "Thứ Hai") ? 5 : (OutputHelper.DayOffWeekVN(StartWeek) == "Thứ Ba") ? 4 : (OutputHelper.DayOffWeekVN(StartWeek) == "Thứ Tư") ? 3 : (OutputHelper.DayOffWeekVN(StartWeek) == "Thứ Năm") ? 2 : (OutputHelper.DayOffWeekVN(StartWeek) == "Thứ Sáu") ? 1 : 0);

                //lọc theo tuần
                var w1 = (from w in rooms
                          where w.GioThi.Date >= StartWeek && w.GioThi.Date <= EndWeek
                          select w).ToList();

                List<string> deny = new List<string>();
                foreach (var r1 in w1)
                    if (!deny.Contains(r1.MaPhong))
                    {
                        deny.Add(r1.MaPhong);
                        Init();
                        string[] s = new string[10];
                        s[0] = ++stt + "";
                        s[1] = StartWeek.ToString("dd/MM/yy") + "-" + EndWeek.ToString("dd/MM/yy");
                        s[2] = r1.MaPhong;

                        foreach (var r2 in w1)
                            if (r2.MaPhong == r1.MaPhong)
                            {
                                DateTime time = r2.GioThi;
                                switch (OutputHelper.DayOffWeekVN(time.Date))
                                {
                                    case "Thứ Hai":
                                        Check(time, 0);
                                        break;
                                    case "Thứ Ba":
                                        Check(time, 1);
                                        break;
                                    case "Thứ Tư":
                                        Check(time, 2);
                                        break;
                                    case "Thứ Năm":
                                        Check(time, 3);
                                        break;
                                    case "Thứ Sáu":
                                        Check(time, 4);
                                        break;
                                    case "Thứ Bảy":
                                        Check(time, 5);
                                        break;
                                }
                            }
                        s[3] = Thu[0, 0].ToString() + Thu[0, 1].ToString() + Thu[0, 2].ToString() + Thu[0, 3].ToString() + Thu[0, 4].ToString() + Thu[0, 5].ToString() + Thu[0, 6].ToString() + Thu[0, 7].ToString() + Thu[0, 8].ToString() + Thu[0, 9].ToString() + Thu[0, 10].ToString() + "";
                        s[4] = Thu[1, 0].ToString() + Thu[1, 1].ToString() + Thu[1, 2].ToString() + Thu[1, 3].ToString() + Thu[1, 4].ToString() + Thu[1, 5].ToString() + Thu[1, 6].ToString() + Thu[1, 7].ToString() + Thu[1, 8].ToString() + Thu[1, 9].ToString() + Thu[1, 10].ToString() + "";
                        s[5] = Thu[2, 0].ToString() + Thu[2, 1].ToString() + Thu[2, 2].ToString() + Thu[2, 3].ToString() + Thu[2, 4].ToString() + Thu[2, 5].ToString() + Thu[2, 6].ToString() + Thu[2, 7].ToString() + Thu[2, 8].ToString() + Thu[2, 9].ToString() + Thu[2, 10].ToString() + "";
                        s[6] = Thu[3, 0].ToString() + Thu[3, 1].ToString() + Thu[3, 2].ToString() + Thu[3, 3].ToString() + Thu[3, 4].ToString() + Thu[3, 5].ToString() + Thu[3, 6].ToString() + Thu[3, 7].ToString() + Thu[3, 8].ToString() + Thu[3, 9].ToString() + Thu[3, 10].ToString() + "";
                        s[7] = Thu[4, 0].ToString() + Thu[4, 1].ToString() + Thu[4, 2].ToString() + Thu[4, 3].ToString() + Thu[4, 4].ToString() + Thu[4, 5].ToString() + Thu[4, 6].ToString() + Thu[4, 7].ToString() + Thu[4, 8].ToString() + Thu[4, 9].ToString() + Thu[4, 10].ToString() + "";
                        s[8] = Thu[5, 0].ToString() + Thu[5, 1].ToString() + Thu[5, 2].ToString() + Thu[5, 3].ToString() + Thu[5, 4].ToString() + Thu[5, 5].ToString() + Thu[5, 6].ToString() + Thu[5, 7].ToString() + Thu[5, 8].ToString() + Thu[5, 9].ToString() + Thu[5, 10].ToString() + "";
                        Result.Add(s);
                    }
            }
            return View(Result);
        }

        [HttpGet]
        public ActionResult StudentSchedule()
        {
            List<String[]> Results = new List<string[]>();
            return View(Results);
        }

        [HttpPost]
        public ActionResult StudentSchedule(String SearchString)
        {
            var sinhviens = (from mh in InputHelper.db.This
                             where mh.MaSinhVien == SearchString
                             select new
                             {
                                 MaMonHoc = mh.MaMonHoc,
                                 MaPhong = mh.MaPhong,
                                 GioThi = mh.CaThi.GioThi
                             }).OrderBy(m => m.GioThi);
            List<String[]> Results = new List<string[]>();
            int stt = 0;
            foreach (var sv in sinhviens)
            {
                string[] s = new string[6];
                s[0] = ++stt + "";
                s[1] = sv.MaMonHoc;
                s[2] = (from mh in InputHelper.db.monhocs
                          where mh.MaMonHoc == sv.MaMonHoc
                          select mh.TenMonHoc).FirstOrDefault();
                 //= s1.ToString();
                s[3] = sv.GioThi.Date.ToString("dd/MM/yyyy");
                s[4] = sv.MaPhong;
                s[5] = sv.GioThi.ToString("HH:mm");
                Results.Add(s);
            }
            return View(Results);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Save(Event changedEvent, FormCollection actionValues)
        {
            String action_type = actionValues["!nativeeditor_status"];
            String source_id = actionValues["id"];
            String target_id = source_id;

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
