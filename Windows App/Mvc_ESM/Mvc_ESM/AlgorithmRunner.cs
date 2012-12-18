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
        public static Dictionary<String, Group> Data = ReadOBJ<Dictionary<String, Group>>("Groups");
        public static int[] Colors;
        public static int ColorNumber;
        public static DateTime[] GroupsTime;
        public static List<Room>[] GroupsRoom;
        public static List<String>[][] GroupsRoomStudents;
        public static DateTime[] MaxColorTime;
        public class Group
        {
            public string MaMonHoc { get; set; }
            public string TenMonHoc { get; set; }
            public string TenBoMon { get; set; }
            public string TenKhoa { get; set; }
            public byte Nhom { get; set; }
            public Nullable<int> SoLuongDK { get; set; }
            public int GroupID { get; set; }
            public Boolean IsIgnored { get; set; }
        }
        public static List<String> Groups;

        public static Handmade.HandmadeData HandmadeData;

        private static void ReadAdjacencyMatrix(string DataFilePath)
        {
            string[] Data = File.ReadAllLines(DataFilePath);
            string[] Split;
            AdjacencyMatrixSize = Data.Length;
            AdjacencyMatrix = new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
            for (int i = 0; i < AdjacencyMatrixSize; i++)
            {
                Split = Data[i].Trim().Split(new char[] { ' ' });
                for (int j = 0; j < Split.Length; j++)
                    AdjacencyMatrix[i, j] = Convert.ToInt32(Split[j]);
            }
        }

        public static T ReadOBJ<T>(String ObjectName)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(RealPath(ObjectName), Encoding.UTF8));
        }

        public static String RealPath(String Name)
        {
            return Path.Combine(Application.StartupPath, Name + ".jso");
        }

        public static void SaveOBJ(String Name, Object OBJ)
        {
            System.IO.File.WriteAllText(
                RealPath(Name),
                JsonConvert.SerializeObject(OBJ, Formatting.Indented),
                Encoding.UTF8
            );
        }

        public static void DeleteOBJ(String Name)
        {
            System.IO.File.Delete(
                RealPath(Name)
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
            Groups = new List<string>();
            var aGroupList = Data.Where(d => !d.Value.IsIgnored);
            var SubjectsList = aGroupList.Select(m => m.Key.Substring(0, m.Key.IndexOf('_'))).Distinct();
            foreach (var subject in SubjectsList)
            {
                var GroupsInOneSubject = aGroupList.Where(m => m.Value.MaMonHoc == subject);
                for (int i = 1; i <= GroupsInOneSubject.Max(m => m.Value.GroupID); i++)
                {
                    var aGroupItem = subject;
                    foreach (var gi in GroupsInOneSubject.Where(m => m.Value.GroupID == i))
                    {
                        aGroupItem += "_" + gi.Value.Nhom;
                    }
                    Groups.Add(aGroupItem);
                }
            }

            InputHelper.Students = ReadOBJ<Dictionary<String, List<String>>>("Students");
            InputHelper.Rooms = ReadOBJ<List<Room>>("Rooms");
            InputHelper.Options = ReadOBJ<Options>("Options");
            AdjacencyMatrixSize = Groups.Count;
            AdjacencyMatrix = new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
        }

        public static bool JsoExits(string ObjectName)
        {
            return File.Exists(RealPath(ObjectName));
        }

        public static void DeleteJso(string ObjectName)
        {
            if (JsoExits(ObjectName))
            {
                try
                {
                    File.Delete(RealPath(ObjectName));
                }
                catch { }
            }
        }

        public void RunCreateAdjacencyMatrix()
        {
            if (JsoExits("AdjacencyMatrix"))
            {
                //AdjacencyMatrix = ReadOBJ<int[,]>("AdjacencyMatrix");
                ReadAdjacencyMatrix(RealPath("AdjacencyMatrix"));
                if (AdjacencyMatrixSize != Groups.Count())
                {
                    AdjacencyMatrixSize = Groups.Count;
                    AdjacencyMatrix = new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
                    BeginI = 0;
                }
                else
                {
                    if (JsoExits("BeginI"))
                    {
                        BeginI = ReadOBJ<int>("BeginI");
                    }
                    else
                    {
                        BeginI = 0;
                    }
                }
            }
            else
            {
                AdjacencyMatrixSize = Groups.Count;
                AdjacencyMatrix = new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
                BeginI = 0;
            }
            Thread thread = new Thread(new ThreadStart(CreateAdjacencyMatrix.Run));
            thread.Name = "CreateAdjacencyMatrix";
            thread.Start();
        }

        public void RunColoring()
        {
            //AdjacencyMatrix = ReadOBJ<int[,]>("AdjacencyMatrix");
            ReadAdjacencyMatrix(RealPath("AdjacencyMatrix"));
            Thread thread = new Thread(new ThreadStart(GraphColoringAlgorithm.Run));
            thread.Name = "GraphColoringAlgorithm";
            thread.Start();
        }

        public void RunMakeTime()
        {
            ColorNumber = ReadOBJ<int>("ColorNumber");
            Colors = ReadOBJ<int[]>("Colors");
            //AdjacencyMatrix = ReadOBJ<int[,]>("AdjacencyMatrix");
            ReadAdjacencyMatrix(RealPath("AdjacencyMatrix"));
            Thread thread = new Thread(new ThreadStart(MakeTime.Run));
            thread.Name = "MakeTime";
            thread.Start();
        }

        public void RunRoomArrangement()
        {
            Colors = ReadOBJ<int[]>("Colors");
            MaxColorTime = ReadOBJ<DateTime[]>("MaxColorTime");
            GroupsTime = ReadOBJ<DateTime[]>("GroupsTime");
            Thread thread = new Thread(new ThreadStart(RoomArrangement.Run));
            thread.Name = "RoomArrangement";
            thread.Start();
        }

        public void RunSaveToDatabase()
        {
            GroupsTime = ReadOBJ<DateTime[]>("GroupsTime");
            GroupsRoom = ReadOBJ<List<Room>[]>("GroupsRoom");
            GroupsRoomStudents = ReadOBJ<List<String>[][]>("GroupsRoomStudents");
            Thread thread = new Thread(new ThreadStart(SaveToDatabase.Run));
            thread.Name = "SaveToDatabase";
            thread.Start();
        }

        public void RunDeleteOldDatabase()
        {
            Thread thread = new Thread(new ThreadStart(SaveToDatabase.DeleteOld));
            thread.Name = "RunDeleteOldDatabase";
            thread.Start();
        }

        public void RunHandmade()
        {
            HandmadeData = ReadOBJ<Handmade.HandmadeData>("Handmade");
            Thread thread = new Thread(new ThreadStart(Handmade.Run));
            thread.Name = "Handmade";
            thread.Start();
            DeleteJso("Handmade");
        }
    }
}
