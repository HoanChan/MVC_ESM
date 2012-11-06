using System;
using System.Collections.Generic;
using System.Collections;
using Mvc_ESM.Models;

namespace Mvc_ESM.Static_Helper
{
    
    public class InputHelper
    {
        /// <summary>
        /// danh sách môn học sẽ xếp lịch
        /// </summary>
        public static List<String> Subjects;
        public static List<Room> Rooms;
        /// <summary>
        /// danh sách sinh viên sẽ bị cấm thi
        /// </summary>
        public static Hashtable Students;

        public static Options Options = new Options();
    }
}