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
            var students = (from m in InputHelper.db.pdkmhs
                            where m.sinhvien.lop1.khoi.KhoaQL.Equals(InputHelper.db.khoas.FirstOrDefault().MaKhoa)
                            select m.sinhvien
                           ).Distinct().Include(m => m.lop1);
            InitViewBag(false);
            return View(students.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Khoa, String Lop, String SearchString)
        {
            var Students = from m in InputHelper.db.pdkmhs select m.sinhvien;
            if(!string.IsNullOrEmpty(Khoa))
            {
                Students = Students.Where(m => m.lop1.khoi.KhoaQL.Equals(Khoa));
            }
            if (!string.IsNullOrEmpty(Lop))
            {
                Students = Students.Where(m => m.lop1.MaLop.Equals(Lop));
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                Students = Students.Where(m => m.Ten.Contains(SearchString) || m.Ho.Contains(SearchString) || (m.Ho + " " + m.Ten).Contains(SearchString));
            }
            Students = Students.Distinct().Include(m => m.lop1);
            InitViewBag(true, SearchString, Khoa);
            return View(Students.ToList());
        }

        private void InitViewBag(Boolean IsPost, string SearchString = "", string Khoa = "")
        {
            var KhoaQry = from d in InputHelper.db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");
            var ClassQry = from b in InputHelper.db.lops
                           where b.khoi.KhoaQL.Equals(IsPost ? Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                           orderby b.MaLop
                           select new { MaLop = b.MaLop, TenLop = b.MaLop };
            ViewBag.Lop = new SelectList(ClassQry.ToArray(), "MaLop", "TenLop");
            ViewBag.SearchString = SearchString;
        }
    }
}