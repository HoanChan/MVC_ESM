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
        public static Boolean Stoped = false;
        //static IQueryable<DKMHI> qrDKMH;
        //static List<pdkmh> DKMH;
        static int CheckSubject(String Subject1ID, String Subject2ID)
        {   
            //var StudentLearnSubject1 = from s1 in qrDKMH
            //                           where s1.MaMonHoc == Subject1ID 
            //                                    && !InputHelper.Students[Subject1ID].Contains(s1.MaSinhVien)
            //                           select s1.MaSinhVien;
            //var StudentLearnTowSubject = (from s2 in qrDKMH
            //                              where s2.MaMonHoc == Subject2ID 
            //                                    && StudentLearnSubject1.Contains(s2.MaSinhVien) 
            //                                    && !InputHelper.Students[Subject2ID].Contains(s2.MaSinhVien)
            //                             select s2.MaSinhVien);
            //return (StudentLearnTowSubject.Count() > 0) ? 1 : 0;
            String StudentsList1 = "";
            String StudentsList2 = "";
            try
            {
                foreach (String st in InputHelper.Students[Subject1ID])
                {
                    StudentsList1 += (StudentsList1.Length > 0 ? ", " : "") + "'" + st + "'";
                }
            }
            catch { }

            try
            {
                foreach (String st in InputHelper.Students[Subject2ID])
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
            IEnumerable<String> Result = db.Database.SqlQuery<String>("select s1.MaSinhVien from pdkmh as s1 "
                                                                    + "where s1.MaSinhVien in (select s2.MaSinhVien from pdkmh as s2 "
                                                                                             + "where s2.MaMonHoc = @S2ID "
                                                                                             + (StudentsList2.Length > 0 ? "and not(s2.MaSinhVien in (" + StudentsList2 + "))" : "")
                                                                                             + ") "
                                                                    + (StudentsList1.Length > 0 ? "and not(s1.MaSinhVien in (" + StudentsList1 + "))" : "")
                                                                    + "and s1.MaMonHoc = @S1ID", pa);
            return Result.Count() > 0 ? 1 : 0;
        }

        public static void Run()
        {
            Stop = false;
            Stoped = false;
            Begin(AlgorithmRunner.AdjacencyMatrix,AlgorithmRunner.BeginI);
            Stoped = true;
        }

        public static void Begin(int[,] oldAdjacencyMatrix, int beginI)
        {

            //qrDKMH = from su in db.monhocs
            //         join dk in db.pdkmhs on su.MaMonHoc equals dk.MaMonHoc
            //         where InputHelper.Subjects.Contains(su.MaMonHoc)
            //         select new DKMHI() {MaMonHoc = dk.MaMonHoc, MaSinhVien = dk.MaSinhVien };

            //InputHelper.Subjects = (from q in qrDKMH select q.MaMonHoc).Distinct().ToList();

            AdjacencyMatrixSize = InputHelper.Subjects.Count;
            AdjacencyMatrix = oldAdjacencyMatrix;//new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
            for (i = beginI; i < AdjacencyMatrixSize; i++)
            {
                for (j = i + 1; j < AdjacencyMatrixSize; j++)
                {
                    ProgressHelper.CreateMatrixInfo = i + "/" + j;
                    ProgressHelper.pbCreateMatrix = 100 * (i * AdjacencyMatrixSize + j - i) / (AdjacencyMatrixSize * AdjacencyMatrixSize / 2);
                    AdjacencyMatrix[i, j] = AdjacencyMatrix[j, i] = 
                        CheckSubject(InputHelper.Subjects[i], InputHelper.Subjects[j]);
                }
                if (Stop)
                {
                    WriteAdjacencyMatrix(AdjacencyMatrix, AlgorithmRunner.Path + "AdjacencyMatrix.txt");
                    AlgorithmRunner.WriteObj("BeginI", i + 1);
                    Stoped = true;
                    return;
                }
            }
            WriteAdjacencyMatrix(AdjacencyMatrix, AlgorithmRunner.Path + "AdjacencyMatrix.txt");
            AlgorithmRunner.WriteObj("BeginI", 0);
        }

        private static void WriteAdjacencyMatrix(int[,] AdjacencyMatrix, string DataFilePath)
        {
            StreamWriter file = new System.IO.StreamWriter(DataFilePath);
            for (int i = 0; i <= AdjacencyMatrix.GetUpperBound(0); i++)
            {
                string Text = "";
                for (int j = 0; j <= AdjacencyMatrix.GetUpperBound(1); j++)
                {
                    Text += AdjacencyMatrix[i, j].ToString();
                    if (j != AdjacencyMatrix.GetUpperBound(1))
                        Text += " ";
                }
                file.WriteLine(Text);
            }
            AdjacencyMatrix = null;
            file.Close();
            file.Dispose();
        }

    }
}