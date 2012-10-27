using System;
using System.Collections.Generic;
using System.Collections;
using Mvc_ESM.Models;

namespace Mvc_ESM.Static_Helper
{
    public class ExamTime
    {
        public String Name;
        public DateTime BGTime;
        public DateTime ETime;
    }

    public class Room
    {
        public String RoomID;
        public int Container;
        public Room(String RoomID, int Container)
        {
            this.RoomID = RoomID;
            this.Container = Container;
        }
    }

    public class Options
    {
        public DateTime StartDate;
        public int NumDate;
        public int DateMin;
        public List<ExamTime> Times = new List<ExamTime>();
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
        public static Hashtable Students;

        public static Options Options = new Options();
    }
}