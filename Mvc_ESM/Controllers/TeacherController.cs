using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Collections;
using Mvc_ESM.Static_Helper;

namespace Mvc_ESM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherController : Controller
    {
        // GET: /GiaoVien/
        [HttpGet]
        public ViewResult Index()
        {
            var giaoviens = (from m in InputHelper.db.giaoviens
                             select m
                           ).Include(m => m.bomon);
            InitViewBag(false);
            return View(giaoviens.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Khoa, String BoMon, String SearchString)
        {
            var giaoviens = (from m in InputHelper.db.giaoviens
                             where ((BoMon == "" && m.bomon.KhoaQL.Equals(Khoa)) || (BoMon != "" && m.bomon.MaBoMon.Equals(BoMon))) && ((m.HoLot + " " + m.TenGiaoVien).Contains(SearchString) || SearchString == "")
                             select m
                           ).Include(m => m.bomon);
            InitViewBag(true, Khoa);
            return View(giaoviens.ToList());
        }

        private void InitViewBag(Boolean IsPost, string Khoa = "")
        {
            var KhoaQry = from d in InputHelper.db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");
                
            var BoMonQry = from b in InputHelper.db.bomons
                           where b.khoa.MaKhoa == (IsPost ? Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                           select new { MaBoMon = b.MaBoMon, TenBoMon = b.TenBoMon };
            ViewBag.BoMon = new SelectList(BoMonQry.ToArray(), "MaBoMon", "TenBoMon");
            ViewBag.SearchString = "";
        }
    }
}