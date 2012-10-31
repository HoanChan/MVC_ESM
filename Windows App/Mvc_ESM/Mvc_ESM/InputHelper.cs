using System;
using System.Collections.Generic;
using System.Collections;

namespace Mvc_ESM.Static_Helper
{
    public class ExamTime
    {
        public String Name { get; set; }
        public DateTime BGTime { get; set; }
        public DateTime ETime { get; set; }
    }

    public class Room
    {
        public String RoomID { get; set; }
        public int Container { get; set; }
    }

    public class Options
    {
        public DateTime StartDate { get; set; }
        public int NumDate { get; set; }
        public int DateMin { get; set; }
        public List<ExamTime> Times { get; set; }
    }

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
        public static Dictionary<String, List<String>> Students;

        public static Options Options = new Options();
    }
}