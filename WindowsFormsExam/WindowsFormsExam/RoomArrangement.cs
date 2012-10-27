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
        public static Hashtable StudentBySubject;
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
        }

        private static void MakeStudentList(int SubjectIndex, int StudentsNumber)
        {
            int RoomNumber = AlgorithmRunner.SubjectRoom[SubjectIndex].Count;
            int StudentsPerRoom = StudentsNumber / RoomNumber;
            AlgorithmRunner.SubjectRoomStudents[SubjectIndex] = new List<string>[RoomNumber];
            int Used = 0;
            int OverLoad = 0;
            for (int i = 0; i < RoomNumber; i++)
            {
                int Use;
                if (StudentsPerRoom + OverLoad > AlgorithmRunner.SubjectRoom[SubjectIndex][i].Container)
                {
                    Use = AlgorithmRunner.SubjectRoom[SubjectIndex][i].Container;
                    OverLoad = (StudentsPerRoom + OverLoad) - Use;
                }
                else
                {
                    Use = StudentsPerRoom + OverLoad;
                    OverLoad = 0;
                }
                for (int j = Used; j < Used + Use; j++)
                {
                    AlgorithmRunner.SubjectRoomStudents[SubjectIndex][i].Add(((List<Student>)StudentBySubject[InputHelper.Subjects[SubjectIndex]])[j].MSSV);

                }
                Used += Use;
            }
        }

        private static void Init()
        {
            StudentBySubject = new Hashtable();
            DKMHEntities db = new DKMHEntities();
            for (int i = 0; i < InputHelper.Subjects.Count; i++)
            {
                var Students = from dk in db.pdkmhs
                               where dk.MaMonHoc == InputHelper.Subjects[i]
                                    && !((List<String>)InputHelper.Students[InputHelper.Subjects[i]]).Contains(dk.MaSinhVien)
                               select new Student()
                               {
                                   MSSV = dk.sinhvien.MaSinhVien,
                                   Ho = dk.sinhvien.Ho,
                                   Ten = dk.sinhvien.Ten
                               };
                StudentBySubject.Add(InputHelper.Subjects[i], Students.OrderBy(s => s.Ten + s.Ho).ToList());
                //Sort(InputHelper.Subjects[i]);
            }
            AlgorithmRunner.SubjectRoom = new List<Room>[InputHelper.Subjects.Count];
            AlgorithmRunner.SubjectRoomStudents = new List<String>[InputHelper.Subjects.Count][];
            InputHelper.Rooms = InputHelper.Rooms.OrderBy(r => r.Container).ToList<Room>();
        }

        // Chia phòng cho từng môn cùng ca
        private static void step2()
        {
            IEnumerable<int> SubjectsIndexList = GetNextStepSubjects();
            while (SubjectsIndexList.Count() > 0)
            {
                RoomUsedIndex = -1; // gán bằng -1 để vào xếp phòng nó tăng lên xét coi còn phòng ko
                foreach (int si in SubjectsIndexList)
                {
                    RoomArrangementForOneSubject(si);
                }
                SubjectsIndexList = GetNextStepSubjects();
            }
        }
        //- B3: Tăng thời gian của tất cả các môn có màu khác và thi sau lên 1 khoảng sao cho > max[màu các môn vừa tăng thời gian]
        private static void RoomArrangementForOneSubject(int SubjectIndex)
        {
            int StudentsNumber = ((List<String>)StudentBySubject[InputHelper.Subjects[SubjectIndex]]).Count;
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
            MakeStudentList(SubjectIndex, StudentsNumber);
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

        private static IEnumerable<int> GetNextStepSubjects()
        {
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
                            yield return si;
                        }
                    }
                }
            }
        }

        private static void Sort(String SubjectID)
        {
            //List<Student> Student = (List<Student>)InputHelper.Student[SubjectID];
            //for (int i = 0; i < Student.Count - 1; i++)
            //{
            //    for (int j = i + 1; j < Student.Count; j++)
            //    {
            //        if (Student[i].Ten.CompareTo(Student[j].Ten) > 0)
            //        {
            //            Student temp = Student[i];
            //            Student[i] = Student[j];
            //            Student[j] = temp;
            //        }
            //        else
            //        {
            //            if (Student[i].Ten.CompareTo(Student[j].Ten) == 0 && Student[i].Ho.CompareTo(Student[j].Ho) > 0)
            //            {
            //                Student temp = Student[i];
            //                Student[i] = Student[j];
            //                Student[j] = temp;
            //            }
            //        }
            //    }
            //}
            //InputHelper.Student[SubjectID] = Student;
            //InputHelper.Students[SubjectID] = ((List<Student>)InputHelper.Students[SubjectID]).OrderBy(s => s.Ten + s.Ho).ToList<Student>();
        }

    }
}
