using Model;
using Mvc_ESM.Static_Helper;
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
            IEnumerable<pdkmh> dkmhs = Data.db.pdkmhs.Where(m => m.MaSinhVien == StudentID);
            var aData = (from dk in dkmhs
                         join lh in Data.db.lichhocvus on (dk.MaMonHoc + dk.Nhom) equals (lh.MaMonHoc + lh.Nhom)
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
        public JsonResult LoadRoomsByDate(long DateMilisecond)
        {
            DateTime realDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(DateMilisecond);
            var aData = (from m in InputHelper.BusyRooms
                         where m.Time.Date == realDate.Date
                         select m.Rooms
                        ).ToList();
            if (aData.Count() > 0)
            {
                return Json(new
                {
                    Ok = "true",
                    Rooms = aData[0]
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
            var aData = (from m in InputHelper.BusyShifts
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
            var Groups = (from m in Data.Groups.Values
                         where m.MaMonHoc == SubjectID && m.IsIgnored
                         select new
                         {
                             m.Nhom,
                             m.SoLuongDK
                         }).ToList();
            if (Groups.Count() > 0)
            {
                var Subject = Data.Groups[SubjectID + "_" + Groups[0].Nhom];
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
        public JsonResult DataTable_SelectGroups(jQueryDataTableParamModel param, List<String> SubjectID = null, List<String> Class = null, List<int> Group = null)
        {

            var Groups = (from m in Data.Groups.Values.Where(g=>!g.IsIgnored)
                            where param.sSearch == null || param.sSearch == "" || m.MaMonHoc.Contains(param.sSearch) || m.TenMonHoc.Contains(param.sSearch)
                            select m
                           ).OrderBy(m => m.TenMonHoc);
            var Result = new List<string[]>();

            foreach (var su in Groups.Skip(param.iDisplayStart).Take(param.iDisplayLength))
            {
                Result.Add(new string[] {
                                            su.MaMonHoc,
                                            su.TenMonHoc,
                                            su.TenBoMon,
                                            su.TenKhoa,
                                            su.Nhom.ToString(),
                                            su.SoLuongDK.ToString(),
                                            Data.Groups[su.MaMonHoc + "_" + su.Nhom].GroupID.ToString(),
                                            //"Xoá"
                                        }
                            );
            }
            if (SubjectID != null && Class != null && Group != null)
            {
                OutputHelper.SaveGroups(SubjectID, Class, Group);
            }
            return Json(new{
                                sEcho = param.sEcho,
                                iTotalRecords = Data.Groups.Count(),
                                iTotalDisplayRecords = Groups.Count(),
                                //iTotalDisplayedRecords = Subjects.Count(),
                                aaData = Result
                            },
                            JsonRequestBehavior.AllowGet
                        );
        }
        
        [HttpPost]
        public JsonResult DataTable_IgnoreGroups(jQueryDataTableParamModel param, List<String> SubjectID = null, List<String> Class = null, List<String> Check = null)
        {

            var Groups = (from m in Data.Groups.Values
                            where param.sSearch == null || param.sSearch == "" || m.MaMonHoc.Contains(param.sSearch) || m.TenMonHoc.Contains(param.sSearch)
                            select m
                           ).OrderBy(m => m.TenMonHoc);
            var Result = new List<string[]>();

            foreach (var su in Groups.Skip(param.iDisplayStart).Take(param.iDisplayLength))
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
                OutputHelper.SaveIgnoreGroups(SubjectID, Class, Check);
            }
            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = Data.Groups.Count(),
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
            return Json(new List<object>(){ new 
                    {
                        pbCreateMatrix = ProgressHelper.pbCreateMatrix,
                        CreateMatrixInfo = ProgressHelper.CreateMatrixInfo
                    }}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectByGroupInfo(string SubjectID)
        {
            var Groups = from g in Data.db.nhoms
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
            var Student = from s in Data.db.sinhviens
                          where s.MaSinhVien.Equals(StudentID)
                          select new
                          {
                              MSSV = s.MaSinhVien,
                              Ho = s.Ho,
                              Ten = s.Ten,
                              Lop = s.Lop,
                              Khoa = s.lop1.khoi.khoa.TenKhoa
                          };
            var Subject = from m in Data.db.monhocs
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

            if ((from d in Data.db.pdkmhs where d.MaMonHoc == SubjectID && d.MaSinhVien == StudentID select d).Count() == 0)
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
            var aData = from b in Data.db.lops
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
            var aData = from b in Data.db.lops
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
            var aData = from b in Data.db.bomons
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
            var aData = (from b in Data.db.bomons
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
            var aData = (from m in Data.db.monhocs
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
            var aData = (from m in Data.db.monhocs
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
            var aData = (from b in Data.db.This
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
