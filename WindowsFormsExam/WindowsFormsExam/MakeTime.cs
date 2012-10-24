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
        //B1: Gán thời gian tối thiểu cho tất cả các môn dựa vào màu của chúng
        private static void Init()
        {
            AlgorithmRunner.SubjectTime = new DateTime[InputHelper.Subjects.Count];
            AlgorithmRunner.MaxColorTime = new DateTime[AlgorithmRunner.ColorNumber];
            // ngày so với ngày bắt đầu kì thi
            int Date = 0;
            // ca thi
            int Slot = 0;
            for (int ColorNumber = 0; ColorNumber < AlgorithmRunner.ColorNumber; ColorNumber++)
            {
                // các môn cùng màu sẽ cùng ca, cùng ngày thi
                for (int i = 0; i < InputHelper.Subjects.Count; i++)
                {
                    if (AlgorithmRunner.Colors[i] == ColorNumber)
                    {
                        AlgorithmRunner.SubjectTime[i] = InputHelper.StartDate.AddDays(Date)
                                                                              .AddHours(InputHelper.Times[Slot].BGTime.Hour)
                                                                              .AddMinutes(InputHelper.Times[Slot].BGTime.Minute);
                        AlgorithmRunner.MaxColorTime[ColorNumber] = AlgorithmRunner.SubjectTime[i];
                    }
                }
                Slot++;
                if (Slot == InputHelper.Times.Count)
                {
                    Slot = 0;
                    Date++;
                }
            }
        }

        //- B5: Tăng thời gian của tất cả các môn có màu khác và thi sau môn M lên 1 khoảng sao cho > max[màu môn M]
        private static void IncSubjectAfter(int CurrentColor)
        {
            //màu max rồi thì hết cái để tăng.
            if (CurrentColor < AlgorithmRunner.ColorNumber)
            {
                // Thời gian các môn thi sau mà cùng màu thì bằng nhau và bằng max[màu],
                // nên lây max[màu tiếp theo] - max[màu hiện tại]
                // ta sẽ đươc step
                int Step = CalcStep(AlgorithmRunner.MaxColorTime[CurrentColor + 1], AlgorithmRunner.MaxColorTime[CurrentColor]);
                // tăng thời gian các môn thi sau, màu lớn hơn thì thi sau.
                for (int si = 0; si < AlgorithmRunner.SubjectTime.Count(); si++)
                {
                    if (AlgorithmRunner.Colors[si] > CurrentColor)
                    {
                        AlgorithmRunner.SubjectTime[si] = IncTime(AlgorithmRunner.SubjectTime[si], Step);
                        if (AlgorithmRunner.MaxColorTime[AlgorithmRunner.Colors[si]] < AlgorithmRunner.SubjectTime[si])
                        {
                            // đổi max
                            AlgorithmRunner.MaxColorTime[AlgorithmRunner.Colors[si]] = AlgorithmRunner.SubjectTime[si];
                        }
                    }
                }
            }
        }
        //- B3: tăng thời gian của môn thi sau (Môn M) trong 2 môn đó lên 1 khoảng là X để thoã điều kiện.
        //- B4: nếu thời gian thi của môn M sau khi được tăng > max[màu của môn M] thì 
        //            gán max[màu của môn M] = thời gian thi của môn M rồi qua bước 5
        private static void IncSubjects(int Checker, int i, int j)
        {

            int Index;
            if (Checker == 1)
            {
                // tăng môn i nhưng phải dựa vào môn j là do ko bít hiện tại khoảng cách giữa 2 môn
                // i và j là bao nhiêu, tăng cho vừa khớp với điều kiện cho chắc.
                AlgorithmRunner.SubjectTime[i] = IncTime(AlgorithmRunner.SubjectTime[j], InputHelper.DateMin);
                Index = i;
            }
            else //Checker == 2
            {
                AlgorithmRunner.SubjectTime[j] = IncTime(AlgorithmRunner.SubjectTime[i], InputHelper.DateMin);
                Index = j;
            }
            int CurrentColor = AlgorithmRunner.Colors[Index];
            // Kiểm tra và tăng max
            if (AlgorithmRunner.MaxColorTime[CurrentColor] < AlgorithmRunner.SubjectTime[Index])
            {
                // đổi max
                AlgorithmRunner.MaxColorTime[CurrentColor] = AlgorithmRunner.SubjectTime[Index];
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
        private static void MakeTime()
        {
            for (int i = 0; i < AlgorithmRunner.AdjacencyMatrixSize; i++)
            {
                for (int j = i + 1; j < AlgorithmRunner.AdjacencyMatrixSize; j++)
                {
                    if (AlgorithmRunner.AdjacencyMatrix[i, j] == 1)
                    {
                        int Checker = CheckTime(i, j);
                        if (Checker != 0)
                        {
                            IncSubjects(Checker, i, j);
                        }
                    }
                }
            }
        }

        private static int CalcStep(DateTime OldTime, DateTime NewTime)
        {
            int Result = 0;
            if (OldTime > NewTime)
            {
                while (true)
                {
                    Result++;
                    if (IncTime(OldTime, Result) >= NewTime)
                        return Result;
                }
            }
            else
            {
                while (true)
                {
                    Result++;
                    if (IncTime(NewTime, Result) >= OldTime)
                        return Result;
                }
            }
        }

        private static DateTime IncTime(DateTime Time, int Step)
        {
            DateTime Result = Time;
            // tìm ca thi dựa vào Time
            int CurrentStep = 0;
            for (int i = 0; i < InputHelper.Times.Count; i++)
            {
                if (Time.Hour == InputHelper.Times[i].BGTime.Hour && Time.Minute == InputHelper.Times[i].BGTime.Minute)
                {
                    CurrentStep = i;
                    break;
                }
            }
            
            int FinalStep = CurrentStep + Step;
            // Ví dụ, currentstep = 1 (thi ca 2), step = 5 (Tăng lên 5 ca) ==> finalstep = 6
            // nếu mỗi ngày 4 ca thi
            // thì thời gian kết quả phải là ca thi thứ 3 của ngày hôm sau
            // 6 % 4 = 2 ==> ca thứ 3
            // 6 / 4 = 1 ==> ngày cần tăng
            Result = new DateTime(Time.Year, 
                                  Time.Month, 
                                  Time.Day, 
                                  InputHelper.Times[FinalStep % InputHelper.Times.Count].BGTime.Hour, 
                                  InputHelper.Times[FinalStep % InputHelper.Times.Count].BGTime.Minute, 
                                  0);
            Result = Result.AddDays(FinalStep / InputHelper.Times.Count);
            return Result;
        }

        private static int CheckTime(int i, int j)
        {
            DateTime T1 = AlgorithmRunner.SubjectTime[i];
            DateTime T2 = AlgorithmRunner.SubjectTime[j];
            if (T1 > T2)
            {
                // t1> t2, và nếu sau khi tăng t2 mà lại lớn hơn t1 == > khoảng cách giữa t1 và t2 chưa đạt yêu cầu
                if (IncTime(T2, InputHelper.DateMin) > T1)
                    return 1; // T1 cần giãn
            }
            else
            {
                if (IncTime(T1, InputHelper.DateMin) > T2)
                    return 2; // t2 cần giãn
            }

            return 0;
        }

        public static void Run()
        {
            Init();
            MakeTime();
        }
    }
}
