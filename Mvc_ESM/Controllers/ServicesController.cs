using Mvc_ESM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    public class ServicesController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        [HttpGet]
        public JsonResult LoadStudentInfo(string StudentID)
        {
            var Data = from s in db.sinhviens
                       where s.MaSinhVien.Equals(StudentID)
                       select new
                       {
                           MSSV = s.MaSinhVien,
                           Ho = s.Ho,
                           Ten = s.Ten,
                           Lop = s.Lop,
                           Khoa = s.lop1.khoi.khoa.TenKhoa
                       };
            if (Data.Count() == 0)
            {
                return Json(new List<object>(){ new 
                {
                    MSSV = "false",
                    Ho = "",
                    Ten = "",
                    Lop = "",
                    Khoa = ""
                }}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Data, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult LoadMonHocByBoMon(string SubjectID)
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
        public JsonResult LoadMonHocByFacultyID(string FacultyID)
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
