using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Mvc_ESM.Static_Helper
{
    public class CreateAdjacencyMatrix
    {
        static DKMHEntities db = new DKMHEntities();
        static int AdjacencyMatrixSize = 0;
        public static int[,] AdjacencyMatrix;
        static IEnumerable<pdkmh> qrDKMH;
        //static List<pdkmh> DKMH;
        static int CheckSubject(String Subject1ID, String Subject2ID)
        {
            var StudentLearnSubject1 = from s1 in qrDKMH
                                       where s1.MaMonHoc == Subject1ID
                                       select s1.MaSinhVien;
            var StudentLearnTowSubject = (from s2 in qrDKMH
                                         where s2.MaMonHoc == Subject2ID && StudentLearnSubject1.Contains(s2.MaSinhVien)
                                         select s2.MaSinhVien);
            return (StudentLearnTowSubject.Count() > 0) ? 1 : 0;
        }

        public static void Run()
        {
            qrDKMH = (from su in db.monhocs
                     join dk in db.pdkmhs on su.MaMonHoc equals dk.MaMonHoc
                     //where Static_Helper.InputHelper.Subjects.Contains(su.MaMonHoc)
                     select dk);
            //DKMH = qrDMKH.ToList(); 
            Static_Helper.InputHelper.Subjects = (from q in qrDKMH select q.MaMonHoc).Distinct().ToList();
            AdjacencyMatrixSize = Static_Helper.InputHelper.Subjects.Count;
            AdjacencyMatrix = new int[AdjacencyMatrixSize, AdjacencyMatrixSize];
            for( int i = 0; i < AdjacencyMatrixSize; i++)
            {
                for (int j = i + 1; j < AdjacencyMatrixSize; j++)
                {
                    Static_Helper.ProgressHelper.pbCreateMatrix = (j+1)*100/(AdjacencyMatrixSize-i);
                    Static_Helper.ProgressHelper.CreateMatrixInfo = Static_Helper.InputHelper.Subjects[i] + " - " + Static_Helper.InputHelper.Subjects[j] + " ( " + i + "/" + j + " )";
                    AdjacencyMatrix[i, j] = AdjacencyMatrix[j, i] = 
                        CheckSubject(Static_Helper.InputHelper.Subjects[i], Static_Helper.InputHelper.Subjects[j]);
                }
            }
        }
        //IEnumerable<int> _Colors = Static_Helper.GraphColoringAlgorithm.Run(AdjacencyMatrix);
    }
}