﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_ESM.Static_Helper
{
    public class Calendar
    {
        public static String DataFormater(List<Event> SubjectTime, Boolean Ok)
        {
            String Result = "<data>";
            for (int i = 0; i < SubjectTime.Count(); i++)
            {
                String mp = Ok ? SubjectTime[i].MaPhong : "";
                Result += "<event id=\"" + SubjectTime[i].id + "\">"
                        + "<start_date><![CDATA[" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", SubjectTime[i].start_date) + "]]></start_date>"
                        + "<end_date><![CDATA[" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", SubjectTime[i].end_date) + "]]></end_date>"
                        + "<text><![CDATA[" + SubjectTime[i].text + (mp.Length > 0 ? " - " + mp : "") + "]]></text>"
                        + "</event>";
            }
            Result += "</data>";
            return Result;
        }
    }
}