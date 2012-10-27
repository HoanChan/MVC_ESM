using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;

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
        static IQueryable<DKMHI> qrDKMH;
        //static List<pdkmh> DKMH;
        static int CheckSubject(String Subject1ID, String Subject2ID)
        {   
            var StudentLearnSubject1 = from s1 in qrDKMH
                                       where s1.MaMonHoc == Subject1ID 
                                                && !((List<String>)InputHelper.Students[Subject1ID]).Contains(s1.MaSinhVien)
                                       select s1.MaSinhVien;
            var StudentLearnTowSubject = (from s2 in qrDKMH
                                          where s2.MaMonHoc == Subject2ID 
                                                && StudentLearnSubject1.Contains(s2.MaSinhVien) 
                                                && !((List<String>)InputHelper.Students[Subject2ID]).Contains(s2.MaSinhVien)
                                         select s2.MaSinhVien);
            return (StudentLearnTowSubject.Count() > 0) ? 1 : 0;
        }

        public static void Run()
        {
            Begin(AlgorithmRunner.AdjacencyMatrix,AlgorithmRunner.BeginI);
        }

        public static void Begin(int[,] oldAdjacencyMatrix, int beginI)
        {

            qrDKMH = from su in db.monhocs
                     join dk in db.pdkmhs on su.MaMonHoc equals dk.MaMonHoc
                     where InputHelper.Subjects.Contains(su.MaMonHoc)
                     select new DKMHI() {MaMonHoc = dk.MaMonHoc, MaSinhVien = dk.MaSinhVien };

            InputHelper.Subjects = (from q in qrDKMH select q.MaMonHoc).Distinct().ToList();

            AdjacencyMatrixSize = InputHelper.Subjects.Count;
            AdjacencyMatrix = oldAdjacencyMatrix;//new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
            for (i = beginI; i < AdjacencyMatrixSize; i++)
            {
                for (j = i + 1; j < AdjacencyMatrixSize; j++)
                {
                    AdjacencyMatrix[i, j] = AdjacencyMatrix[j, i] = 
                        CheckSubject(InputHelper.Subjects[i], InputHelper.Subjects[j]);
                }
                if (Stop)
                {
                    WriteAdjacencyMatrix(AdjacencyMatrix, AlgorithmRunner.Path + "AdjacencyMatrix.txt");
                    Stoped = true;
                    return;
                }
            }
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