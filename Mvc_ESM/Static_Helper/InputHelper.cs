using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mvc_ESM.Static_Helper
{
    
    public class InputHelper
    {
        public static DKMHEntities db = new DKMHEntities();
        /// <summary>
        /// danh sách sinh viên sẽ bị cấm thi
        /// </summary>
        public static Dictionary<String, List<String>> IgnoreStudents = InitIgnoreStudents();

        public static Options Options = InitOptions();

        public static Dictionary<String, Group> Groups = InitGroups();

        public static List<Shift> Shifts = InitShift();

        public static List<RoomList> BusyRooms = InitRooms();

        public static Dictionary<String, Group> InitGroups()
        {
            String GroupFile = OutputHelper.RealPath("Groups");
            Dictionary<String, Group> aGroups = File.Exists(GroupFile) ?
                                                JsonConvert.DeserializeObject<Dictionary<String, Group>>(File.ReadAllText(GroupFile)) :
                                                (from m in db.monhocs
                                                 join d in db.pdkmhs on m.MaMonHoc equals d.MaMonHoc
                                                 select new Group()
                                                 {
                                                     MaMonHoc = m.MaMonHoc,
                                                     TenMonHoc = m.TenMonHoc,
                                                     TenBoMon = m.bomon.TenBoMon,
                                                     TenKhoa = m.bomon.khoa.TenKhoa,
                                                     Nhom = d.Nhom,
                                                     SoLuongDK = d.nhom1.SoLuongDK,
                                                     GroupID = 1,
                                                     IsIgnored = false
                                                 })
                                                   .Distinct()
                                                   .ToDictionary(k => (k.MaMonHoc + "_" + k.Nhom), k => k);
            return aGroups;
        }

        public static List<RoomList> InitRooms()
        {
            String Path = OutputHelper.RealPath("Rooms");
            if (File.Exists(Path))
            {
                return JsonConvert.DeserializeObject<List<RoomList>>(File.ReadAllText(Path));
            }
            else
            {
                List<RoomList> aRoomList = new List<RoomList>();
                List<Room> Rooms = db.phongs.Where(p => p.SucChua > 0).Select(m => new Room()
                                                                                                {
                                                                                                    RoomID = m.MaPhong,
                                                                                                    Container = (int)m.SucChua,
                                                                                                    IsBusy = false
                                                                                                }).ToList();
                for (int i = 0; i < Options.NumDate; i++)
                {
                    DateTime ShiftTime = Options.StartDate.AddDays(i);
                    for (int j = 0; j < Options.Times.Count; j++)
                    {
                        aRoomList.Add(new RoomList() { Rooms = new List<Room>(Rooms), Time = ShiftTime + Options.Times[j].TimeOfDay });
                    }
                }
                return aRoomList;
            }
        }

        public static List<Shift> InitShift()
        {
            String Path = OutputHelper.RealPath("Shift");
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

        public static Dictionary<String, List<String>> InitIgnoreStudents()
        {
            String Path = OutputHelper.RealPath("IgnoreStudents");
            if (File.Exists(Path))
            {
                return JsonConvert.DeserializeObject<Dictionary<String, List<String>>>(File.ReadAllText(Path));
            }
            else
            {
                return new Dictionary<String, List<String>>();
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
                    NumDate = 100,
                    DateMin = 1,
                    ShiftTime = 120,
                    MinStudent = 10,
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

    }
}