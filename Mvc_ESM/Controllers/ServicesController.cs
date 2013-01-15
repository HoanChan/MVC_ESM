using Model;
using Mvc_ESM.Static_Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    public class ServicesController : Controller
    {
        [HttpGet]
        public JsonResult LoadStudentTimetable(String StudentID)
        {
            IEnumerable<pdkmh> dkmhs = InputHelper.db.pdkmhs.Where(m => m.MaSinhVien == StudentID);
            var aData = (from dk in dkmhs
                         join lh in InputHelper.db.lichhocvus on (dk.MaMonHoc + dk.Nhom) equals (lh.MaMonHoc + lh.Nhom)
                         select new
                         {
                             dk.MaMonHoc,
                             dk.nhom1.monhoc.TenMonHoc,
                             SoTC = dk.nhom1.monhoc.TCLyThuyet,
                             dk.Nhom,
                             lh.MaPhong,
                             lh.SoTiet,
                             lh.SoTuan,
                             lh.TietBD,
                             lh.TuanBD,
                             lh.Thu,
                             lh.nhom1.giaovien.TenGiaoVien
                         }
                         ).ToList();
                        
            if (aData.Count() > 0)
            {
                return Json(new
                {
                    Ok = "true",
                    Data = aData
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Ok = "false" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LoadRoomsByDateAndShift(long DateMilisecond, int Shift)
        {
            DateTime realDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(DateMilisecond).Date + InputHelper.Options.Times[Shift].TimeOfDay;
            var aData = InputHelper.BusyRooms.FirstOrDefault(m=>m.Time == realDate);
            if (aData != null)
            {
                return Json(new
                {
                    Ok = "true",
                    Rooms = aData.Rooms
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Ok = "false" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LoadTimesByDate(long DateMilisecond)
        {
            DateTime realDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(DateMilisecond);
            var aData = (from m in InputHelper.Shifts
                         where !m.IsBusy && m.Time.Date == realDate.Date
                         select m.Time.ToString("dd/MM/yyy HH:mm")
                        ).ToList();
            if (aData.Count() > 0)
            {
                return Json(new
                {
                    Ok = "true",
                    Times = aData
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Ok = "false" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LoadGroupsBySubjectID(string SubjectID)
        {
            var Groups = (from m in InputHelper.Groups.Values
                         where m.MaMonHoc == SubjectID && m.IsIgnored
                         select new
                         {
                             m.Nhom,
                             m.SoLuongDK
                         }).ToList();
            if (Groups.Count() > 0)
            {
                var Subject = InputHelper.Groups[SubjectID + "_" + Groups[0].Nhom];
                return Json(new { 
                                    MSMH = SubjectID,
                                    TenMH = Subject.TenMonHoc,
                                    TenKhoa = Subject.TenKhoa,
                                    TenBM = Subject.TenBoMon,
                                    Groups = Groups 
                                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { MSMH = "false" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DataTable_SelectGroups(jQueryDataTableParamModel param, List<String> SubjectID = null, List<String> Class = null, List<int> Group = null, String Search = null)
        {

            var Groups = from m in InputHelper.Groups.Values.Where(g=>!g.IsIgnored) select m;
            var total = Groups.Count();
            if (Search != null && Search != "")
            {
                Groups = Groups.Where(m => m.MaMonHoc.ToLower().Contains(Search.ToLower()) || m.TenMonHoc.ToLower().Contains(Search.ToLower()));
            }
            var Result = new List<string[]>();

            foreach (var su in Groups.OrderBy(m => m.TenMonHoc).Skip(param.iDisplayStart).Take(param.iDisplayLength))
            {
                Result.Add(new string[] {
                                            su.MaMonHoc,
                                            su.TenMonHoc,
                                            su.TenBoMon,
                                            su.TenKhoa,
                                            su.Nhom.ToString(),
                                            su.SoLuongDK.ToString(),
                                            InputHelper.Groups[su.MaMonHoc + "_" + su.Nhom].GroupID.ToString(),
                                            //"Xoá"
                                        }
                            );
            }
            if (SubjectID != null && Class != null && Group != null)
            {
                OutputHelper.SaveGroups(SubjectID, Class, Group, false);
            }
            return Json(new{
                                sEcho = param.sEcho,
                                iTotalRecords = total,
                                iTotalDisplayRecords = Groups.Count(),
                                //iTotalDisplayedRecords = Subjects.Count(),
                                aaData = Result
                            },
                            JsonRequestBehavior.AllowGet
                        );
        }
        
        [HttpPost]
        public JsonResult DataTable_IgnoreGroups(jQueryDataTableParamModel param, List<String> SubjectID = null, List<String> Class = null, List<String> Check = null, String Search = null, String ShowIgnore = null, String ShowNotIgnore = null)
        {

            var Groups = from m in InputHelper.Groups.Values select m;
            var total = Groups.Count();
            if(Search != null && Search != "")
            {
                Groups = Groups.Where(m => m.MaMonHoc.ToLower().Contains(Search.ToLower()) || m.TenMonHoc.ToLower().Contains(Search.ToLower()));
            }
            if(ShowIgnore == "checked" && ShowNotIgnore != "checked")
            {
                Groups = Groups.Where(m => m.IsIgnored == true);
            }
            if(ShowIgnore != "checked" && ShowNotIgnore == "checked")
            {
                Groups = Groups.Where(m => m.IsIgnored == false);
            }
            if (ShowIgnore != "checked" && ShowNotIgnore != "checked")
            {
                Groups = Groups.Where(m => false);
            }
            var Result = new List<string[]>();

            foreach (var su in Groups.OrderBy(m => m.TenMonHoc).Skip(param.iDisplayStart).Take(param.iDisplayLength))
            {
                Result.Add(new string[] {
                                            su.MaMonHoc,
                                            su.TenMonHoc,
                                            su.TenBoMon,
                                            su.TenKhoa,
                                            su.Nhom.ToString(),
                                            su.SoLuongDK.ToString(),
                                            su.IsIgnored?"checked":"",
                                            //"Xoá"
                                        }
                            );
            }
            if (SubjectID != null && Class != null && Check != null)
            {
                OutputHelper.SaveIgnoreGroups(SubjectID, Class, Check, false);
            }
            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = total,
                                iTotalDisplayRecords = Groups.Count(),
                                //iTotalDisplayedRecords = Subjects.Count(),
                                aaData = Result
                            },
                            JsonRequestBehavior.AllowGet
                        );
        }

        [HttpGet]
        public JsonResult GetProgressInfo()
        {
            var isBusy = System.IO.File.Exists(OutputHelper.RealPath("IsBusy"));
            var StatusPath = OutputHelper.RealPath("Status");
            var Status = System.IO.File.Exists(StatusPath) ? JsonConvert.DeserializeObject<String>(System.IO.File.ReadAllText(StatusPath)) : "";
            var Type = Status.Length > 4 ? Status.Substring(0, 3) : "";
            var Info = Status.Length > 4 ? Status.Substring(4) : "";
            return Json(new 
                    {
                        isBusy,
                        Type,
                        Info
                    }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectByGroupInfo(string SubjectID)
        {
            var Groups = from g in InputHelper.db.nhoms
                          where g.MaMonHoc == SubjectID
                          select new
                          {
                              MSMH = g.MaMonHoc,
                              TenMH = g.monhoc.TenMonHoc,
                              BoMon = g.monhoc.bomon.TenBoMon,
                              Khoa = g.monhoc.bomon.khoa.TenKhoa,
                              Nhom = g.Nhom1,
                              SL = g.SoLuongDK
                          };
            if (Groups.Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            MSMH = "false"
                        }}, JsonRequestBehavior.AllowGet);
            }

            return Json(Groups, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadStudentAndSubjectInfo(string StudentID, string SubjectID)
        {
            var Student = from s in InputHelper.db.sinhviens
                          where s.MaSinhVien.Equals(StudentID)
                          select new
                          {
                              MSSV = s.MaSinhVien,
                              Ho = s.Ho,
                              Ten = s.Ten,
                              Lop = s.Lop,
                              Khoa = s.lop1.khoi.khoa.TenKhoa
                          };
            var Subject = from m in InputHelper.db.monhocs
                          where m.MaMonHoc.Equals(SubjectID)
                          select new
                          {
                              MSMH = m.MaMonHoc,
                              TenMH = m.TenMonHoc,
                              BoMon = m.bomon.TenBoMon
                          };
            if (Student.Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            MSSV = "false"
                        }}, JsonRequestBehavior.AllowGet);
            }

            if (Subject.Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            MSMH = "false"
                        }}, JsonRequestBehavior.AllowGet);
            }

            if ((from d in InputHelper.db.pdkmhs where d.MaMonHoc == SubjectID && d.MaSinhVien == StudentID select d).Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            OK = "false",
                            TenMH = Subject.FirstOrDefault().TenMH,
                            Ho = Student.FirstOrDefault().Ho,
                            Ten = Student.FirstOrDefault().Ten
                        }}, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<object>(){ new 
                    {
                        OK = "",
                        MSMH = Subject.FirstOrDefault().MSMH,
                        TenMH = Subject.FirstOrDefault().TenMH,
                        BoMon = Subject.FirstOrDefault().BoMon,
                        MSSV = Student.FirstOrDefault().MSSV,
                        Ho = Student.FirstOrDefault().Ho,
                        Ten = Student.FirstOrDefault().Ten,
                        Lop = Student.FirstOrDefault().Lop,
                        Khoa = Student.FirstOrDefault().Khoa
                    }}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadClassByFacultyName(string FacultyName)
        {
            var aData = from b in InputHelper.db.lops
                       where b.khoi.khoa.TenKhoa == FacultyName || FacultyName == ""
                       select new SelectListItem()
                       {
                           Text = b.MaLop,
                           Value = b.MaLop,
                       };

            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadClassByFacultyID(string FacultyID)
        {
            var aData = from b in InputHelper.db.lops
                       where b.khoi.khoa.MaKhoa == FacultyID
                       select new SelectListItem()
                       {
                           Text = b.MaLop,
                           Value = b.MaLop,
                       };

            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsByFacultyID(string FacultyID)
        {
            var aData = from b in InputHelper.db.bomons
                       where b.khoa.MaKhoa == FacultyID
                       select new SelectListItem()
                       {
                           Text = b.TenBoMon,
                           Value = b.MaBoMon,
                       };

            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsByFacultyName(string FacultyName)
        {
            var aData = (from b in InputHelper.db.bomons
                        where b.khoa.TenKhoa.Replace("&", "và") == FacultyName || b.khoa.TenKhoa == FacultyName || FacultyName == ""
                        select new SelectListItem()
                        {
                            Text = b.TenBoMon,
                            Value = b.MaBoMon,
                        }).Distinct();

            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsBySubject(string SubjectID)
        {
            var aData = (from m in InputHelper.db.monhocs
                        where m.BoMonQL == SubjectID
                        select new SelectListItem()
                         {
                             Text = m.TenMonHoc,
                             Value = m.MaMonHoc,
                         });
            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectByFacultyID(string FacultyID)
        {
            var aData = (from m in InputHelper.db.monhocs
                        where m.bomon.KhoaQL == FacultyID
                        select new SelectListItem()
                        {
                            Text = m.TenMonHoc,
                            Value = m.MaMonHoc,
                        });
            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadRoomsBySubjectID(string SubjectID)
        {
            var aData = (from b in InputHelper.db.This
                       where b.MaMonHoc == SubjectID
                       select new SelectListItem()
                       {
                           Text = b.MaPhong,
                           Value = b.MaPhong,
                       }).Distinct();
            return Json(aData, JsonRequestBehavior.AllowGet);
        }
    }
}
