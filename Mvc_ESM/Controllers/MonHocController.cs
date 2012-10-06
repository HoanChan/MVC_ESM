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
    public class MonHocController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        [HttpGet]
        public JsonResult LoadBoMonsByKhoa(string KhoaID)
        {
            var Data =  from b in db.bomons where b.khoa.MaKhoa == KhoaID select new SelectListItem()
            {
                Text = b.TenBoMon,
                Value = b.MaBoMon,
            };

            return Json(Data, JsonRequestBehavior.AllowGet);
        } 

        //
        // GET: /MonHoc/
        [HttpGet]
        public ViewResult Index()
        {
            var monhocs = (from m in db.monhocs
                           where ((Static_Helper.MonHocHelper.BoMon == "" && m.bomon.KhoaQL.Equals(Static_Helper.MonHocHelper.Khoa)) || (Static_Helper.MonHocHelper.BoMon != "" && m.bomon.MaBoMon.Equals(Static_Helper.MonHocHelper.BoMon))) && (m.TenMonHoc.Contains(Static_Helper.MonHocHelper.SearchString) || Static_Helper.MonHocHelper.SearchString == "")
                           select m
                           ).Include(m => m.bomon).Include(m => m.khoa);
            InitViewBag(false);
            return View(monhocs.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Khoa, String BoMon, String SearchString)
        {
            Static_Helper.MonHocHelper.Khoa = Khoa;
            Static_Helper.MonHocHelper.SearchString = SearchString;
            Static_Helper.MonHocHelper.BoMon = BoMon;
            var monhocs = (from m in db.monhocs 
                           where ((BoMon == "" && m.bomon.KhoaQL.Equals(Khoa)) || (BoMon != "" && m.bomon.MaBoMon.Equals(BoMon))) && (m.TenMonHoc.Contains(SearchString) || SearchString == "")
                           select m
                           ).Include(m => m.bomon).Include(m => m.khoa);
            InitViewBag(true);
            return View(monhocs.ToList());
        }

        private void InitViewBag(Boolean IsPost)
        {
            var KhoaQry = from d in db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");
            var BoMonQry = from b in db.bomons
                            where b.khoa.MaKhoa == (IsPost ? Static_Helper.MonHocHelper.Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                            select new { MaBoMon = b.MaBoMon, TenBoMon = b.TenBoMon };
            ViewBag.BoMon = new SelectList(BoMonQry.ToArray(), "MaBoMon", "TenBoMon");
            ViewBag.SearchString = "";
        }

        //
        // GET: /MonHoc/Details/5

        public ViewResult Details(string id)
        {
            monhoc monhoc = db.monhocs.Find(id);
            return View(monhoc);
        }

        //
        // GET: /MonHoc/Create

        public ActionResult Create()
        {
            ViewBag.BoMonQL = new SelectList(db.bomons, "MaBoMon", "TenBoMon");
            ViewBag.KhoaXepLich = new SelectList(db.khoas, "MaKhoa", "TenKhoa");
            return View();
        } 

        //
        // POST: /MonHoc/Create

        [HttpPost]
        public ActionResult Create(monhoc monhoc)
        {
            if (ModelState.IsValid)
            {
                db.monhocs.Add(monhoc);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.BoMonQL = new SelectList(db.bomons, "MaBoMon", "TenBoMon", monhoc.BoMonQL);
            ViewBag.KhoaXepLich = new SelectList(db.khoas, "MaKhoa", "TenKhoa", monhoc.KhoaXepLich);
            return View(monhoc);
        }
        
        //
        // GET: /MonHoc/Edit/5
 
        public ActionResult Edit(string id)
        {
            monhoc monhoc = db.monhocs.Find(id);
            ViewBag.BoMonQL = new SelectList(db.bomons, "MaBoMon", "TenBoMon", monhoc.BoMonQL);
            ViewBag.KhoaXepLich = new SelectList(db.khoas, "MaKhoa", "TenKhoa", monhoc.KhoaXepLich);
            return View(monhoc);
        }

        //
        // POST: /MonHoc/Edit/5

        [HttpPost]
        public ActionResult Edit(monhoc monhoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monhoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BoMonQL = new SelectList(db.bomons, "MaBoMon", "TenBoMon", monhoc.BoMonQL);
            ViewBag.KhoaXepLich = new SelectList(db.khoas, "MaKhoa", "TenKhoa", monhoc.KhoaXepLich);
            return View(monhoc);
        }

        //
        // GET: /MonHoc/Delete/5
 
        public ActionResult Delete(string id)
        {
            monhoc monhoc = db.monhocs.Find(id);
            return View(monhoc);
        }

        //
        // POST: /MonHoc/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            monhoc monhoc = db.monhocs.Find(id);
            db.monhocs.Remove(monhoc);
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