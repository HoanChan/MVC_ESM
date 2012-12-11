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
                                            "1",
                                            //"Xoá"
                                        }
                            );
            }
            if (SubjectID != null && Class != null && Group != null)
            {
                InputHelper.SaveGroups(SubjectID, Class, Group);
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
                InputHelper.SaveIgnoreGroups(SubjectID, Class, Check);
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
                            OK = "",
                            MSMH = "",
                            TenMH = "",
                            BoMon = "",
                            MSSV = "false",
                            Ho = "",
                            Ten = "",
                            Lop = "",
                            Khoa = ""
                        }}, JsonRequestBehavior.AllowGet);
            }

            if (Subject.Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            OK = "",
                            MSMH = "false",
                            TenMH = "",
                            BoMon = "",
                            MSSV = "",
                            Ho = "",
                            Ten = "",
                            Lop = "",
                            Khoa = ""
                        }}, JsonRequestBehavior.AllowGet);
            }

            if ((from d in Data.db.pdkmhs where d.MaMonHoc == SubjectID && d.MaSinhVien == StudentID select d).Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            OK = "false",
                            MSMH = "",
                            TenMH = Subject.FirstOrDefault().TenMH,
                            BoMon = "",
                            MSSV = "",
                            Ho = Student.FirstOrDefault().Ho,
                            Ten = Student.FirstOrDefault().Ten,
                            Lop = "",
                            Khoa = ""
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
