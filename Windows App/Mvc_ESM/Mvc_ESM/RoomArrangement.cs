using Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mvc_ESM.Static_Helper
{
    class RoomArrangement
    {

        //- B1a: Lấy danh sách sinh viên sẽ thi mỗi môn, xếp theo ABC, lấy luôn số lượng
        //- B1b: Xếp danh sách môn trong 1 ca theo thứ tự sao cho khi xếp phòng số phòng được sử dụng là tối đa
        private static List<Shift> ShiftList;
        private static Dictionary<String, List<String>> StudentByGroup;
        private static Boolean[] Progressed;
        private static int RoomUsedIndex;
        private static int MaxContaint;

        public static void Run()
        {
            Init();
            Arrangement();
            AlgorithmRunner.SaveOBJ("GroupsTime", AlgorithmRunner.GroupsTime);
            AlgorithmRunner.SaveOBJ("GroupsRoom", AlgorithmRunner.GroupsRoom);
            AlgorithmRunner.SaveOBJ("GroupsRoomStudents", AlgorithmRunner.GroupsRoomStudents);
            UpdateShiftsAndRooms();
            AlgorithmRunner.SaveOBJ("AppShifts", InputHelper.Shifts);
            AlgorithmRunner.SaveOBJ("AppRooms", InputHelper.Rooms);
        }

        private static void UpdateShiftsAndRooms()
        {
            for (int Index = 0; Index < AlgorithmRunner.GroupsTime.Length; Index++)
            {
                int ShiftIndex = InputHelper.Shifts.FindIndex(m => m.Time == AlgorithmRunner.GroupsTime[Index]);
                InputHelper.Shifts[ShiftIndex].IsBusy = true;
                int RoomListIndex = InputHelper.Rooms.FindIndex(m => m.Time == AlgorithmRunner.GroupsTime[Index]);
                foreach (Room aRoom in AlgorithmRunner.GroupsRoom[Index])
                {
                    int RoomIndex = InputHelper.Rooms[RoomListIndex].Rooms.FindIndex(m => m.RoomID == aRoom.RoomID);
                    InputHelper.Rooms[RoomListIndex].Rooms[RoomIndex].IsBusy = true;
                }
            }

        }

        private static void MakeStudentList(int GroupIndex)
        {
            int StudentsNumber = StudentByGroup[AlgorithmRunner.Groups[GroupIndex]].Count;
            int RoomNumber = AlgorithmRunner.GroupsRoom[GroupIndex].Count;
            int StudentsPerRoom = StudentsNumber / RoomNumber;
            AlgorithmRunner.GroupsRoomStudents[GroupIndex] = new List<String>[RoomNumber];
            int Used = 0;
            int OverLoad = StudentsNumber - StudentsPerRoom * RoomNumber;
            for (int RoomIndex = 0; RoomIndex < RoomNumber; RoomIndex++)
            {
                int Use;
                if (StudentsPerRoom + OverLoad > AlgorithmRunner.GroupsRoom[GroupIndex][RoomIndex].Container)
                {
                    Use = AlgorithmRunner.GroupsRoom[GroupIndex][RoomIndex].Container;
                    OverLoad = (StudentsPerRoom + OverLoad) - Use;
                }
                else
                {
                    Use = StudentsPerRoom + OverLoad;
                    OverLoad = 0;
                }
                AlgorithmRunner.GroupsRoomStudents[GroupIndex][RoomIndex] = new List<String>();
                for (int StudentIndex = Used; StudentIndex < Used + Use; StudentIndex++)
                {
                    AlgorithmRunner.GroupsRoomStudents[GroupIndex][RoomIndex].Add(StudentByGroup[AlgorithmRunner.Groups[GroupIndex]][StudentIndex]);

                }
                Used += Use;
            }
        }

        public static int CalcShift(DateTime OldTime, DateTime NewTime)
        {
            int Shift1Index = InputHelper.Shifts.FindIndex(m => m.Time == OldTime);
            int Shift2Index = InputHelper.Shifts.FindIndex(m => m.Time == NewTime);
            return Math.Abs(Shift1Index - Shift2Index);
        }

        private static DateTime IncTime(DateTime Time, int Shift)
        {
            int CurrentShiftIndex = ShiftList.FindIndex(m => m.Time == Time);
            if (CurrentShiftIndex + Shift > ShiftList.Count - 1)
            {
                AlgorithmRunner.SaveOBJ("Status", "err Số ca thi không đủ, đang dừng lại ở ca thi thứ: " + CurrentShiftIndex + Shift);
                AlgorithmRunner.IsBusy = false;
                Thread.CurrentThread.Abort();
            }
            return ShiftList[CurrentShiftIndex + Shift].Time;
        }

        private static void SetDefaultTime()
        {
            AlgorithmRunner.GroupsTime = new DateTime[AlgorithmRunner.Groups.Count];
            AlgorithmRunner.MaxColorTime = new DateTime[AlgorithmRunner.ColorNumber];
            // ca thi
            int ShiftIndex = 0;
            for (int ColorNumber = 1; ColorNumber < AlgorithmRunner.ColorNumber; ColorNumber++)
            {
                // các môn cùng màu sẽ cùng ca, cùng ngày thi
                for (int GroupIndex = 0; GroupIndex < AlgorithmRunner.Groups.Count; GroupIndex++)
                {
                    if (AlgorithmRunner.Colors[GroupIndex] == ColorNumber)
                    {
                        AlgorithmRunner.GroupsTime[GroupIndex] = ShiftList[ShiftIndex].Time;
                        AlgorithmRunner.MaxColorTime[ColorNumber] = AlgorithmRunner.GroupsTime[GroupIndex];
                    }
                }
                ShiftIndex += InputHelper.Options.DateMin + 1;
            }
        }

        private static void GetStudentList()
        {
            StudentByGroup = new Dictionary<String, List<String>>();
            for (int GroupIndex = 0; GroupIndex < AlgorithmRunner.Groups.Count; GroupIndex++)
            {
                String SubjectID = AlgorithmRunner.GetSubjectID(AlgorithmRunner.Groups[GroupIndex]);
                String ClassList = AlgorithmRunner.GetClassList(AlgorithmRunner.Groups[GroupIndex]);
                String IgnoreStudents = InputHelper.IgnoreStudents.ContainsKey(SubjectID) ? JsonConvert.SerializeObject(InputHelper.IgnoreStudents[SubjectID]) : "[]";
                IgnoreStudents = IgnoreStudents.Substring(1, IgnoreStudents.Length - 2).Replace("\"", "'");
                IEnumerable<String> Result = InputHelper.db.Database.SqlQuery<String>("select sinhvien.MaSinhVien from pdkmh, sinhvien "
                                                                            + "where pdkmh.MaSinhVien = sinhvien.MaSinhVien "
                                                                            + "and MaMonHoc = '" + SubjectID + "' "
                                                                            + (IgnoreStudents.Length > 0 ? "and not(sinhvien.MaSinhVien in (" + IgnoreStudents + ")) " : "")
                                                                            + "and pdkmh.Nhom in(" + ClassList + ") "
                                                                            + "order by (Ten + Ho)");
                StudentByGroup.Add(AlgorithmRunner.Groups[GroupIndex], Result.ToList<String>());
            }
            AlgorithmRunner.GroupsRoom = new List<Room>[AlgorithmRunner.Groups.Count];
            AlgorithmRunner.GroupsRoomStudents = new List<String>[AlgorithmRunner.Groups.Count][];
        }
        
        private static void Init()
        {
            ShiftList = InputHelper.Shifts.Where(m => !m.IsBusy).ToList();
            Progressed = new Boolean[AlgorithmRunner.Groups.Count];
            MaxContaint = InputHelper.Rooms.Max(m => m.Rooms.Where(w => !w.IsBusy).Sum(s => s.Container));
            SetDefaultTime();
            GetStudentList();
            
            //InputHelper.Rooms = InputHelper.Rooms.OrderBy(r => r.Container).ToList<Room>();
        }

        // Chia phòng cho từng môn cùng ca
        private static void Arrangement()
        {
            List<int> GroupIndexList;
            GroupIndexList = GetNextShiftGroups();//.ToList();
            //int x = SubjectsIndexList.Count();
            while (GroupIndexList.Count() > 0)
            {
                var RoomList = InputHelper.Rooms.Find(m => m.Time.Date == AlgorithmRunner.GroupsTime[GroupIndexList[0]].Date).Rooms.Where(r => !r.IsBusy).ToList();
                RoomUsedIndex = -1; // gán bằng -1 để vào xếp phòng nó tăng lên xét coi còn phòng ko
                foreach (int si in GroupIndexList)
                {
                    RoomArrangementForOneGroup(si, RoomList);
                }
                GroupIndexList = GetNextShiftGroups();
            }
        }
        //- B3: Tăng thời gian của tất cả các môn có màu khác và thi sau lên 1 khoảng sao cho > max[màu các môn vừa tăng thời gian]
        private static void RoomArrangementForOneGroup(int GroupIndex, List<Room> RoomList)
        {
            int StudentsNumber = StudentByGroup[AlgorithmRunner.Groups[GroupIndex]].Count;
            if (StudentsNumber > MaxContaint)
            {
                AlgorithmRunner.SaveOBJ("Status", "err Phòng thi không đủ, đang dừng lại ở nhóm thi: " + AlgorithmRunner.Groups[GroupIndex] + " với số sinh viên là:" + StudentsNumber);
                AlgorithmRunner.IsBusy = false;
                Thread.CurrentThread.Abort();
            }
            AlgorithmRunner.GroupsRoom[GroupIndex] = new List<Room>();
            int OldRoomUsedIndex = RoomUsedIndex;
            while (StudentsNumber > 0)
            {
                RoomUsedIndex++;
                if (RoomUsedIndex < RoomList.Count) // còn phòng
                {                    
                    
                    AlgorithmRunner.GroupsRoom[GroupIndex].Add(RoomList[RoomUsedIndex]);
                    StudentsNumber -= RoomList[RoomUsedIndex].Container;
                    
                    // đáng lẽ code phân trực tiếp sv vào phòng ở đây, nhưng như vậy phòng ít phòng nhiều
                    // để đó sau này truy vấn lại môn này thi mấy phòng rồi chia sau!
                }
                else // hết phòng == > cần giãn ca
                {
                    AlgorithmRunner.GroupsRoom[GroupIndex].Clear(); // xoá mấy phòng lỡ thêm vào
                    Progressed[GroupIndex] = false; // cho nó trở lại trạng thái chưa xử lý
                    RoomUsedIndex = OldRoomUsedIndex;
                    // chuyển môn hiện tại qua ca tiếp theo
                    AlgorithmRunner.GroupsTime[GroupIndex] = IncTime(AlgorithmRunner.GroupsTime[GroupIndex], 1);
                    int CurrentColor = AlgorithmRunner.Colors[GroupIndex];
                    // Kiểm tra và tăng max
                    if (AlgorithmRunner.MaxColorTime[CurrentColor] < AlgorithmRunner.GroupsTime[GroupIndex])
                    {
                        // đổi max
                        AlgorithmRunner.MaxColorTime[CurrentColor] = AlgorithmRunner.GroupsTime[GroupIndex];
                        // chuyển các môn màu khác ở ca phía sau đi ra sau 1 ca, tránh tình trạng khác màu mà cùng ca
                        IncSubjectAfter(CurrentColor);
                    }
                    return; // thoát luôn
                }
            }
            MakeStudentList(GroupIndex);
        }
        // chuyển các môn màu khác ở ca phía sau đi ra sau 1 ca, tránh tình trạng khác màu mà cùng ca
        private static void IncSubjectAfter(int CurrentColor)
        {
            if (CurrentColor < AlgorithmRunner.ColorNumber)
            {
                for (int si = 0; si < AlgorithmRunner.GroupsTime.Count(); si++)
                {
                    if (AlgorithmRunner.Colors[si] > CurrentColor)
                    {
                        AlgorithmRunner.GroupsTime[si] = IncTime(AlgorithmRunner.GroupsTime[si], 1);
                        if (AlgorithmRunner.MaxColorTime[AlgorithmRunner.Colors[si]] < AlgorithmRunner.GroupsTime[si])
                        {
                            // đổi max
                            AlgorithmRunner.MaxColorTime[AlgorithmRunner.Colors[si]] = AlgorithmRunner.GroupsTime[si];
                        }
                    }
                }
            }
        }

        private static List<int> GetNextShiftGroups()
        {
            List<int> Result = new List<int>();
            for (int i = 0; i < AlgorithmRunner.Groups.Count; i++)
            {
                if (!Progressed[i])
                {
                    DateTime ShiftTime = AlgorithmRunner.GroupsTime[i];
                    for (int si = 0; si < AlgorithmRunner.Groups.Count; si++)
                    {
                        if (!Progressed[si] && AlgorithmRunner.GroupsTime[si] == ShiftTime)
                        {
                            Progressed[si] = true; // xử lý nó rồi
                            Result.Add(si);
                        }
                    }
                    return Result;
                }
            }
            return Result;
        }
    }
}
