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
            if (SubjectHelper.Khoa == null || SubjectHelper.Khoa == "")
            {
                SubjectHelper.Khoa = (from d in Data.db.khoas
                                                    orderby d.TenKhoa
                                                    select d).FirstOrDefault().MaKhoa;
                SubjectHelper.SearchString = "";
                SubjectHelper.BoMon = "";
                InitViewBag(false);
            }
            else
            {
                InitViewBag(true);
            }
            var monhocs = (from m in Data.db.monhocs
                           where m.bomon.KhoaQL.Equals(SubjectHelper.Khoa)
                           select m
                           ).Include(m => m.bomon).Include(m => m.khoa);
            
            return View(monhocs.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Khoa, String BoMon, String SearchString)
        {
            SubjectHelper.Khoa = Khoa;
            SubjectHelper.SearchString = SearchString;
            SubjectHelper.BoMon = BoMon;
            var monhocs = (from m in Data.db.monhocs 
                           where ((BoMon == "" && m.bomon.KhoaQL.Equals(Khoa)) || (BoMon != "" && m.bomon.MaBoMon.Equals(BoMon))) && (m.TenMonHoc.Contains(SearchString) || SearchString == "")
                           select m
                           ).Include(m => m.bomon).Include(m => m.khoa);
            InitViewBag(true);
            return View(monhocs.ToList());
        }

        private void InitViewBag(Boolean IsPost)
        {
            var KhoaQry = from d in Data.db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");
            var BoMonQry = from b in Data.db.bomons
                            where b.khoa.MaKhoa == (IsPost ? SubjectHelper.Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                            orderby b.TenBoMon
                            select new { MaBoMon = b.MaBoMon, TenBoMon = b.TenBoMon };
            ViewBag.BoMon = new SelectList(BoMonQry.ToArray(), "MaBoMon", "TenBoMon");
            ViewBag.SearchString = "";
        }

        protected override void Dispose(bool disposing)
        {
            //Data.db.Dispose();
            base.Dispose(disposing);
        }
    }
}