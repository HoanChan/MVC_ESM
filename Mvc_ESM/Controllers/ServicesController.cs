﻿using Model;
using Mvc_ESM.Static_Helper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    public class ServicesController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

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
            var Groups = from g in db.nhoms
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
                            MSMH = "false",
                            TenMH = "",
                            BoMon = "",
                            Khoa = "",
                            Nhom = 0,
                            Sl = 0
                        }}, JsonRequestBehavior.AllowGet);
            }

            return Json(Groups, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadStudentAndSubjectInfo(string StudentID, string SubjectID)
        {
            var Student = from s in db.sinhviens
                          where s.MaSinhVien.Equals(StudentID)
                          select new
                          {
                              MSSV = s.MaSinhVien,
                              Ho = s.Ho,
                              Ten = s.Ten,
                              Lop = s.Lop,
                              Khoa = s.lop1.khoi.khoa.TenKhoa
                          };
            var Subject = from m in db.monhocs
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

            if ((from d in db.pdkmhs where d.MaMonHoc == SubjectID && d.MaSinhVien == StudentID select d).Count() == 0)
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
            var Data = from b in db.lops
                       where b.khoi.khoa.TenKhoa == FacultyName || FacultyName == ""
                       select new SelectListItem()
                       {
                           Text = b.MaLop,
                           Value = b.MaLop,
                       };

            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadClassByFacultyID(string FacultyID)
        {
            var Data = from b in db.lops
                       where b.khoi.khoa.MaKhoa == FacultyID
                       select new SelectListItem()
                       {
                           Text = b.MaLop,
                           Value = b.MaLop,
                       };

            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsByFacultyID(string FacultyID)
        {
            var Data = from b in db.bomons
                       where b.khoa.MaKhoa == FacultyID
                       select new SelectListItem()
                       {
                           Text = b.TenBoMon,
                           Value = b.MaBoMon,
                       };

            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsByFacultyName(string FacultyName)
        {
            var Data = (from b in db.bomons
                        where b.khoa.TenKhoa.Replace("&", "và") == FacultyName || b.khoa.TenKhoa == FacultyName || FacultyName == ""
                        select new SelectListItem()
                        {
                            Text = b.TenBoMon,
                            Value = b.MaBoMon,
                        }).Distinct();

            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsBySubject(string SubjectID)
        {
            var Data = (from m in db.monhocs
                        where m.BoMonQL == SubjectID
                        select new SelectListItem()
                         {
                             Text = m.TenMonHoc,
                             Value = m.MaMonHoc,
                         });
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectByFacultyID(string FacultyID)
        {
            var Data = (from m in db.monhocs
                        where m.bomon.KhoaQL == FacultyID
                        select new SelectListItem()
                        {
                            Text = m.TenMonHoc,
                            Value = m.MaMonHoc,
                        });
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadRoomsBySubjectID(string SubjectID)
        {
            var Data = (from b in db.This
                       where b.MaMonHoc == SubjectID
                       select new SelectListItem()
                       {
                           Text = b.MaPhong,
                           Value = b.MaPhong,
                       }).Distinct();
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
