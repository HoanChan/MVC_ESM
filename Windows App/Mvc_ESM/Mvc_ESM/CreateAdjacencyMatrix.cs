using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Mvc_ESM.Static_Helper
{
    public class DKMHI
    {
        public string MaSinhVien;
        public string MaMonHoc;

    }
    public class CreateAdjacencyMatrix
    {
        static DKMHEntities db = new DKMHEntities();
        static int AdjacencyMatrixSize = 0;
        public static int[,] AdjacencyMatrix;
        public static int i, j;
        public static Boolean Stop = false;
        public static Boolean Stoped = true;

        static int CheckGroups(String Group1ID, String Group2ID)
        {   
            String Subject1ID = AlgorithmRunner.GetSubjectID(Group1ID);
            String Subject2ID = AlgorithmRunner.GetSubjectID(Group2ID);
            String Group1 = AlgorithmRunner.GetClassList(Group1ID);
            String Group2 = AlgorithmRunner.GetClassList(Group2ID);
            if (Subject1ID == Subject2ID && Group1 != Group2)
            {
                return 1;
            }
            String StudentsList1 = "";
            String StudentsList2 = "";
            try
            {
                foreach (String st in InputHelper.IgnoreStudents[Subject1ID])
                {
                    StudentsList1 += (StudentsList1.Length > 0 ? ", " : "") + "'" + st + "'";
                }
            }
            catch { }

            try
            {
                foreach (String st in InputHelper.IgnoreStudents[Subject2ID])
                {
                    StudentsList2 += (StudentsList2.Length > 0 ? ", " : "") + "'" + st + "'";
                }
            }
            catch { }

            var pa = new SqlParameter[] 
                        { 
                            new SqlParameter("@S1ID", SqlDbType.NVarChar) { Value = Subject1ID },
                            new SqlParameter("@S2ID", SqlDbType.NVarChar) { Value = Subject2ID }
                        };
            int Result = db.Database.SqlQuery<int>("select count(s1.MaSinhVien) from pdkmh as s1 "
                                                                    + "where s1.MaSinhVien in (select s2.MaSinhVien from pdkmh as s2 "
                                                                                             + "where s2.MaMonHoc = @S2ID "
                                                                                             + (StudentsList2.Length > 0 ? "and not(s2.MaSinhVien in (" + StudentsList2 + "))" : "")
                                                                                             + "and s2.Nhom in(" + Group2 +")"
                                                                                             + ") "
                                                                    + (StudentsList1.Length > 0 ? "and not(s1.MaSinhVien in (" + StudentsList1 + "))" : "")
                                                                    + "and s1.MaMonHoc = @S1ID "
                                                                    + "and s1.Nhom in(" + Group1 + ")", pa).ElementAt(0);
            return Result == 0 ? 0 : 1;
        }

        public static void Run()
        {
            AlgorithmRunner.IsBusy = true;
            Stop = false;
            Stoped = false;
            Begin(AlgorithmRunner.AdjacencyMatrix, AlgorithmRunner.BeginI);
            Stoped = true;
            AlgorithmRunner.IsBusy = false;
        }

        public static void Begin(int[,] oldAdjacencyMatrix, int beginI)
        {
            AdjacencyMatrixSize = AlgorithmRunner.AdjacencyMatrixSize;
            AdjacencyMatrix = oldAdjacencyMatrix;
            for (i = beginI; i < AdjacencyMatrixSize; i++)
            {
                for (j = i + 1; j < AdjacencyMatrixSize; j++)
                {
                    //ProgressHelper.CreateMatrixInfo = (1 + i) + "/" + (1 + j);
                    //ProgressHelper.pbCreateMatrix = 100 * (i * AdjacencyMatrixSize + j) / (AdjacencyMatrixSize * AdjacencyMatrixSize);
                    AlgorithmRunner.SaveOBJ("Status", "inf Đang phân tích dữ liệu (" + (1 + i) + "/" + (1 + j) + ")...");
                    AdjacencyMatrix[i, j] = AdjacencyMatrix[j, i] = CheckGroups(AlgorithmRunner.Groups[i], AlgorithmRunner.Groups[j]);
                }
                if (Stop)
                {
                    //AlgorithmRunner.SaveOBJ("AdjacencyMatrix", AdjacencyMatrix);
                    WriteAdjacencyMatrix(AdjacencyMatrix, AlgorithmRunner.RealPath("AdjacencyMatrix"));
                    AlgorithmRunner.SaveOBJ("BeginI", i + 1);
                    AlgorithmRunner.Clear();
                    Stoped = true;
                    Environment.Exit(0);
                    return;
                }
            }
            WriteAdjacencyMatrix(AdjacencyMatrix, AlgorithmRunner.RealPath("AdjacencyMatrix"));
            AlgorithmRunner.DeleteOBJ("BeginI");
        }
        private static void WriteAdjacencyMatrix(int[,] Matrix, string DataFilePath)
        {
            StreamWriter file = new System.IO.StreamWriter(DataFilePath);
            for (int i = 0; i <= Matrix.GetUpperBound(0); i++)
            {
                string Text = "";
                for (int j = 0; j <= Matrix.GetUpperBound(1); j++)
                {
                    Text += Matrix[i, j].ToString();
                    if (j != Matrix.GetUpperBound(1))
                        Text += " ";
                }
                file.WriteLine(Text);
            }
            Matrix = null;
            file.Close();
            file.Dispose();
        }
    }
}