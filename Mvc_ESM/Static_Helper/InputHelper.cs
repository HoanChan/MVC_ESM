using System;
using System.Collections.Generic;

using Mvc_ESM.Models;

namespace Mvc_ESM.Static_Helper
{
    public class IgoreSV
    {
        public String MaMonHoc { get; set; }
        public String MaSinhVien { get; set; }
        public IgoreSV(String mh, String sv)
        {
            MaMonHoc = mh;
            MaSinhVien = sv;
        }
    }

    public class InputHelper
    {
        /// <summary>
        /// danh sách môn học sẽ xếp lịch
        /// </summary>
        public static List<String> Subjects;
        /// <summary>
        /// danh sách sinh viên sẽ bị cấm thi
        /// </summary>
        public static List<String> Student;
        public static List<IgoreSV> IgoreStudent;
    }
}