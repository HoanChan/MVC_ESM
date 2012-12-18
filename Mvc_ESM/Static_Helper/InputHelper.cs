using System;
using System.Collections.Generic;
using System.Collections;
using Model;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Mvc_ESM.Static_Helper
{
    
    public class InputHelper
    {
        /// <summary>
        /// danh sách môn học sẽ xếp lịch
        /// </summary>
        public static Dictionary<String, List<Class>> Subjects = new Dictionary<String, List<Class>>();
        public static List<Room> Rooms;
        /// <summary>
        /// danh sách sinh viên sẽ bị cấm thi
        /// </summary>
        public static Dictionary<String, List<String>> Students;
        
        public static Options Options = InitOptions();

        public static List<Shift> BusyShifts = InitBusyShift();

        public static List<RoomList> BusyRooms = InitListRoom();

        public static List<RoomList> InitListRoom()
        {
            String Path = OutputHelper.RealPath("BusyRooms");
            if (File.Exists(Path))
            {
                return JsonConvert.DeserializeObject<List<RoomList>>(File.ReadAllText(Path));
            }
            else
            {
                List<RoomList> aRoomList = new List<RoomList>();
                List<Room> Rooms = Data.db.phongs.Where(p => p.SucChua > 0).Select(m => new Room()
                                                                                                {
                                                                                                    RoomID = m.MaPhong,
                                                                                                    Container = (int)m.SucChua,
                                                                                                    IsBusy = false
                                                                                                }).ToList();
                for (int i = 0; i < InputHelper.Options.NumDate; i++)
                {
                    DateTime ShiftTime = InputHelper.Options.StartDate.AddDays(i);
                    aRoomList.Add(new RoomList() { Rooms = Rooms, Time = ShiftTime });
                }
                return aRoomList;
            }
        }

        public static List<Shift> InitBusyShift()
        {
            String Path = OutputHelper.RealPath("BusyShift");
            if (File.Exists(Path))
            {
                return JsonConvert.DeserializeObject<List<Shift>>(File.ReadAllText(Path));
            }
            else
            {
                List<Shift> aShift = new List<Shift>();
                for (int i = 0; i < InputHelper.Options.NumDate; i++)
                {
                    for (int j = 0; j < InputHelper.Options.Times.Count; j++)
                    {
                        DateTime ShiftTime = InputHelper.Options.StartDate.AddDays(i)
                                                                      .AddHours(InputHelper.Options.Times[j].Hour)
                                                                      .AddMinutes(InputHelper.Options.Times[j].Minute);
                        aShift.Add(new Shift() { IsBusy = (ShiftTime.DayOfWeek == DayOfWeek.Sunday), Time = ShiftTime });
                    }
                }
                return aShift;
            }
        }

        public static Options InitOptions()
        {
            String OptionsPath = OutputHelper.RealPath("Options");
            if (File.Exists(OptionsPath))
            {
                return JsonConvert.DeserializeObject<Options>(File.ReadAllText(OptionsPath));
            }
            else
            {
                return new Options()
                {
                    StartDate = DateTime.Now.Date,
                    NumDate = 21,
                    DateMin = 1,
                    StepTime = 120,
                    Times = new List<DateTime>()
                    {
                        DateTime.Now.Date.AddHours(7).AddMinutes(15),
                        DateTime.Now.Date.AddHours(9).AddMinutes(30),
                        DateTime.Now.Date.AddHours(13),
                        DateTime.Now.Date.AddHours(15).AddMinutes(15)
                    }

                };
            }
        }

        public static String SaveIgnoreGroups(List<String> SubjectID, List<String> Class, List<String> Check)
        {
            string paramInfo = "";
            for (int i = 0; i < SubjectID.Count; i++)
            {
                String aKey = SubjectID[i] + "_" + Class[i];
                if (Data.Groups.ContainsKey(aKey))
                {
                    Data.Groups[aKey].IsIgnored = (Check[i] == "checked"); // Check[i] != "undefined"
                }
                paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Check: " + Check[i] + "<br />";
            }
            OutputHelper.SaveOBJ("Groups", Data.Groups);
            return paramInfo;
        }

        public static String SaveGroups(List<String> SubjectID, List<String> Class, List<int> Group)
        {
            string paramInfo = "";
            for (int i = 0; i < SubjectID.Count; i++)
            {
                String aKey = SubjectID[i] + "_" + Class[i];
                if (Data.Groups.ContainsKey(aKey)) // bo cung dc
                {
                    Data.Groups[aKey].GroupID = Group[i];
                }

                paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Group: " + Group[i] + "<br />";
            }
            var Groups = new List<String>(); // Mamonhoc_nhom1_nhom2_nhom3...
            SubjectID = SubjectID.Distinct().ToList();
            for (int Index = 0; Index < SubjectID.Count(); Index++)
            {
                List<String> data = Data.Groups.Where(g=>g.Key.Contains(SubjectID[Index]))
                                                .Select(g=>g.Value.GroupID.ToString())
                                                .ToList();
                Boolean[] Progressed = new Boolean[data.Count()];
                for (int i = 0; i < data.Count(); i++)
                {
                    if (!Progressed[i])
                    {
                        String GroupItem = SubjectID[Index];
                        for (int j = 0; j < data.Count(); j++)
                        {
                            if (data[i] == data[j])
                            {
                                Progressed[j] = true;
                                GroupItem += "_" + data[j];
                            }
                        }
                        if (!Groups.Contains(GroupItem))
                        {
                            Groups.Add(GroupItem);
                        }
                    }
                }
            }
            OutputHelper.SaveOBJ("Groups", Groups);
            return paramInfo;
        }
    }
}