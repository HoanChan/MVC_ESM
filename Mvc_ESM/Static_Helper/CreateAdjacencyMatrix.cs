using Mvc_ESM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_ESM.Static_Helper
{
    public class CreateAdjacencyMatrix
    {
        static DKMHEntities db = new DKMHEntities();
        static int AdjacencyMatrixSize = 0;
        public static int[,] AdjacencyMatrix;
        static List<pdkmh> qrDMKH;
        static int CheckSubject(String Subject1ID, String Subject2ID)
        {
            var StudentLearnSubject1 = from su in qrDMKH 
                                       where su.MaMonHoc == Subject1ID 
                                       select su.MaSinhVien;
            var StudentLearnTowSubject = from su in qrDMKH 
                                         where su.MaMonHoc == Subject2ID && StudentLearnSubject1.Contains(su.MaSinhVien) 
                                         select su.MaSinhVien;
            return (StudentLearnTowSubject.Count() > 0) ? 1 : 0;
        }

        public static void Run()
        {
            qrDMKH = (from su in db.monhocs
                     join dk in db.pdkmhs on su.MaMonHoc equals dk.MaMonHoc
                     where Static_Helper.InputHelper.Subjects.Contains(su.MaMonHoc)
                     select dk).ToList();
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