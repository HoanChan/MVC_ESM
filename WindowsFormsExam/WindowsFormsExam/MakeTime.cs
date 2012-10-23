using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_ESM.Static_Helper
{
    public class MakeTime
    {
        //B1: Gán thời gian tối thiểu cho tất cả các môn dựa vào màu của chúng
        private static void Step1()
        {
            AlgorithmRunner.SubjectTime = new DateTime[InputHelper.Subjects.Count];
            AlgorithmRunner.MaxColorTime = new DateTime[AlgorithmRunner.ColorNumber];
            int Date = 0;
            for (int ColorNumber = 0; ColorNumber < AlgorithmRunner.ColorNumber; ColorNumber++)
            {
                int Slot = 0;
                for (int i = 0; i < InputHelper.Subjects.Count; i++)
                {
                    if (AlgorithmRunner.Colors[i] == ColorNumber)
                    {
                        AlgorithmRunner.SubjectTime[i] = InputHelper.StartDate.AddDays(Date).AddHours(InputHelper.Times[Slot].BGTime.Hour);
                        Slot++;
                        if (Slot == InputHelper.Times.Count)
                        {
                            Slot = 0;
                            Date++;
                        }
                        AlgorithmRunner.MaxColorTime[ColorNumber] = AlgorithmRunner.SubjectTime[i];
                    }
                }
                if (Slot != 0)
                {
                    Date++;
                }
            }
        }
        //B2: tìm sinh viên có 2 môn thi chưa thoã điều kiện là cách nhau n buổi
        //+ Bước này không duyệt theo sinh viên mà sử dụng ma trận kề để tìm 2 môn có sinh viên thi cùng
        //    như vậy sẽ không cần truy vấn vào cơ sở dữ liệu nữa!
        //+ sau khi tìm được, giãn thời gian
        private static void Step2()
        {
            for (int i = 0; i < AlgorithmRunner.AdjacencyMatrixSize; i++)
            {
                for (int j = i + 1; j < AlgorithmRunner.AdjacencyMatrixSize; j++)
                {
                    if (AlgorithmRunner.AdjacencyMatrix[i, j] == 1)
                    {
                        TimeSpan Time = new TimeSpan(InputHelper.DateMin, 0, 0, 0);
                        int Checker = 0;
                        if (AlgorithmRunner.SubjectTime[i] > AlgorithmRunner.SubjectTime[j] && AlgorithmRunner.SubjectTime[i] - AlgorithmRunner.SubjectTime[j] < Time)
                        {
                            Checker = 1; // môn i cần giãn thời gian
                        }
                        else if (AlgorithmRunner.SubjectTime[j] - AlgorithmRunner.SubjectTime[i] < Time)
                        {
                            Checker = 2; // môn j cần giãn thời gian
                        }
                        if (Checker != 0)
                        {

                        }
                    }
                }
            }
        }

    }
}
