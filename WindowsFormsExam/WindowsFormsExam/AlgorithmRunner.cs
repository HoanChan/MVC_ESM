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
        public static List<Room>[] SubjectRoom;
        public static List<String>[][] SubjectRoomStudents;
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

        private static T ReadObj<T>(String ObjectName)
        {
            return (T)fastJSON.JSON.Instance.ToObject(File.ReadAllText(Path + ObjectName + ".jso", Encoding.UTF8));
        }

        public void Init()
        {
            ReadAdjacencyMatrix(Path + "AdjacencyMatrix.txt");
            BeginI = 0;
            InputHelper.Subjects = ReadObj<List<String>>("Subjects");
            InputHelper.Students = ReadObj<Hashtable>("Students");
            InputHelper.Options = ReadObj<Options>("Options");
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

        public void RunMakeTime()
        {
            Thread thread = new Thread(new ThreadStart(MakeTime.Run));
            thread.Name = "MakeTime";
            thread.Start();
        }

        public void RunRoomArrangement()
        {
            Thread thread = new Thread(new ThreadStart(RoomArrangement.Run));
            thread.Name = "RoomArrangement";
            thread.Start();
        }
    }
}
