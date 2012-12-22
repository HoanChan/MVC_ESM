using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace Mvc_ESM.Static_Helper
{
    public class MakeTime
    {
        private static List<Shift> ShiftList = InputHelper.BusyShifts.Where(m => !m.IsBusy).ToList();
        //B1: Gán thời gian tối thiểu cho tất cả các môn dựa vào màu của chúng
        private static void Init()
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
                ShiftIndex+= InputHelper.Options.DateMin + 1;
            }
        }

        //- B5: Tăng thời gian của tất cả các môn có màu khác và thi sau môn M lên 1 khoảng sao cho > max[màu môn M]
        private static void IncSubjectAfter(int CurrentColor)
        {
            //màu max rồi thì hết cái để tăng.
            if (CurrentColor < AlgorithmRunner.ColorNumber - 1)
            {
                // Thời gian các môn thi sau mà cùng màu thì bằng nhau và bằng max[màu],
                // nên lây min[màu tiếp theo] - max[màu hiện tại]
                // ta sẽ đươc Shift
                // Ví dụ: thời gian môn cần tăng thuộc ca 1, max[màu môn cần tăng] = 2
                // sau đó tăng lên 3 ca thành thời gian môn cần tăng = 4, max[màu môn cần tăng] = 4
                // max[màu tiếp theo] = 3 cần tăng 2
                DateTime MinNextColorTime = AlgorithmRunner.MaxColorTime[CurrentColor + 1];

                for (int si = 0; si < AlgorithmRunner.GroupsTime.Count(); si++)
                {
                    if (AlgorithmRunner.Colors[si] >= CurrentColor)
                    {
                        if (MinNextColorTime > AlgorithmRunner.GroupsTime[si])
                        {
                            // đổi min
                            MinNextColorTime = AlgorithmRunner.GroupsTime[si];
                        }
                    }
                }

                // min đã lớn hơn rồi thì thôi !
                if (MinNextColorTime > AlgorithmRunner.MaxColorTime[CurrentColor])
                {
                    return;
                }

                int Shift = CalcShift(MinNextColorTime, AlgorithmRunner.MaxColorTime[CurrentColor]) + 1;
                // tăng thời gian các môn thi sau, màu lớn hơn thì thi sau.
                for (int si = 0; si < AlgorithmRunner.GroupsTime.Count(); si++)
                {
                    if (AlgorithmRunner.Colors[si] > CurrentColor)
                    {
                        AlgorithmRunner.GroupsTime[si] = IncTime(AlgorithmRunner.GroupsTime[si], Shift);
                        if (AlgorithmRunner.MaxColorTime[AlgorithmRunner.Colors[si]] < AlgorithmRunner.GroupsTime[si])
                        {
                            // đổi max
                            AlgorithmRunner.MaxColorTime[AlgorithmRunner.Colors[si]] = AlgorithmRunner.GroupsTime[si];
                        }
                    }
                }
                
            }
        }
        //- B3: tăng thời gian của môn thi sau (Môn M) trong 2 môn đó lên 1 khoảng là X để thoã điều kiện.
        //- B4: nếu thời gian thi của môn M sau khi được tăng > max[màu của môn M] thì 
        //            gán max[màu của môn M] = thời gian thi của môn M rồi qua bước 5
        private static void IncSubjects(int i, int j)
        {

            int Index;
            int Shift = InputHelper.Options.DateMin + 1;
            //màu thằng nào lớn hơn thì giãn thằng đó
            if (AlgorithmRunner.Colors[i] > AlgorithmRunner.Colors[j])
            {
                // tăng môn i nhưng phải dựa vào môn j là do ko bít hiện tại khoảng cách giữa 2 môn
                // i và j là bao nhiêu, tăng cho vừa khớp với điều kiện cho chắc.
                AlgorithmRunner.GroupsTime[i] = IncTime(AlgorithmRunner.GroupsTime[j], Shift);
                Index = i;
            }
            else 
            {
                AlgorithmRunner.GroupsTime[j] = IncTime(AlgorithmRunner.GroupsTime[i], Shift);
                Index = j;
            }
            
            int CurrentColor = AlgorithmRunner.Colors[Index];
            // Kiểm tra và tăng max
            if (AlgorithmRunner.MaxColorTime[CurrentColor] < AlgorithmRunner.GroupsTime[Index])
            {
                // đổi max
                AlgorithmRunner.MaxColorTime[CurrentColor] = AlgorithmRunner.GroupsTime[Index];
                IncSubjectAfter(CurrentColor);
            }
            //else
            //{
            //    // không vượt quá max thì chỉ tăng mình nó mấy môn khác sẽ không bị ảnh hưởng.
            //}
            
        }

        //- B2: tìm sinh viên có 2 môn thi chưa thoã điều kiện là cách nhau n buổi
        //+ Bước này không duyệt theo sinh viên mà sử dụng ma trận kề để tìm 2 môn có sinh viên thi cùng
        //    như vậy sẽ không cần truy vấn vào cơ sở dữ liệu nữa!
        //+ sau khi tìm được, giãn thời gian
        private static void CreateTime()
        {
            for (int i = 0; i < AlgorithmRunner.AdjacencyMatrixSize; i++)
            {
                for (int j = i + 1; j < AlgorithmRunner.AdjacencyMatrixSize; j++)
                {
                    if (AlgorithmRunner.AdjacencyMatrix[i, j] == 1)
                    {
                        if (CheckTime(i, j))
                        {
                            IncSubjects(i, j);
                        }
                    }
                }
            }
        }

        public static int CalcShift(DateTime OldTime, DateTime NewTime)
        {
            int Shift1Index = InputHelper.BusyShifts.FindIndex(m => m.Time == OldTime);
            int Shift2Index = InputHelper.BusyShifts.FindIndex(m => m.Time == NewTime);
            return Math.Abs(Shift1Index - Shift2Index);
        }

        public static DateTime IncTime(DateTime Time, int Shift)
        {
            int CurrentShiftIndex = ShiftList.FindIndex(m => m.Time == Time);
            if (CurrentShiftIndex + Shift > ShiftList.Count - 1)
            {
                AlgorithmRunner.SaveOBJ("IsNotEnoughShift", CurrentShiftIndex + Shift);
                Environment.Exit(0);
            }
            return ShiftList[CurrentShiftIndex + Shift].Time;
        }

        private static Boolean CheckTime(int i, int j)
        {
            DateTime T1 = AlgorithmRunner.GroupsTime[i];
            DateTime T2 = AlgorithmRunner.GroupsTime[j];
            int Shift1Index = InputHelper.BusyShifts.FindIndex(m => m.Time == T1);
            int Shift2Index = InputHelper.BusyShifts.FindIndex(m => m.Time == T2);
            //Đã đạt yêu cầu, trả về 0 không tăng cái nào lên làm gì
            if (Math.Abs(Shift1Index - Shift2Index) > InputHelper.Options.DateMin)
            {
                return false;
            }
            // mặc định do j > i nên nếu Shift1Index > Shift2Index thì tăng t1 không thì tăng t2
            return true;
        }

        public static void Run()
        {
            Init();
            //CreateTime();
            AlgorithmRunner.SaveOBJ("GroupsTime", AlgorithmRunner.GroupsTime);
            AlgorithmRunner.SaveOBJ("MaxColorTime", AlgorithmRunner.MaxColorTime);

        }
    }
}
