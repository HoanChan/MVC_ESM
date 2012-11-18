using Newtonsoft.Json;
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
        public static DateTime[] GroupsTime;
        public static List<Room>[] GroupsRoom;
        public static List<String>[][] GroupsRoomStudents;
        public static DateTime[] MaxColorTime;
        public static List<String> Groups;

        public static T ReadJson<T>(String ObjectName)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(Path + ObjectName + ".jso", Encoding.UTF8));
        }

        public static void WriteJson(String ObjectName, Object Obj)
        {
            System.IO.File.WriteAllText(
                Path + ObjectName + ".jso",
                JsonConvert.SerializeObject(Obj, Formatting.Indented),
                Encoding.UTF8
            ); 
        }

        public static String GetSubjectID(String GroupID)
        {
            return GroupID.Substring(0, GroupID.IndexOf('_'));
        }

        public static String GetClassList(String GroupID)
        {
            return GroupID.Substring(GroupID.IndexOf('_') + 1).Replace('_', ',');
        }

        public void Init()
        {
            InputHelper.Subjects = ReadJson<Dictionary<String, List<Class>>>("Subjects");
            Groups = ReadJson<List<String>>("Groups");
            InputHelper.Students = ReadJson<Dictionary<String, List<String>>>("Students");
            InputHelper.Rooms = ReadJson<List<Room>>("Rooms");
            InputHelper.Options = ReadJson<Options>("Options");
            AdjacencyMatrixSize = Groups.Count;
            AdjacencyMatrix = new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
        }

        public void RunCreateAdjacencyMatrix()
        {
            if (File.Exists(Path + "AdjacencyMatrix.jso"))
            {
                AdjacencyMatrix = ReadJson<int[,]>("AdjacencyMatrix");
            }
            if (File.Exists(Path + "BeginI.jso"))
            {
                BeginI =  ReadJson<int>("BeginI");
            }
            else
            {
                BeginI = 0;
            }
            Thread thread = new Thread(new ThreadStart(CreateAdjacencyMatrix.Run));
            thread.Name = "CreateAdjacencyMatrix";
            thread.Start();
        }

        public void RunColoring()
        {
            AdjacencyMatrix = ReadJson<int[,]>("AdjacencyMatrix");
            Thread thread = new Thread(new ThreadStart(GraphColoringAlgorithm.Run));
            thread.Name = "GraphColoringAlgorithm";
            thread.Start();
        }

        public void RunMakeTime()
        {
            ColorNumber = ReadJson<int>("ColorNumber");
            Colors = ReadJson<int[]>("Colors");
            AdjacencyMatrix = ReadJson<int[,]>("AdjacencyMatrix");
            Thread thread = new Thread(new ThreadStart(MakeTime.Run));
            thread.Name = "MakeTime";
            thread.Start();
        }

        public void RunRoomArrangement()
        {
            Colors = ReadJson<int[]>("Colors");
            MaxColorTime = ReadJson<DateTime[]>("MaxColorTime");
            GroupsTime = ReadJson<DateTime[]>("GroupsTime");
            Thread thread = new Thread(new ThreadStart(RoomArrangement.Run));
            thread.Name = "RoomArrangement";
            thread.Start();
        }

        public void RunSaveToDatabase()
        {
            GroupsTime = ReadJson<DateTime[]>("GroupsTime");
            GroupsRoom = ReadJson<List<Room>[]>("GroupsRoom");
            GroupsRoomStudents = ReadJson<List<String>[][]>("GroupsRoomStudents");
            Thread thread = new Thread(new ThreadStart(SaveToDatabase.Run));
            thread.Name = "SaveToDatabase";
            thread.Start();
        }
    }
}
