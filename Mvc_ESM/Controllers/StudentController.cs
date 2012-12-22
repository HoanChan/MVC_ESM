using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Mvc_ESM.Static_Helper;

namespace Mvc_ESM.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        [HttpGet]
        public ViewResult Index()
        {
            StudentHelper.Khoa = (from d in Data.db.khoas
                                                orderby d.TenKhoa
                                                select d).FirstOrDefault().MaKhoa;
            StudentHelper.SearchString = "";
            StudentHelper.Lop = "Tất cả";
            var students = (from m in Data.db.pdkmhs
                            where m.sinhvien.lop1.khoi.KhoaQL.Equals(StudentHelper.Khoa)
                            select m.sinhvien
                           ).Distinct().Include(m => m.lop1);
            InitViewBag(false);
            return View(students.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Khoa, String Lop, String SearchString)
        {
            StudentHelper.Khoa = Khoa;
            StudentHelper.SearchString = SearchString;
            StudentHelper.Lop = Lop;
            var sinhviens = (from m in Data.db.pdkmhs
                             where (Lop == "" && m.sinhvien.lop1.khoi.KhoaQL.Equals(Khoa)) || (Lop != "" && m.sinhvien.lop1.MaLop.Equals(Lop)) && (m.sinhvien.Ten.Contains(SearchString) || SearchString == "")
                             select m.sinhvien
                           ).Distinct().Include(m => m.lop1);
            InitViewBag(true);
            return View(sinhviens.ToList());
        }

        private void InitViewBag(Boolean IsPost)
        {
            var KhoaQry = from d in Data.db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");
            var ClassQry = from b in Data.db.lops
                           where b.khoi.KhoaQL == (IsPost ? StudentHelper.Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                           orderby b.MaLop
                           select new { MaLop = b.MaLop, TenLop = b.MaLop };
            ViewBag.Lop = new SelectList(ClassQry.ToArray(), "MaLop", "TenLop");
            ViewBag.SearchString = "";
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}