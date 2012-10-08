using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Models;

namespace Mvc_ESM.Controllers
{
    public class SelectSinhVienController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /SelectSinhVien/
        [HttpGet]
        public ViewResult Index()
        {
            var sinhviens = (from s in db.sinhviens
                             join d in db.pdkmhs on s.MaSinhVien equals d.MaSinhVien
                             join m in db.monhocs on d.MaMonHoc equals m.MaMonHoc
                             where m.MaMonHoc == Static_Helper.StudentHelper.MonHoc
                             select s).Distinct();
            InitViewBag(false);
            return View(sinhviens.ToList());
        }

        [HttpPost]
        public ViewResult Index(String MonHoc)
        {
        //    Static_Helper.StudentHelper.Khoa = Khoa;
          //  Static_Helper.StudentHelper.BoMon = BoMon;
            Static_Helper.StudentHelper.MonHoc = MonHoc;

            var sinhviens = (from s in db.sinhviens
                             join d in db.pdkmhs on s.MaSinhVien equals d.MaSinhVien
                             join m in db.monhocs on d.MaMonHoc equals m.MaMonHoc
                             where m.MaMonHoc == MonHoc
                             select s).Distinct();

            InitViewBag(true);
            return View(sinhviens.ToList());
        }

        private void InitViewBag(Boolean IsPost)
        {
            var KhoaQry = from d in db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");

            var BoMonQry = from b in db.bomons
                           where b.khoa.MaKhoa == (IsPost ? Static_Helper.StudentHelper.Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                           select new { MaBoMon = b.MaBoMon, TenBoMon = b.TenBoMon };
            ViewBag.BoMon = new SelectList(BoMonQry.ToArray(), "MaBoMon", "TenBoMon");

            var MonHocQry = from m in db.monhocs
                            where m.BoMonQL == (IsPost ? Static_Helper.StudentHelper.BoMon : BoMonQry.FirstOrDefault().MaBoMon)
                            select new { MaMonHoc = m.MaMonHoc, TenMonHoc = m.TenMonHoc };
            ViewBag.MonHoc = new SelectList(MonHocQry.ToArray(), "MaMonHoc", "TenMonHoc");
        }

        [HttpPost]
        public String SelectSuccess(List<String> StudentID)
        {
            Static_Helper.InputHelper.Student = StudentID;
            string paramInfo = "";
            foreach (String si in StudentID)
            {
                paramInfo += "Value:" + si + "<br /><br />";
            }
            return paramInfo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}