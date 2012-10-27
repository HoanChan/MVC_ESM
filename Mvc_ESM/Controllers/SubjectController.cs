using Mvc_ESM.Models;
using Mvc_ESM.Static_Helper;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{ 
    public class SubjectController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /MonHoc/
        [HttpGet]
        public ViewResult Index()
        {
            if (SubjectHelper.Khoa == null || SubjectHelper.Khoa == "")
            {
                SubjectHelper.Khoa = (from d in db.khoas
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
            var monhocs = (from m in db.monhocs
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
                            where b.khoa.MaKhoa == (IsPost ? SubjectHelper.Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                            orderby b.TenBoMon
                            select new { MaBoMon = b.MaBoMon, TenBoMon = b.TenBoMon };
            ViewBag.BoMon = new SelectList(BoMonQry.ToArray(), "MaBoMon", "TenBoMon");
            ViewBag.SearchString = "";
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
        // POST: /MonHoc/Delete/5

        [HttpPost, ActionName("Delete")]
        public String DeleteConfirmed(string id)
        {
            try
            {
                monhoc monhoc = db.monhocs.Find(id);
                db.monhocs.Remove(monhoc);
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