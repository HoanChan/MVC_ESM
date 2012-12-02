using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_ESM.Static_Helper
{
    public class Data
    {
        public static DKMHEntities db = new DKMHEntities();
        public class Subject
        {
            public string MaMonHoc { get; set; }
            public string TenMonHoc { get; set; }
            public string TenBoMon { get; set; }
            public string TenKhoa { get; set; }
            public byte Nhom { get; set; }
            public Nullable<int> SoLuongDK { get; set; }
        }
        public static IQueryable<Subject> Subjects = (from m in db.monhocs
                                                     join d in db.pdkmhs on m.MaMonHoc equals d.MaMonHoc
                                                     select new Subject(){ MaMonHoc=                                                                m.MaMonHoc,
                                                                TenMonHoc = m.TenMonHoc,
                                                                TenBoMon = m.bomon.TenBoMon,
                                                                TenKhoa = m.bomon.khoa.TenKhoa,
                                                                Nhom = d.Nhom,
                                                                SoLuongDK = d.nhom1.SoLuongDK
                                                     }).Distinct();
    }
}