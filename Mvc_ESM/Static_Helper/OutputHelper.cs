using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace Mvc_ESM.Static_Helper
{
    public class OutputHelper
    {
        public static String WinAppExe = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Win_App"), "Mvc_ESM.exe");
        public static String RealPath(String Name)
        {
            return Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Win_App"), Name + ".jso");
        }
        public static void SaveOBJ(String Name, Object OBJ)
        {
            System.IO.File.WriteAllText(
                RealPath( Name ),
                JsonConvert.SerializeObject(OBJ, Formatting.Indented),
                Encoding.UTF8
            );            
        }
        public static void DeleteOBJ(String Name)
        {
            System.IO.File.Delete(
                RealPath(Name)
            );
        }

        public static String DayOffWeekVN(DateTime aDate)
        {
            switch (aDate.DayOfWeek.ToString())
            {
                case "Sunday": return "Chủ Nhật";
                case "Monday": return "Thứ Hai";
                case "Tuesday": return "Thứ Ba";
                case "Wednesday": return "Thứ Tư";
                case "Thursday": return "Thứ Năm";
                case "Friday": return "Thứ Sáu";
                case "Saturday": return "Thứ Bảy";
            }
            return "xx";
        }


        public static String SaveIgnoreGroups(List<String> SubjectID, List<String> Class, List<String> Check)
        {
            string paramInfo = "";
            for (int i = 0; i < SubjectID.Count; i++)
            {
                String aKey = SubjectID[i] + "_" + Class[i];
                if (Data.Groups.ContainsKey(aKey))
                {
                    Data.Groups[aKey].IsIgnored = (Check[i] == "checked"); // Check[i] != "undefined"
                }
                paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Check: " + Check[i] + "<br />";
            }
            OutputHelper.SaveOBJ("Groups", Data.Groups);
            return paramInfo;
        }

        public static String SaveGroups(List<String> SubjectID, List<String> Class, List<int> Group)
        {
            string paramInfo = "";
            for (int i = 0; i < SubjectID.Count; i++)
            {
                String aKey = SubjectID[i] + "_" + Class[i];
                if (Data.Groups.ContainsKey(aKey)) // bo cung dc
                {
                    Data.Groups[aKey].GroupID = Group[i];
                }

                paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Group: " + Group[i] + "<br />";
            }
            //var Groups = new List<String>(); // Mamonhoc_nhom1_nhom2_nhom3...
            //SubjectID = SubjectID.Distinct().ToList();
            //for (int Index = 0; Index < SubjectID.Count(); Index++)
            //{
            //    List<String> data = Data.Groups.Where(g=>g.Key.Contains(SubjectID[Index]))
            //                                    .Select(g=>g.Value.GroupID.ToString())
            //                                    .ToList();
            //    Boolean[] Progressed = new Boolean[data.Count()];
            //    for (int i = 0; i < data.Count(); i++)
            //    {
            //        if (!Progressed[i])
            //        {
            //            String GroupItem = SubjectID[Index];
            //            for (int j = 0; j < data.Count(); j++)
            //            {
            //                if (data[i] == data[j])
            //                {
            //                    Progressed[j] = true;
            //                    GroupItem += "_" + data[j];
            //                }
            //            }
            //            if (!Groups.Contains(GroupItem))
            //            {
            //                Groups.Add(GroupItem);
            //            }
            //        }
            //    }
            //}
            OutputHelper.SaveOBJ("Groups", Data.Groups);
            return paramInfo;
        }
    }
}