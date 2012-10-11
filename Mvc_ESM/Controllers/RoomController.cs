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
    public class RoomController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /Room/
        [HttpGet]
        public ViewResult Index()
        {
            var phongs = (from r in db.phongs
                          where (Static_Helper.RoomHelper.MaPhong == "" || (Static_Helper.RoomHelper.MaPhong != "" && r.MaPhong.Equals(Static_Helper.RoomHelper.MaPhong)))
                          select r);
            InitViewBag();
            return View(phongs.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Phong)
        {
            Static_Helper.RoomHelper.MaPhong = Phong;
            var phongs = (from r in db.phongs
                          where (Phong == "" || (Phong != "" && r.MaPhong.Equals(Phong)))
                          select r);
            InitViewBag();
            return View(phongs.ToList());
        }

        private void InitViewBag()
        {
            var RoomQry =from r in db.phongs
                          select new { MaPhong = r.MaPhong, TenPhong = r.MaPhong };
            ViewBag.Phong = new SelectList(RoomQry.ToArray(), "MaPhong", "TenPhong");
        }

        //
        // GET: /Room/Details/5

        public ViewResult Details(string id)
        {
            phong phong = db.phongs.Find(id);
            return View(phong);
        }

        //
        // GET: /Room/Create

        public ActionResult Create()
        {
            ViewBag.KhoaQL = new SelectList(db.khoas, "MaKhoa", "TenKhoa");
            return View();
        }

        //
        // POST: /Room/Create

        [HttpPost]
        public ActionResult Create(phong phong)
        {
            if (ModelState.IsValid)
            {
                db.phongs.Add(phong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhoaQL = new SelectList(db.khoas, "MaKhoa", "TenKhoa", phong.KhoaQL);
            return View(phong);
        }

        //
        // GET: /Room/Edit/5

        public ActionResult Edit(string id)
        {
            phong phong = db.phongs.Find(id);
            ViewBag.KhoaQL = new SelectList(db.khoas, "MaKhoa", "TenKhoa", phong.KhoaQL);
            return View(phong);
        }

        //
        // POST: /Room/Edit/5

        [HttpPost]
        public ActionResult Edit(phong phong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoaQL = new SelectList(db.khoas, "MaKhoa", "TenKhoa", phong.KhoaQL);
            return View(phong);
        }

        //
        // GET: /Room/Delete/5
        /*
        public ActionResult Delete(string id)
        {
            phong phong = db.phongs.Find(id);
            return View(phong);
        }*/

        //
        // POST: /Room/Delete/5

        [HttpPost, ActionName("Delete")]
        public String DeleteConfirmed(string id)
        {

            try
            {
                phong phong = db.phongs.Find(id);
                db.phongs.Remove(phong);
                db.SaveChanges();
                InitViewBag();
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