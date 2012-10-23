using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mvc_ESM.Static_Helper
{
    class AlgorithmRunner
    {
        public static int[,] AdjacencyMatrix;
        public static int AdjacencyMatrixSize;
        public static int BeginI;
        public static String Path = Application.StartupPath + "\\";
        public static int[] Colors;
        public static int ColorNumber;
        public static DateTime[] SubjectTime;
        public static DateTime[] MaxColorTime;
        private void ReadAdjacencyMatrix(string DataFilePath)
        {
            string[] Data = File.ReadAllLines(DataFilePath);
            string[] Split; 
            AdjacencyMatrixSize = Data.Length;
            AdjacencyMatrix = new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
            for (int i = 0; i < AdjacencyMatrixSize; i++)
            {
                Split = Data[i].Split(new char[] { ' ' });
                for (int j = 0; j < Split.Length; j++)
                    AdjacencyMatrix[i, j] = Convert.ToInt32(Split[j]);
            }
        }

        public void Init()
        {
            ReadAdjacencyMatrix(Path + "AdjacencyMatrix.txt");
            BeginI = 0;
            List<String> Subjects = (List<String>) Mvc_ESM.Static_Helper.XML.XML2OBJ(Path + "Subjects.txt", typeof(List<String>));
            HastableStudent Students = (HastableStudent) Mvc_ESM.Static_Helper.XML.XML2OBJ(Path + "Subjects.txt", typeof(HastableStudent));
            InputHelper.DateMin = 1;
            InputHelper.StartDate = DateTime.Now.Date;
            InputHelper.Times = new List<ExamTime>(){   new ExamTime(){
                                                            BGTime = DateTime.Now.Date.AddHours(7),
                                                            ETime = DateTime.Now.Date.AddHours(8),
                                                            Name = "1"
                                                        },
                                                        new ExamTime(){
                                                            BGTime = DateTime.Now.Date.AddHours(9),
                                                            ETime = DateTime.Now.Date.AddHours(10),
                                                            Name = "2"
                                                        },
                                                        new ExamTime(){
                                                            BGTime = DateTime.Now.Date.AddHours(13),
                                                            ETime = DateTime.Now.Date.AddHours(14),
                                                            Name = "2"
                                                        },
                                                        new ExamTime(){
                                                            BGTime = DateTime.Now.Date.AddHours(15),
                                                            ETime = DateTime.Now.Date.AddHours(16),
                                                            Name = "2"
                                                        }
                                                    };
            InputHelper.NumDate = 30;
        }

        public void RunCreateAdjacencyMatrix()
        {
            Thread thread = new Thread(new ThreadStart(CreateAdjacencyMatrix.Run));
            thread.Name = "CreateAdjacencyMatrix";
            thread.Start();
        }

        public void RunColoring()
        {
            Thread thread = new Thread(new ThreadStart(GraphColoringAlgorithm.Run));
            thread.Name = "GraphColoringAlgorithm";
            thread.Start();
        }
    }
}
