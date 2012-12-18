using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mvc_ESM.Static_Helper
{
    class SaveToDatabase
    {
        private static DKMHEntities db = new DKMHEntities();
        public static void Run()
        {
            DeleteOld();
            Save();
        }

        public static void DeleteOld()
        {
            db.Database.ExecuteSqlCommand("DELETE FROM Thi");
            db.Database.ExecuteSqlCommand("DELETE FROM CaThi");
            var DbName = Regex.Match(db.Database.Connection.ConnectionString, "initial\\scatalog=([^;]+)").Groups[1].Value;
            db.Database.ExecuteSqlCommand("DBCC SHRINKFILE (" + DbName + ", 1) ");
            db.Database.ExecuteSqlCommand("DBCC SHRINKFILE (" + DbName + "_log, 1) ");
        }
        private static void Save()
        {
            for (int GroupIndex = 0; GroupIndex < AlgorithmRunner.Groups.Count; GroupIndex++)
            {
                Thi aRecord = new Thi();
                aRecord.MaMonHoc = AlgorithmRunner.GetSubjectID(AlgorithmRunner.Groups[GroupIndex]);
                aRecord.Nhom = AlgorithmRunner.GetClassList(AlgorithmRunner.Groups[GroupIndex]);
                DateTime FirstShiftTime = InputHelper.Options.StartDate.AddHours(InputHelper.Options.Times[0].Hour)
                                                                      .AddMinutes(InputHelper.Options.Times[0].Minute);
                String ShiftID = "";// InputHelper.Options.StartDate.Year + "" + InputHelper.Options.StartDate.Month + "" + InputHelper.Options.StartDate.Day;
                ShiftID += MakeTime.CalcShift(FirstShiftTime, AlgorithmRunner.GroupsTime[GroupIndex]).ToString();
                if ((from ct in db.CaThis where ct.MaCa == ShiftID select ct).Count() == 0)
                {
                    var pa = new SqlParameter[] 
                        { 
                            new SqlParameter("@MaCa", SqlDbType.NVarChar) { Value = ShiftID },
                            new SqlParameter("@GioThi", SqlDbType.DateTime) { Value = AlgorithmRunner.GroupsTime[GroupIndex] },
                        };
                    db.Database.ExecuteSqlCommand("INSERT INTO CaThi (MaCa, GioThi) VALUES (@MaCa, @GioThi)", pa);
                }
                aRecord.MaCa = ShiftID;
                String SQLQuery = "";
                for (int RoomIndex = 0; RoomIndex < AlgorithmRunner.GroupsRoom[GroupIndex].Count; RoomIndex++)
                {
                    aRecord.MaPhong = AlgorithmRunner.GroupsRoom[GroupIndex][RoomIndex].RoomID;
                    for (int StudentIndex = 0; StudentIndex < AlgorithmRunner.GroupsRoomStudents[GroupIndex][RoomIndex].Count; StudentIndex++)
                    {
                        aRecord.MaSinhVien = AlgorithmRunner.GroupsRoomStudents[GroupIndex][RoomIndex][StudentIndex];
                        SQLQuery += String.Format("INSERT INTO Thi (MaCa, MaMonHoc, Nhom, MaPhong, MaSinhVien) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')\r\n", 
                                                    aRecord.MaCa, 
                                                    aRecord.MaMonHoc, 
                                                    aRecord.Nhom, 
                                                    aRecord.MaPhong, 
                                                    aRecord.MaSinhVien
                                                );
                    } // sinh viên
                } // phòng
                db.Database.ExecuteSqlCommand(SQLQuery);  
                //db.SaveChanges();
            }// môn
        }
    }
}
