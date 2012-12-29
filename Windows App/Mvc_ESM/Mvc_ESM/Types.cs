﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_ESM.Static_Helper
{
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

    public class Class
    {
        public String ClassID { get; set; }
        public int Group { get; set; }
        public Boolean IsIgnored { get; set; }
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
        public int MinStudent { get; set; }
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

    public class jQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }
    }

}