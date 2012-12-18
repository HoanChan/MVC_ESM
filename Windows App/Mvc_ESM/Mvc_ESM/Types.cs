using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_ESM.Static_Helper
{
    public class Class
    {
        public String ClassID { get; set; }
        public int Group { get; set; }
    }


    public class Room
    {
        public String RoomID { get; set; }
        public int Container { get; set; }
        public Boolean IsBusy { get; set; }
    }

    public class RoomList
    {
        public DateTime Time { get; set; }
        public List<Room> Rooms { get; set; }
    }

    public class Shift
    {
        public DateTime Time { get; set; }
        public Boolean IsBusy { get; set; }
    }

    public class Options
    {
        public DateTime StartDate { get; set; }
        public int NumDate { get; set; }
        public int DateMin { get; set; }
        public int ShiftTime { get; set; }
        public List<DateTime> Times { get; set; }
    }

    public partial class Event
    {
        public String id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public String text { get; set; }
        public String details { get; set; }
        public int Step { get; set; }
        public String MaPhong { get; set; }
    }
}