using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Models;
using Mvc_ESM.Static_Helper;

namespace Mvc_ESM.Controllers
{
    public class StudentController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        [HttpGet]
        public JsonResult LoadClassByFacultyID(string FacultyID)
        {
            var Data = from b in db.lops
                       where b.khoi.KhoaQL == FacultyID
                       select new SelectListItem()
                       {
                           Text = b.MaLop,
                           Value = b.MaLop,
                       };
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Student/
        [HttpGet]
        public ViewResult Index()
        {
            StudentHelper.Khoa = (from d in db.khoas
                                                orderby d.TenKhoa
                                                select d).FirstOrDefault().MaKhoa;
            StudentHelper.SearchString = "";
            StudentHelper.Lop = "Tất cả";
            var students = (from m in db.sinhviens
                            where m.lop1.khoi.KhoaQL.Equals(StudentHelper.Khoa)
                            select m
                           ).Include(m => m.lop1);
            InitViewBag(false);
            return View(students.ToList());
        }

        [HttpPost]
        public ViewResult Index(String Khoa, String Lop, String SearchString)
        {
            StudentHelper.Khoa = Khoa;
            StudentHelper.SearchString = SearchString;
            StudentHelper.Lop = Lop;
            var sinhviens = (from m in db.sinhviens
                             where (Lop == "" && m.lop1.khoi.KhoaQL.Equals(Khoa)) || (Lop != "" && m.lop1.MaLop.Equals(Lop)) && (m.Ten.Contains(SearchString) || SearchString == "")
                             select m
                           ).Include(m=>m.lop1);
            InitViewBag(true);
            return View(sinhviens.ToList());
        }

        private void InitViewBag(Boolean IsPost)
        {
            var KhoaQry = from d in db.khoas
                          orderby d.TenKhoa
                          select new { MaKhoa = d.MaKhoa, TenKhoa = d.TenKhoa };
            ViewBag.Khoa = new SelectList(KhoaQry.ToArray(), "MaKhoa", "TenKhoa");
            var ClassQry = from b in db.lops
                           where b.khoi.KhoaQL == (IsPost ? StudentHelper.Khoa : KhoaQry.FirstOrDefault().MaKhoa)
                           orderby b.MaLop
                           select new { MaLop = b.MaLop, TenLop = b.MaLop };
            ViewBag.Lop = new SelectList(ClassQry.ToArray(), "MaLop", "TenLop");
            ViewBag.SearchString = "";
        }

        //
        // GET: /Student/Details/5

        public ViewResult Details(string id)
        {
            sinhvien sinhvien = db.sinhviens.Find(id);
            return View(sinhvien);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            ViewBag.ChuyenNganh = new SelectList(db.chuyennganhs, "MaChuyenNganh", "TenChuyenNganh");
            ViewBag.Lop = new SelectList(db.lops, "MaLop", "CVHT");
            return View();
        }

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(sinhvien sinhvien)
        {
            if (ModelState.IsValid)
            {
                db.sinhviens.Add(sinhvien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChuyenNganh = new SelectList(db.chuyennganhs, "MaChuyenNganh", "TenChuyenNganh", sinhvien.ChuyenNganh);
            ViewBag.Lop = new SelectList(db.lops, "MaLop", "CVHT", sinhvien.Lop);
            return View(sinhvien);
        }

        //
        // GET: /Student/Edit/5

        public ActionResult Edit(string id)
        {
            sinhvien sv = db.sinhviens.Find(id);
            ViewBag.Khoa = (from d in db.khoas
                            orderby d.TenKhoa
                            select d).FirstOrDefault().MaKhoa;
            ViewBag.ChuyenNganh = new SelectList(db.chuyennganhs, "MaChuyenNganh", "TenChuyenNganh", sv.ChuyenNganh);
            ViewBag.Lop = new SelectList(db.lops, "MaLop", "MaLop", sv.Lop);
            return View(sv);
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        public ActionResult Edit(sinhvien sinhvien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sinhvien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChuyenNganh = new SelectList(db.chuyennganhs, "MaChuyenNganh", "TenChuyenNganh", sinhvien.ChuyenNganh);
            ViewBag.Lop = new SelectList(db.lops, "MaLop", "CVHT", sinhvien.Lop);
            return View(sinhvien);
        }

        //
        // GET: /Student/Delete/5
        /*
        public ActionResult Delete(string id)
        {
            sinhvien sinhvien = db.sinhviens.Find(id);
            return View(sinhvien);
        }

        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            sinhvien sinhvien = db.sinhviens.Find(id);
            db.sinhviens.Remove(sinhvien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        [HttpPost, ActionName("Delete")]
        public String DeleteConfirmed(string id)
        {
            try
            {
                sinhvien sinhvien = db.sinhviens.Find(id);
                db.sinhviens.Remove(sinhvien);
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