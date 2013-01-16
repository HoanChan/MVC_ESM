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
        public static int[] Colors;
        public static int ColorNumber;
        public static DateTime[] GroupsTime;
        public static List<Room>[] GroupsRoom;
        public static List<String>[][] GroupsRoomStudents;
        public static DateTime[] MaxColorTime;
        public static List<String> Groups;

        private static Boolean b_IsBusy = false;

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

        public static bool OBJExits(string ObjectName)
        {
            return File.Exists(RealPath(ObjectName));
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
            var aGroupList = InputHelper.Groups.Where(d => !d.Value.IsIgnored);
            var SubjectsList = aGroupList.Select(m => m.Key.Substring(0, m.Key.IndexOf('_'))).Distinct();
            foreach (var subject in SubjectsList)
            {
                var GroupsInOneSubject = aGroupList.Where(m => m.Value.MaMonHoc == subject).Select(m => m.Value);
                var GroupsIDList = GroupsInOneSubject.Select(m => m.GroupID).Distinct();
                foreach (var aID in GroupsIDList)
                {
                    var aGroupItem = subject;
                    foreach (var gi in GroupsInOneSubject.Where(m => m.GroupID == aID))
                    {
                        aGroupItem += "_" + gi.Nhom;
                    }
                    Groups.Add(aGroupItem);
                }
            }
            AdjacencyMatrixSize = Groups.Count;
            AdjacencyMatrix = new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
        }
                
        public void RunCreateAdjacencyMatrix()
        {
            if (OBJExits("AdjacencyMatrix"))
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
                    if (OBJExits("BeginI"))
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

        public void RunCalc()
        {
            Thread thread = new Thread(new ThreadStart(RunNext));
            thread.Name = "RunCalc";
            thread.Start();
        }

        private void RunNext()
        {
            IsBusy = true;
            if (OBJExits("AdjacencyMatrix") && !OBJExits("BeginI"))
            {
                SaveOBJ("Status", "inf Đang xếp lịch...");
                ReadAdjacencyMatrix(RealPath("AdjacencyMatrix"));
                GraphColoringAlgorithm.Run();
                SaveOBJ("Status", "inf Tô màu xong! Đang xếp thời gian...");
                RoomArrangement.Run();
                SaveOBJ("Status", "inf Xếp lịch xong! Hãy lưu kết quả vào CSDL (Tất cả các kết quả xếp lịch trước sẽ bị xoá)");
            }
            else
            {
                SaveOBJ("Status", "err Chưa hoàn thiện quá trình phân tích CSDL");
            }
            IsBusy = false;
        }

        public void RunSaveToDatabase()
        {
            if (OBJExits("GroupsTime") && OBJExits("GroupsRoom") && OBJExits("GroupsRoomStudents"))
            {
                GroupsTime = ReadOBJ<DateTime[]>("GroupsTime");
                GroupsRoom = ReadOBJ<List<Room>[]>("GroupsRoom");
                GroupsRoomStudents = ReadOBJ<List<String>[][]>("GroupsRoomStudents");
                Thread thread = new Thread(new ThreadStart(SaveToDatabase.Run));
                thread.Name = "SaveToDatabase";
                thread.Start();
            }
            else
            {
                SaveOBJ("Status", "err Chưa hoàn thiện quá trình xếp lịch");
            }
        }

        public void RunStop()
        {
            Thread thread = new Thread(new ThreadStart(Stop));
            thread.Name = "RunStop";
            thread.Start();
        }

        private void Stop()
        {
            if (!CreateAdjacencyMatrix.Stoped)
            {
                CreateAdjacencyMatrix.Stop = true;
            }
            else
            {
                Clear();
                Environment.Exit(0);
            }
        }

        public static void Clear()
        {
            IsBusy = false;
            DeleteOBJ("Status");
        }

        public void RunHandmade()
        {
            HandmadeData = ReadOBJ<Handmade.HandmadeData>("Handmade");
            Thread thread = new Thread(new ThreadStart(Handmade.Run));
            thread.Name = "Handmade";
            thread.Start();
            DeleteOBJ("Handmade");
        }

        public static bool IsBusy
        {
            get { return b_IsBusy; }
            set
            {
                b_IsBusy = value;
                if (b_IsBusy)
                {
                    SaveOBJ("IsBusy", true);
                }
                else
                {
                    DeleteOBJ("IsBusy");
                }
            }
        }
    }
}
