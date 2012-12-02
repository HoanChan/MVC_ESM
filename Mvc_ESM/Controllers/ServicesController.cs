using Model;
using Mvc_ESM.Static_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc_ESM.Controllers
{
    public class ServicesController : Controller
    {
        public class jQueryDataTableParamModel
        {
            /// <summary>
            /// Request sequence number sent by DataTable,
            /// same value must be returned in response
            /// </summary>       
            public string sEcho { get; set; }

            /// <summary>
            /// Text used for filtering
            /// </summary>
            public string sSearch { get; set; }

            /// <summary>
            /// Number of records that should be shown in table
            /// </summary>
            public int iDisplayLength { get; set; }

            /// <summary>
            /// First record that should be shown(used for paging)
            /// </summary>
            public int iDisplayStart { get; set; }

            /// <summary>
            /// Number of columns in table
            /// </summary>
            public int iColumns { get; set; }

            /// <summary>
            /// Number of columns that are used in sorting
            /// </summary>
            public int iSortingCols { get; set; }

            /// <summary>
            /// Comma separated list of column names
            /// </summary>
            public string sColumns { get; set; }

            public List<string> Class { get; set; }

            public List<int> Group { get; set; }

            public List<string> SubjectID { get; set; }
        }

        public static String SaveGroups(List<String> SubjectID, List<String> Class, List<int> Group)
        {
            InputHelper.Subjects = new Dictionary<String, List<Class>>();
            string paramInfo = "";
            for (int i = 0; i < SubjectID.Count; i++)
            {
                Class aClass = new Class() { ClassID = Class[i], Group = Group[i] };
                if (InputHelper.Subjects.ContainsKey(SubjectID[i]))
                {
                    if (!InputHelper.Subjects[SubjectID[i]].Contains(aClass))
                    {
                        InputHelper.Subjects[SubjectID[i]].Add(aClass);
                    }
                }
                else
                {
                    InputHelper.Subjects.Add(SubjectID[i], new List<Class>() { aClass });
                }
                paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Group: " + Group[i] + "<br /><br />";
            }
            OutputHelper.SaveOBJ("Subjects", InputHelper.Subjects);
            List<String> Groups = new List<String>();
            foreach (String Subject in InputHelper.Subjects.Keys)
            {
                Boolean[] Progressed = new Boolean[InputHelper.Subjects[Subject].Count];
                for (int i = 0; i < InputHelper.Subjects[Subject].Count; i++)
                {
                    if (!Progressed[i])
                    {
                        String GroupItem = Subject;
                        for (int j = 0; j < InputHelper.Subjects[Subject].Count; j++)
                        {
                            if (InputHelper.Subjects[Subject][i].Group == InputHelper.Subjects[Subject][j].Group)
                            {
                                Progressed[j] = true;
                                GroupItem += "_" + InputHelper.Subjects[Subject][j].ClassID;
                            }
                        }
                        if (!Groups.Contains(GroupItem))
                        {
                            Groups.Add(GroupItem);
                        }
                    }
                }
            }
            OutputHelper.SaveOBJ("Groups", Groups);
            return paramInfo;
        }

        [HttpPost]
        public JsonResult DataTable_SelectSubjects(jQueryDataTableParamModel param)
        {

            var Subjects = (from m in Data.Subjects
                            where param.sSearch == null || param.sSearch == "" || m.MaMonHoc.Contains(param.sSearch) || m.TenMonHoc.Contains(param.sSearch)
                            select m
                           ).OrderBy(m=>m.TenMonHoc);
            var Result = new List<string[]>();

            foreach (var su in Subjects.Skip(param.iDisplayStart).Take(param.iDisplayLength))
            {
                Result.Add(new string[] {
                                            su.MaMonHoc,
                                            su.TenMonHoc,
                                            su.TenBoMon,
                                            su.TenKhoa,
                                            su.Nhom.ToString(),
                                            su.SoLuongDK.ToString(),
                                            "1",
                                            "Xoá"
                                        }
                            );
            }
            if (param.SubjectID != null && param.Class != null && param.Group != null)
            {
                SaveGroups(param.SubjectID, param.Class, param.Group);
            }
            return Json(new{
                                sEcho = param.sEcho,
                                iTotalRecords = Data.Subjects.Count(),
                                iTotalDisplayRecords = Subjects.Count(),
                                //iTotalDisplayedRecords = Subjects.Count(),
                                aaData = Result
                            },
                            JsonRequestBehavior.AllowGet
                        );
        }

        [HttpGet]
        public JsonResult GetProgressInfo()
        {
            return Json(new List<object>(){ new 
                    {
                        pbCreateMatrix = ProgressHelper.pbCreateMatrix,
                        CreateMatrixInfo = ProgressHelper.CreateMatrixInfo
                    }}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectByGroupInfo(string SubjectID)
        {
            var Groups = from g in Data.db.nhoms
                          where g.MaMonHoc == SubjectID
                          select new
                          {
                              MSMH = g.MaMonHoc,
                              TenMH = g.monhoc.TenMonHoc,
                              BoMon = g.monhoc.bomon.TenBoMon,
                              Khoa = g.monhoc.bomon.khoa.TenKhoa,
                              Nhom = g.Nhom1,
                              SL = g.SoLuongDK
                          };
            if (Groups.Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            MSMH = "false"
                        }}, JsonRequestBehavior.AllowGet);
            }

            return Json(Groups, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadStudentAndSubjectInfo(string StudentID, string SubjectID)
        {
            var Student = from s in Data.db.sinhviens
                          where s.MaSinhVien.Equals(StudentID)
                          select new
                          {
                              MSSV = s.MaSinhVien,
                              Ho = s.Ho,
                              Ten = s.Ten,
                              Lop = s.Lop,
                              Khoa = s.lop1.khoi.khoa.TenKhoa
                          };
            var Subject = from m in Data.db.monhocs
                          where m.MaMonHoc.Equals(SubjectID)
                          select new
                          {
                              MSMH = m.MaMonHoc,
                              TenMH = m.TenMonHoc,
                              BoMon = m.bomon.TenBoMon
                          };
            if (Student.Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            OK = "",
                            MSMH = "",
                            TenMH = "",
                            BoMon = "",
                            MSSV = "false",
                            Ho = "",
                            Ten = "",
                            Lop = "",
                            Khoa = ""
                        }}, JsonRequestBehavior.AllowGet);
            }

            if (Subject.Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            OK = "",
                            MSMH = "false",
                            TenMH = "",
                            BoMon = "",
                            MSSV = "",
                            Ho = "",
                            Ten = "",
                            Lop = "",
                            Khoa = ""
                        }}, JsonRequestBehavior.AllowGet);
            }

            if ((from d in Data.db.pdkmhs where d.MaMonHoc == SubjectID && d.MaSinhVien == StudentID select d).Count() == 0)
            {
                return Json(new List<object>(){ new 
                        {
                            OK = "false",
                            MSMH = "",
                            TenMH = Subject.FirstOrDefault().TenMH,
                            BoMon = "",
                            MSSV = "",
                            Ho = Student.FirstOrDefault().Ho,
                            Ten = Student.FirstOrDefault().Ten,
                            Lop = "",
                            Khoa = ""
                        }}, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<object>(){ new 
                    {
                        OK = "",
                        MSMH = Subject.FirstOrDefault().MSMH,
                        TenMH = Subject.FirstOrDefault().TenMH,
                        BoMon = Subject.FirstOrDefault().BoMon,
                        MSSV = Student.FirstOrDefault().MSSV,
                        Ho = Student.FirstOrDefault().Ho,
                        Ten = Student.FirstOrDefault().Ten,
                        Lop = Student.FirstOrDefault().Lop,
                        Khoa = Student.FirstOrDefault().Khoa
                    }}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadClassByFacultyName(string FacultyName)
        {
            var aData = from b in Data.db.lops
                       where b.khoi.khoa.TenKhoa == FacultyName || FacultyName == ""
                       select new SelectListItem()
                       {
                           Text = b.MaLop,
                           Value = b.MaLop,
                       };

            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadClassByFacultyID(string FacultyID)
        {
            var aData = from b in Data.db.lops
                       where b.khoi.khoa.MaKhoa == FacultyID
                       select new SelectListItem()
                       {
                           Text = b.MaLop,
                           Value = b.MaLop,
                       };

            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsByFacultyID(string FacultyID)
        {
            var aData = from b in Data.db.bomons
                       where b.khoa.MaKhoa == FacultyID
                       select new SelectListItem()
                       {
                           Text = b.TenBoMon,
                           Value = b.MaBoMon,
                       };

            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsByFacultyName(string FacultyName)
        {
            var aData = (from b in Data.db.bomons
                        where b.khoa.TenKhoa.Replace("&", "và") == FacultyName || b.khoa.TenKhoa == FacultyName || FacultyName == ""
                        select new SelectListItem()
                        {
                            Text = b.TenBoMon,
                            Value = b.MaBoMon,
                        }).Distinct();

            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectsBySubject(string SubjectID)
        {
            var aData = (from m in Data.db.monhocs
                        where m.BoMonQL == SubjectID
                        select new SelectListItem()
                         {
                             Text = m.TenMonHoc,
                             Value = m.MaMonHoc,
                         });
            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadSubjectByFacultyID(string FacultyID)
        {
            var aData = (from m in Data.db.monhocs
                        where m.bomon.KhoaQL == FacultyID
                        select new SelectListItem()
                        {
                            Text = m.TenMonHoc,
                            Value = m.MaMonHoc,
                        });
            return Json(aData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadRoomsBySubjectID(string SubjectID)
        {
            var aData = (from b in Data.db.This
                       where b.MaMonHoc == SubjectID
                       select new SelectListItem()
                       {
                           Text = b.MaPhong,
                           Value = b.MaPhong,
                       }).Distinct();
            return Json(aData, JsonRequestBehavior.AllowGet);
        }
    }
}
