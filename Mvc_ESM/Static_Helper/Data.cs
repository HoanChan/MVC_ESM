using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mvc_ESM.Static_Helper
{
    public class Data
    {
        public static DKMHEntities db = new DKMHEntities();
        public class Group
        {
            public string MaMonHoc { get; set; }
            public string TenMonHoc { get; set; }
            public string TenBoMon { get; set; }
            public string TenKhoa { get; set; }
            public byte Nhom { get; set; }
            public Nullable<int> SoLuongDK { get; set; }
            public int GroupID { get; set; }
            public Boolean IsIgnored { get; set; }
        }
        public static Dictionary<String, Group> Groups = initGroups();
        public static Dictionary<String, Group> initGroups()
        {
            String GroupFile = OutputHelper.RealPath("Groups");
            Dictionary<String, Group> aGroups = File.Exists(GroupFile) ?
                                                JsonConvert.DeserializeObject < Dictionary<String, Group> >(File.ReadAllText(GroupFile)):
                                                (from m in db.monhocs
                                                 join d in db.pdkmhs on m.MaMonHoc equals d.MaMonHoc
                                                 select new Group()
                                                   {
                                                       MaMonHoc = m.MaMonHoc,
                                                       TenMonHoc = m.TenMonHoc,
                                                       TenBoMon = m.bomon.TenBoMon,
                                                       TenKhoa = m.bomon.khoa.TenKhoa,
                                                       Nhom = d.Nhom,
                                                       SoLuongDK = d.nhom1.SoLuongDK,
                                                       GroupID = 1,
                                                       IsIgnored = false
                                                   })
                                                   .Distinct()
                                                   .ToDictionary(k => (k.MaMonHoc + "_" + k.Nhom), k => k);
            return aGroups;
        }
    }
}