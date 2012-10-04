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
    public class GiaoVienController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /GiaoVien/

        public ViewResult Index()
        {
            var giaoviens = db.giaoviens.Include(g => g.bomon);
            return View(giaoviens.ToList());
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