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
                Split = Data[i].Trim().Split(new char[] { ' ' });
                for (int j = 0; j < Split.Length; j++)
                    AdjacencyMatrix[i, j] = Convert.ToInt32(Split[j]);
            }
        }

        public static T ReadObj<T>(String ObjectName)
        {
            return fastJSON.JSON.Instance.ToObject<T>(File.ReadAllText(Path + ObjectName + ".jso", Encoding.UTF8));
        }

        public static Object ReadObj(String ObjectName)
        {
            return fastJSON.JSON.Instance.ToObject(File.ReadAllText(Path + ObjectName + ".jso", Encoding.UTF8));
        }

        public static void WriteObj(String ObjectName, Object Obj)
        {
            System.IO.File.WriteAllText(Path + ObjectName + ".jso", fastJSON.JSON.Instance.ToJSON(Obj), Encoding.UTF8);
        }

        public static Dictionary<KeyType, List<ValueItemType>> GetDictionary<KeyType, ValueItemType>(object Obj)
        {
            Dictionary<KeyType, List<ValueItemType>> Result = new Dictionary<KeyType, List<ValueItemType>>();
            Dictionary<KeyType, Object> X = (Dictionary<KeyType, Object>)((object[])Obj)[0];
            for (int i = 0; i < X.Keys.Count; i++)
            {
                if (X.ElementAt(i).Value is ValueItemType)
                {
                    Result.Add(X.ElementAt(i).Key, new List<ValueItemType>() { (ValueItemType)X.ElementAt(i).Value });
                }
                else
                {
                    List<Object> Values = (List<Object>)X.ElementAt(i).Value;
                    List<ValueItemType> Y = new List<ValueItemType>();
                    for (int j = 0; j < Values.Count(); j++)
                    {
                        Y.Add((ValueItemType)Values[j]);
                    }
                    Result.Add(X.ElementAt(i).Key, Y);
                }
            }
            return Result;
        }

        public static int[] GetIntArray(object Obj)
        {
            int[] Result = new int[((object[])Obj).Length];
            for (int i = 0; i < Result.Length; i++)
            {
                Result[i] = Int32.Parse(((object[])Obj)[i].ToString());
            }
            return Result;
        }

        public static DateTime[] GetDateTimeArray(object Obj)
        {
            DateTime[] Result = new DateTime[((object[])Obj).Length];
            for (int i = 0; i < Result.Length; i++)
            {
                Result[i] = DateTime.Parse(((object[])Obj)[i].ToString());
            }
            return Result;
        }

        public static List<Room>[] GetListRoomArray(object Obj)
        {
            object[] RootObj = ((object[])Obj);
            List<Room>[] Result = new List<Room>[RootObj.Length];
            for (int i = 0; i < Result.Length; i++)
            {
                List<Room> Item = new List<Room>();
                foreach (object ob in (List<Object>)RootObj[i])
                {
                    Dictionary<String, Object> R = (Dictionary<String, Object>)ob;
                    Item.Add(new Room() { RoomID = R["RoomID"].ToString(), Container = int.Parse(R["Container"].ToString()) });
                }
                Result[i] = Item;
            }
            return Result;
        }

        public static List<String>[][] GetListString2DArray(object Obj)
        {
            object[] RootObj = ((object[])Obj);
            List<String>[][] Result = new List<String>[RootObj.Length][];
            for (int i = 0; i < RootObj.Length; i++)
            {
                List<Object> SubObj = (List<Object>)RootObj[i];
                Result[i] = new List<string>[SubObj.Count];
                for (int j = 0; j < SubObj.Count; j++)
                {
                    List<Object> Items = (List<Object>)SubObj[j];
                    List<String> Item = new List<String>();
                    for (int k = 0; k < Items.Count; k++)
                    {
                        Item.Add((String)Items[k]);
                    }
                    Result[i][j] = Item;
                }
            }
            return Result;
        }

        public void Init()
        {
            InputHelper.Subjects = ReadObj<List<String>>("Subjects");
            InputHelper.Students = GetDictionary<String, String>(ReadObj("Students"));
            InputHelper.Rooms = ReadObj<List<Room>>("Rooms");
            InputHelper.Options = (Options)ReadObj("Options");
        }

        public void RunCreateAdjacencyMatrix()
        {
            Thread thread = new Thread(new ThreadStart(CreateAdjacencyMatrix.Run));
            thread.Name = "CreateAdjacencyMatrix";
            thread.Start();
        }

        public void RunColoring()
        {
            ReadAdjacencyMatrix(Path + "AdjacencyMatrix.txt");
            BeginI = 0;
            Thread thread = new Thread(new ThreadStart(GraphColoringAlgorithm.Run));
            thread.Name = "GraphColoringAlgorithm";
            thread.Start();
        }

        public void RunMakeTime()
        {
            ColorNumber = ReadObj<int>("ColorNumber");
            Colors = GetIntArray(ReadObj("Colors"));
            Thread thread = new Thread(new ThreadStart(MakeTime.Run));
            thread.Name = "MakeTime";
            thread.Start();
        }

        public void RunRoomArrangement()
        {
            Colors = GetIntArray(ReadObj("Colors"));
            MaxColorTime = GetDateTimeArray(ReadObj("MaxColorTime"));
            SubjectTime = GetDateTimeArray(ReadObj("SubjectTime"));
            Thread thread = new Thread(new ThreadStart(RoomArrangement.Run));
            thread.Name = "RoomArrangement";
            thread.Start();
        }

        public void RunSaveToDatabase()
        {
            SubjectTime = GetDateTimeArray(ReadObj("SubjectTime"));
            SubjectRoom = GetListRoomArray(ReadObj("SubjectRoom"));
            SubjectRoomStudents = GetListString2DArray(ReadObj("SubjectRoomStudents"));
            Thread thread = new Thread(new ThreadStart(SaveToDatabase.Run));
            thread.Name = "SaveToDatabase";
            thread.Start();
        }
    }
}
