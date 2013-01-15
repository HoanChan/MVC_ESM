using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace Mvc_ESM.Static_Helper
{
    public class OutputHelper
    {
        public static String WinAppExe = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Win_App"), "Mvc_ESM.exe");
        public static String RealPath(String Name)
        {
            return Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Win_App"), Name + ".jso");
        }
        public static void SaveOBJ(String Name, Object OBJ)
        {
            System.IO.File.WriteAllText(
                RealPath( Name ),
                JsonConvert.SerializeObject(OBJ, Formatting.Indented),
                Encoding.UTF8
            );            
        }
        public static void DeleteOBJ(String Name)
        {
            System.IO.File.Delete(
                RealPath(Name)
            );
        }

        public static String DayOffWeekVN(DateTime aDate)
        {
            switch (aDate.DayOfWeek.ToString())
            {
                case "Sunday": return "Chủ Nhật";
                case "Monday": return "Thứ Hai";
                case "Tuesday": return "Thứ Ba";
                case "Wednesday": return "Thứ Tư";
                case "Thursday": return "Thứ Năm";
                case "Friday": return "Thứ Sáu";
                case "Saturday": return "Thứ Bảy";
            }
            return "xx";
        }

        public static String SaveRooms(long DateMilisecond, int Shift, List<String> RoomID, List<int> Container, List<String> Check)
        {
            DateTime realDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(DateMilisecond).Date + InputHelper.Options.Times[Shift].TimeOfDay;
            for (int RoomIndex = 0; RoomIndex < InputHelper.BusyRooms.Count; RoomIndex++)
            {
                if (InputHelper.BusyRooms[RoomIndex].Time == realDate)
                {
                    string paramInfo = realDate.ToString() + "<br />";
                    InputHelper.BusyRooms[RoomIndex].Rooms.Clear();
                    for (int Index = 0; Index < RoomID.Count; Index++)
                    {
                        InputHelper.BusyRooms[RoomIndex].Rooms.Add(new Room() { RoomID = RoomID[Index], Container = Container[Index], IsBusy = (Check[Index] == "checked") });
                        paramInfo += "MH:" + RoomID[RoomIndex] + " Container: " + Container[RoomIndex] + " Check: " + Check[RoomIndex] + "<br />";
                    }
                    OutputHelper.SaveOBJ("Rooms", InputHelper.BusyRooms);
                    return paramInfo;
                }
            }
            return "";
        }

        public static String SaveIgnoreGroups(List<String> SubjectID, List<String> Class, List<String> Check, Boolean IsFinal = false)
        {
            string paramInfo = "";
            Dictionary<String, Group> IgnoreGroups = (Dictionary<String, Group>)(HttpContext.Current.Session["IgnoreGroups"] ?? InputHelper.Groups);

            for (int i = 0; i < SubjectID.Count; i++)
            {
                String aKey = SubjectID[i] + "_" + Class[i];
                if (IgnoreGroups.ContainsKey(aKey))
                {
                    IgnoreGroups[aKey].IsIgnored = (Check[i] == "checked"); // Check[i] != "undefined"
                }
                paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Check: " + Check[i] + "<br />";
            }
            if (IsFinal)
            {
                InputHelper.Groups = IgnoreGroups;
                OutputHelper.SaveOBJ("Groups", InputHelper.Groups);
            }
            else
            {
                HttpContext.Current.Session["IgnoreGroups"] = IgnoreGroups;
            }
            return paramInfo;
        }

        public static String SaveGroups(List<String> SubjectID, List<String> Class, List<int> Group, Boolean IsFinal = false)
        {
            string paramInfo = "";
            Dictionary<String, Group> Groups = (Dictionary<String, Group>)(HttpContext.Current.Session["IgnoreGroups"] ?? InputHelper.Groups);
            for (int i = 0; i < SubjectID.Count; i++)
            {
                String aKey = SubjectID[i] + "_" + Class[i];
                if (Groups.ContainsKey(aKey)) // bo cung dc
                {
                    Groups[aKey].GroupID = Group[i];
                }

                paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Group: " + Group[i] + "<br />";
            }
             if (IsFinal)
             {
                 InputHelper.Groups = Groups;
                 OutputHelper.SaveOBJ("Groups", InputHelper.Groups); 
             }
             else
             {
                 HttpContext.Current.Session["IgnoreGroups"] = Groups;
             }
            return paramInfo;
        }
    }
}