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
    }
}