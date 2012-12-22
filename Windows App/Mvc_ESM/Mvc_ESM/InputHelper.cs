using System;
using System.Collections.Generic;
using System.Collections;
using Model;
using System.IO;
using System.Text;
using System.Linq;

namespace Mvc_ESM.Static_Helper
{
    
    public class InputHelper
    {
        /// <summary>
        /// danh sách môn học sẽ xếp lịch
        /// </summary>
        public static Dictionary<String, List<Class>> Subjects;
        public static List<Room> Rooms = InitListRoom();
        /// <summary>
        /// danh sách sinh viên sẽ bị cấm thi
        /// </summary>
        public static Dictionary<String, List<String>> Students;

        public static Options Options = InitOptions();

        public static List<Shift> BusyShifts = InitBusyShift();

        public static List<RoomList> BusyRooms = InitListBusyRooms();

        public static List<Room> InitListRoom()
        {
            if (AlgorithmRunner.JsoExits("Rooms"))
            {
                return AlgorithmRunner.ReadOBJ<List<Room>>("Rooms");
            }
            else
            {
                DKMHEntities db = new DKMHEntities();
                List<Room> Rooms = db.phongs.Where(p => p.SucChua > 0).OrderBy(m => m.MaPhong).Select(m => new Room()
                {
                    RoomID = m.MaPhong,
                    Container = (int)m.SucChua,
                    IsBusy = false
                }).ToList();
                return Rooms;
            }
        }

        public static List<RoomList> InitListBusyRooms()
        {
            if (AlgorithmRunner.JsoExits("BusyRooms"))
            {
                return AlgorithmRunner.ReadOBJ<List<RoomList>>("BusyRooms");
            }
            else
            {
                DKMHEntities db = new DKMHEntities();
                List<RoomList> aRoomList = new List<RoomList>();
                //List<Room> Rooms = db.phongs.Where(p => p.SucChua > 0).OrderBy(m => m.MaPhong).Select(m => new Room()
                //{
                //    RoomID = m.MaPhong,
                //    Container = (int)m.SucChua,
                //    IsBusy = false
                //}).ToList();
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
            if (AlgorithmRunner.JsoExits("BusyShift"))
            {
                var Result = AlgorithmRunner.ReadOBJ<List<Shift>>("BusyShift");
                if (Result.Count == Options.NumDate * Options.Times.Count)
                    return Result;
            }
            
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
            AlgorithmRunner.SaveOBJ("BusyShift", aShift);
            return aShift;
        }

        public static Options InitOptions()
        {
            if (AlgorithmRunner.JsoExits("Options"))
            {
                return AlgorithmRunner.ReadOBJ<Options>("Options");
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