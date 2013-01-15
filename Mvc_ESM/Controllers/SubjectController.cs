using Model;
using Mvc_ESM.Static_Helper;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectController : Controller
    {

        //
        // GET: /MonHoc/
        [HttpGet]
        public ViewResult Index()
        {

            InitViewBag(false);
            var monhocs = (from m in InputHelper.db.monhocs
                           where m.bomon.KhoaQL.Equals(InputHelper.db.khoas.FirstOrDefault().MaKhoa)
                           select m
                           ).Include(m => m.bomon).Include(m => m.khoa);
            return View(monhocs.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Khoa, String BoMon, String SearchString)
        {
            var Subjects = (from m in InputHelper.db.monhocs 
                           select m
                           );//.Include(m => m.bomon).Include(m => m.khoa);
            //where ((BoMon == "" && m.bomon.KhoaQL.Equals(Khoa)) || (BoMon != "" && m.bomon.MaBoMon.Equals(BoMon))) && (m.TenMonHoc.Contains(SearchString) || SearchString == "")
            if (!String.IsNullOrEmpty(Khoa))
            {
                Subjects = Subjects.Where(m => m.bomon.KhoaQL.Equals(Khoa));
            }
            if (!String.IsNullOrEmpty(BoMon))
            {
                Subjects = Subjects.Where(m => m.bomon.MaBoMon.Equals(BoMon));
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                Subjects = Subjects.Where(m => m.TenMonHoc.Contains(SearchString));
            }
            InitViewBag(true, SearchString, Khoa);
            return View(Subjects.ToList());
        }

        private void InitViewBag(Boolean IsPost, string SearchString = "", string Khoa = "")
        {
            var KhoaQry = from d in InputHelper.db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");
            var BoMonQry = from b in InputHelper.db.bomons
                            where b.khoa.MaKhoa.Equals(IsPost ? Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                            orderby b.TenBoMon
                            select new { MaBoMon = b.MaBoMon, TenBoMon = b.TenBoMon };
            ViewBag.BoMon = new SelectList(BoMonQry.ToArray(), "MaBoMon", "TenBoMon");
            ViewBag.SearchString = SearchString;
        }
    }
}