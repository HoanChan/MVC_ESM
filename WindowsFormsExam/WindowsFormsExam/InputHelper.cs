﻿using System;
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
    public class HastableStudent
    {
        public List<String> Subjects;
        public List<String> Students;
    }
    public class InputHelper
    {
        /// <summary>
        /// danh sách môn học sẽ xếp lịch
        /// </summary>
        public static List<String> Subjects;


        public static List<String> Rooms;

        /// <summary>
        /// danh sách sinh viên sẽ bị cấm thi
        /// </summary>
        public static Hashtable Student;

        public static DateTime StartDate;
        public static int NumDate;
        public static int DateMin;
        public static List<ExamTime> Times;
    }
}