using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_ESM.Models;

namespace Mvc_ESM.Controllers
{
    public class CreateMatrixController : Controller
    {
        DKMHEntities db = new DKMHEntities();
        static int AdjacencyMatrixSize = 0;
        static int[,] AdjacencyMatrix;
        List<object> Subject;


        private int CheckSubject(String su1, String su2)
        {
            var Data1 = (from sv in db.sinhviens
                         join d in db.pdkmhs on sv.MaSinhVien equals d.MaSinhVien
                         where d.MaMonHoc == su1
                         select new
                         {
                             mssv = sv.MaSinhVien
                         }).Distinct();

            List<object> l1 = new List<object>(Data1);

            var Data2 = (from sv in db.sinhviens
                         join d in db.pdkmhs on sv.MaSinhVien equals d.MaSinhVien
                         where d.MaMonHoc == su2
                         select new
                         {
                             mssv = sv.MaSinhVien
                         }).Distinct();
            List<object> l2 = new List<object>(Data2);
            foreach (var a in l1)
                if (l2.Contains(a))
                    return 1;
            return 0;
        }

        private void InitMatrix()
        {
            var monhoc = (from s in db.pdkmhs
                          select new
                          {
                              MSMH = s.MaMonHoc
                          }).Distinct();
            Subject = new List<object>(monhoc);
            AdjacencyMatrixSize = Subject.Count();

            int i = 0;
            int j;
            foreach (var su1 in Subject)
            {
                j = 0;
                foreach (var su2 in Subject)
                {
                    AdjacencyMatrix[i, j] = CheckSubject(su1.ToString(), su2.ToString());
                    j++;
                }
                i++;
            }
        }
        IEnumerable<int> _Colors = Static_Helper.GraphColoringAlgorithm.Run(AdjacencyMatrix);
    }
}
