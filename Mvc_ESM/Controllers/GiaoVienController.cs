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
    public class GiaoVienController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /GiaoVien/
        [HttpGet]
        public ViewResult Index()
        {
            var giaoviens = (from m in db.giaoviens
                             where (m.bomon.KhoaQL.Equals(Static_Helper.GiaoVienHelper.Khoa) || Static_Helper.GiaoVienHelper.Khoa == "") && (m.HoLot + " " + m.TenGiaoVien).Contains(Static_Helper.GiaoVienHelper.SearchString)
                             select m
                           ).Include(m => m.bomon);
            InitViewBag();
            return View(giaoviens.ToList());
        }
        [HttpPost]
        public ViewResult Index(String Khoa, String SearchString)
        {
            Static_Helper.GiaoVienHelper.Khoa = Khoa;
            Static_Helper.GiaoVienHelper.SearchString = SearchString;
            var giaoviens = (from m in db.giaoviens
                           where (m.bomon.KhoaQL.Equals(Khoa) || Khoa == "") && (m.HoLot + " " + m.TenGiaoVien).Contains(Static_Helper.GiaoVienHelper.SearchString)
                           select m
                           ).Include(m => m.bomon);
            InitViewBag();
            return View(giaoviens.ToList());
        }


        private void InitViewBag()
        {
            var KhoaLst = new ArrayList();
            var KhoaQry = from d in db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            KhoaLst.AddRange(KhoaQry.ToArray());
            ViewBag.Khoa = new SelectList(KhoaLst, "MaKhoa", "TenKhoa");
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}