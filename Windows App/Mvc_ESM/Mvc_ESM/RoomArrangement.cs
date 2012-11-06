using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.Entity;
using Model;
namespace Mvc_ESM.Static_Helper
{
    class RoomArrangement
    {

        //- B1a: Lấy danh sách sinh viên sẽ thi mỗi môn, xếp theo ABC, lấy luôn số lượng
        //- B1b: Xếp danh sách môn trong 1 ca theo thứ tự sao cho khi xếp phòng số phòng được sử dụng là tối đa
        private static Dictionary<String, List<Student>> StudentBySubject;
        private static Boolean[] Progressed = new Boolean[InputHelper.Subjects.Count];
        private static int RoomUsedIndex;
        private class Student
        {
            public String MSSV;
            public String Ho;
            public String Ten;
        }

        public static void Run()
        {
            Init();
            step2();
            AlgorithmRunner.WriteObj("SubjectTime", AlgorithmRunner.SubjectTime);
            AlgorithmRunner.WriteObj("SubjectRoom", AlgorithmRunner.SubjectRoom);
            AlgorithmRunner.WriteObj("SubjectRoomStudents", AlgorithmRunner.SubjectRoomStudents);
        }

        private static void MakeStudentList(int SubjectIndex)
        {
            int StudentsNumber = StudentBySubject[InputHelper.Subjects[SubjectIndex]].Count;
            int RoomNumber = AlgorithmRunner.SubjectRoom[SubjectIndex].Count;
            int StudentsPerRoom = StudentsNumber / RoomNumber;
            AlgorithmRunner.SubjectRoomStudents[SubjectIndex] = new List<string>[RoomNumber];
            int Used = 0;
            int OverLoad = StudentsNumber - StudentsPerRoom * RoomNumber;
            for (int RoomIndex = 0; RoomIndex < RoomNumber; RoomIndex++)
            {
                int Use;
                if (StudentsPerRoom + OverLoad > AlgorithmRunner.SubjectRoom[SubjectIndex][RoomIndex].Container)
                {
                    Use = AlgorithmRunner.SubjectRoom[SubjectIndex][RoomIndex].Container;
                    OverLoad = (StudentsPerRoom + OverLoad) - Use;
                }
                else
                {
                    Use = StudentsPerRoom + OverLoad;
                    OverLoad = 0;
                }
                AlgorithmRunner.SubjectRoomStudents[SubjectIndex][RoomIndex] = new List<string>();
                for (int StudentIndex = Used; StudentIndex < Used + Use; StudentIndex++)
                {
                    AlgorithmRunner.SubjectRoomStudents[SubjectIndex][RoomIndex].Add(StudentBySubject[InputHelper.Subjects[SubjectIndex]][StudentIndex].MSSV);

                }
                Used += Use;
            }
        }

        private static void Init()
        {
            StudentBySubject = new Dictionary<string,List<Student>>();
            DKMHEntities db = new DKMHEntities();
            for (int SubjectIndex = 0; SubjectIndex < InputHelper.Subjects.Count; SubjectIndex++)
            {
                String SubjectID = InputHelper.Subjects[SubjectIndex];
                var Students = (from dk in db.pdkmhs
                                where dk.MaMonHoc == SubjectID
                                    //&& !InputHelper.Students[SubjectID].Contains(dk.MaSinhVien)
                               select new Student()
                               {
                                   MSSV = dk.sinhvien.MaSinhVien,
                                   Ho = dk.sinhvien.Ho,
                                   Ten = dk.sinhvien.Ten
                               }).OrderBy(s => s.Ten + s.Ho).ToList();
                //if (Students.Count == 0)
                //{
                //    int x = 1;
                //}
                StudentBySubject.Add(InputHelper.Subjects[SubjectIndex], Students);
                //Sort(InputHelper.Subjects[i]);
            }
            AlgorithmRunner.SubjectRoom = new List<Room>[InputHelper.Subjects.Count];
            AlgorithmRunner.SubjectRoomStudents = new List<String>[InputHelper.Subjects.Count][];
            InputHelper.Rooms = InputHelper.Rooms.OrderBy(r => r.Container).ToList<Room>();
        }

        // Chia phòng cho từng môn cùng ca
        private static void step2()
        {
            List<int> SubjectsIndexList;
            SubjectsIndexList = GetNextStepSubjects();//.ToList();
            //int x = SubjectsIndexList.Count();
            while (SubjectsIndexList.Count() > 0)
            {
                RoomUsedIndex = -1; // gán bằng -1 để vào xếp phòng nó tăng lên xét coi còn phòng ko
                foreach (int si in SubjectsIndexList)
                {
                    RoomArrangementForOneSubject(si);
                }
                SubjectsIndexList = GetNextStepSubjects().ToList();
            }
        }
        //- B3: Tăng thời gian của tất cả các môn có màu khác và thi sau lên 1 khoảng sao cho > max[màu các môn vừa tăng thời gian]
        private static void RoomArrangementForOneSubject(int SubjectIndex)
        {
            int StudentsNumber = StudentBySubject[InputHelper.Subjects[SubjectIndex]].Count;
            AlgorithmRunner.SubjectRoom[SubjectIndex] = new List<Room>();
            int OldRoomUsedIndex = RoomUsedIndex;
            while (StudentsNumber > 0)
            {
                RoomUsedIndex++;
                if (RoomUsedIndex < InputHelper.Rooms.Count) // còn phòng
                {
                    AlgorithmRunner.SubjectRoom[SubjectIndex].Add(InputHelper.Rooms[RoomUsedIndex]);
                    StudentsNumber -= InputHelper.Rooms[RoomUsedIndex].Container;
                    // đáng lẽ code phân trực tiếp sv vào phòng ở đây, nhưng như vậy phòng ít phòng nhiều
                    // để đó sau này truy vấn lại môn này thi mấy phòng rồi chia sau!
                }
                else // hết phòng == > cần giãn ca
                {
                    AlgorithmRunner.SubjectRoom[SubjectIndex].Clear(); // xoá mấy phòng lỡ thêm vào
                    Progressed[SubjectIndex] = false; // cho nó trở lại trạng thái chưa xử lý
                    RoomUsedIndex = OldRoomUsedIndex;
                    // chuyển môn hiện tại qua ca tiếp theo
                    AlgorithmRunner.SubjectTime[SubjectIndex] = MakeTime.IncTime(AlgorithmRunner.SubjectTime[SubjectIndex], 1);
                    int CurrentColor = AlgorithmRunner.Colors[SubjectIndex];
                    // Kiểm tra và tăng max
                    if (AlgorithmRunner.MaxColorTime[CurrentColor] < AlgorithmRunner.SubjectTime[SubjectIndex])
                    {
                        // đổi max
                        AlgorithmRunner.MaxColorTime[CurrentColor] = AlgorithmRunner.SubjectTime[SubjectIndex];
                        // chuyển các môn màu khác ở ca phía sau đi ra sau 1 ca, tránh tình trạng khác màu mà cùng ca
                        IncSubjectAfter(CurrentColor);
                    }
                    return; // thoát luôn
                }
            }
            MakeStudentList(SubjectIndex);
        }

        // chuyển các môn màu khác ở ca phía sau đi ra sau 1 ca, tránh tình trạng khác màu mà cùng ca
        private static void IncSubjectAfter(int CurrentColor)
        {
            if (CurrentColor < AlgorithmRunner.ColorNumber)
            {
                for (int si = 0; si < AlgorithmRunner.SubjectTime.Count(); si++)
                {
                    if (AlgorithmRunner.Colors[si] > CurrentColor)
                    {
                        AlgorithmRunner.SubjectTime[si] = MakeTime.IncTime(AlgorithmRunner.SubjectTime[si], 1);
                        if (AlgorithmRunner.MaxColorTime[AlgorithmRunner.Colors[si]] < AlgorithmRunner.SubjectTime[si])
                        {
                            // đổi max
                            AlgorithmRunner.MaxColorTime[AlgorithmRunner.Colors[si]] = AlgorithmRunner.SubjectTime[si];
                        }
                    }
                }
            }
        }

        private static List<int> GetNextStepSubjects()
        {
            List<int> Result = new List<int>();
            for (int i = 0; i < InputHelper.Subjects.Count; i++)
            {
                if (!Progressed[i])
                {
                    DateTime StepTime = AlgorithmRunner.SubjectTime[i];
                    for (int si = 0; si < InputHelper.Subjects.Count; si++)
                    {
                        if (!Progressed[si] && AlgorithmRunner.SubjectTime[si] == StepTime)
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
