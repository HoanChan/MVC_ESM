using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Models;
using System.Collections;

namespace Mvc_ESM.Controllers
{
    public class TeacherController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        [HttpGet]
        public JsonResult LoadBoMonsByKhoa(string KhoaID)
        {
            var Data = from b in db.bomons
                       where b.khoa.MaKhoa == KhoaID
                       select new SelectListItem()
                       {
                           Text = b.TenBoMon,
                           Value = b.MaBoMon,
                       };

            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /GiaoVien/
        [HttpGet]
        public ViewResult Index()
        {
            var giaoviens = (from m in db.giaoviens
                             where ((Static_Helper.TeacherHelper.BoMon == "" && m.bomon.KhoaQL.Equals(Static_Helper.TeacherHelper.Khoa)) || (Static_Helper.TeacherHelper.BoMon != "" && m.bomon.MaBoMon.Equals(Static_Helper.TeacherHelper.BoMon))) && ((m.HoLot + " " + m.TenGiaoVien).Contains(Static_Helper.TeacherHelper.SearchString) || Static_Helper.TeacherHelper.SearchString == "")
                             select m
                           ).Include(m => m.bomon);
            InitViewBag(false);
            return View(giaoviens.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Khoa, String BoMon, String SearchString)
        {
            Static_Helper.TeacherHelper.Khoa = Khoa;
            Static_Helper.TeacherHelper.SearchString = SearchString;
            Static_Helper.TeacherHelper.BoMon = BoMon;
            var giaoviens = (from m in db.giaoviens
                             where ((BoMon == "" && m.bomon.KhoaQL.Equals(Khoa)) || (BoMon != "" && m.bomon.MaBoMon.Equals(BoMon))) && ((m.HoLot + " " + m.TenGiaoVien).Contains(SearchString) || SearchString == "")
                             select m
                           ).Include(m => m.bomon);
            InitViewBag(true);
            return View(giaoviens.ToList());
        }

        private void InitViewBag(Boolean IsPost)
        {
            var KhoaQry = from d in db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");
                
            var BoMonQry = from b in db.bomons
                           where b.khoa.MaKhoa == (IsPost ? Static_Helper.TeacherHelper.Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                           select new { MaBoMon = b.MaBoMon, TenBoMon = b.TenBoMon };
            ViewBag.BoMon = new SelectList(BoMonQry.ToArray(), "MaBoMon", "TenBoMon");
            ViewBag.SearchString = "";
        }

        //
        // GET: /GiaoVien/Details/5

        public ViewResult Details(string id)
        {
            giaovien giaovien = db.giaoviens.Find(id);
            return View(giaovien);
        }

        //
        // GET: /GiaoVien/Create

        public ActionResult Create()
        {
            ViewBag.BoMonQL = new SelectList(db.bomons, "MaBoMon", "TenBoMon");
            return View();
        }

        //
        // POST: /GiaoVien/Create

        [HttpPost]
        public ActionResult Create(giaovien giaovien)
        {
            if (ModelState.IsValid)
            {
                db.giaoviens.Add(giaovien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BoMonQL = new SelectList(db.bomons, "MaBoMon", "TenBoMon", giaovien.BoMonQL);
            return View(giaovien);
        }

        //
        // GET: /GiaoVien/Edit/5

        public ActionResult Edit(string id)
        {
            giaovien giaovien = db.giaoviens.Find(id);
            ViewBag.BoMonQL = new SelectList(db.bomons, "MaBoMon", "TenBoMon", giaovien.BoMonQL);
            return View(giaovien);
        }

        //
        // POST: /GiaoVien/Edit/5

        [HttpPost]
        public ActionResult Edit(giaovien giaovien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(giaovien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BoMonQL = new SelectList(db.bomons, "MaBoMon", "TenBoMon", giaovien.BoMonQL);
            return View(giaovien);
        }

        //
        // GET: /GiaoVien/Delete/5
/*
        public ActionResult Delete(string id)
        {
            giaovien giaovien = db.giaoviens.Find(id);
            return View(giaovien);
        }

        //
        // POST: /GiaoVien/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            giaovien giaovien = db.giaoviens.Find(id);
            db.giaoviens.Remove(giaovien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */

        [HttpPost, ActionName("Delete")]
        public String DeleteConfirmed(string id)
        {
            try
            {
                giaovien giaovien = db.giaoviens.Find(id);
                db.giaoviens.Remove(giaovien);
                db.SaveChanges();
                return "Xoá thành công!";
            }
            catch (Exception e)
            {
                return "Xoá không được [" + e.Message + "]";
            }

        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}