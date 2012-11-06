using Model;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace Mvc_ESM.Static_Helper
{
    class SaveToDatabase
    {   
        public static void Run()
        {
            DeleteOld();
            Save();
        }

        private static void DeleteOld()
        {
            DKMHEntities db = new DKMHEntities();
            db.Database.ExecuteSqlCommand("DELETE FROM Thi");
            db.Database.ExecuteSqlCommand("DELETE FROM CaThi");           
        }

        private static void Save()
        {
            DKMHEntities db = new DKMHEntities();            
            for (int SubjectIndex = 0; SubjectIndex < InputHelper.Subjects.Count; SubjectIndex++)
            {
                Thi aRecord = new Thi();
                aRecord.MaMonHoc = InputHelper.Subjects[SubjectIndex];

                DateTime FirstStepTime = InputHelper.Options.StartDate.AddHours(InputHelper.Options.Times[0].BGTime.Hour)
                                                                      .AddMinutes(InputHelper.Options.Times[0].BGTime.Minute);
                String StepID = MakeTime.CalcStep(FirstStepTime, AlgorithmRunner.SubjectTime[SubjectIndex]).ToString();
                if ((from ct in db.CaThis where ct.MaCa == StepID select ct).Count() == 0)
                {
                    int BeginTime = 0;
                    for (int i = 0; i < InputHelper.Options.Times.Count; i++)
                    {
                        if (AlgorithmRunner.SubjectTime[SubjectIndex].TimeOfDay == InputHelper.Options.Times[i].BGTime.TimeOfDay)
                        {
                            BeginTime = i;
                            break;
                        }
                    }
                    db.CaThis.Add(new CaThi() { MaCa = StepID, 
                                                NgayThi = AlgorithmRunner.SubjectTime[SubjectIndex].Date,
                                                SoTiet = 3,
                                                TietBD = BeginTime});
                    db.SaveChanges();
                }
                aRecord.MaCa = StepID;
                for (int RoomIndex = 0; RoomIndex < AlgorithmRunner.SubjectRoom[SubjectIndex].Count; RoomIndex++)
                {
                    aRecord.MaPhong = AlgorithmRunner.SubjectRoom[SubjectIndex][RoomIndex].RoomID;
                    for (int StudentIndex = 0; StudentIndex < AlgorithmRunner.SubjectRoomStudents[SubjectIndex][RoomIndex].Count; StudentIndex++)
                    {
                        aRecord.MaSinhVien = AlgorithmRunner.SubjectRoomStudents[SubjectIndex][RoomIndex][StudentIndex];
                        //Thi item = new Thi() { MaCa = aRecord.MaCa, MaMonHoc = aRecord.MaMonHoc, MaPhong = aRecord.MaPhong, MaSinhVien = aRecord.MaSinhVien };
                        //db.This.Add(item);
                        var pa = new SqlParameter[] 
                        { 
                            new SqlParameter("@MaCa", SqlDbType.NVarChar) { Value = aRecord.MaCa },
                            new SqlParameter("@MaMonHoc", SqlDbType.NVarChar) { Value = aRecord.MaMonHoc },
                            new SqlParameter("@MaPhong", SqlDbType.NVarChar) { Value = aRecord.MaPhong },
                            new SqlParameter("@MaSinhVien", SqlDbType.NVarChar) { Value = aRecord.MaSinhVien }
                        };
                        db.Database.ExecuteSqlCommand("INSERT INTO Thi (MaCa, MaMonHoc, MaPhong, MaSinhVien) VALUES (@MaCa, @MaMonHoc, @MaPhong, @MaSinhVien)", pa);    
                    } // sinh viên
                } // phòng
                //db.SaveChanges();
            }// môn
        }
    }
}
