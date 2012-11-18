using System;
using System.Collections.Generic;
using System.Collections;
using Model;
using System.IO;
using System.Text;

namespace Mvc_ESM.Static_Helper
{
    
    public class InputHelper
    {
        /// <summary>
        /// danh sách môn học sẽ xếp lịch
        /// </summary>
        public static Dictionary<String, List<Class>> Subjects;
        public static List<Room> Rooms;
        /// <summary>
        /// danh sách sinh viên sẽ bị cấm thi
        /// </summary>
        public static Dictionary<String, List<String>> Students;

        public static Options Options = new Options();

        //public static void SaveOBJ(String Name, Object OBJ)
        //{
        //    System.IO.File.WriteAllText(
        //        Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Win_App"), Name + ".jso"), 
        //        fastJSON.JSON.Instance.ToJSON(OBJ), 
        //        Encoding.UTF8
        //    );
        //}
    }
}