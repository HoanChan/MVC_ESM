using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    public partial class Event
    {
        public String id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public String text { get; set; }
        public String details { get; set; }
        public int Step { get; set; }
    }
}