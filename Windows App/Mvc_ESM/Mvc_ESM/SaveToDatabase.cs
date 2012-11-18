using Model;
using System;
using System.Collections.Generic;
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
            //DeleteOld();
            Save();
        }
        private static void Save()
        {
            DKMHEntities db = new DKMHEntities();
            for (int GroupIndex = 0; GroupIndex < AlgorithmRunner.Groups.Count; GroupIndex++)
            {
                Thi aRecord = new Thi();
                aRecord.MaMonHoc = AlgorithmRunner.GetSubjectID(AlgorithmRunner.Groups[GroupIndex]);
                aRecord.Nhom = AlgorithmRunner.GetClassList(AlgorithmRunner.Groups[GroupIndex]);
                DateTime FirstStepTime = InputHelper.Options.StartDate.AddHours(InputHelper.Options.Times[0].Hour)
                                                                      .AddMinutes(InputHelper.Options.Times[0].Minute);
                String StepID = InputHelper.Options.StartDate.Year + "" + InputHelper.Options.StartDate.Month + "" + InputHelper.Options.StartDate.Day;
                StepID += MakeTime.CalcStep(FirstStepTime, AlgorithmRunner.GroupsTime[GroupIndex]).ToString();
                if ((from ct in db.CaThis where ct.MaCa == StepID select ct).Count() == 0)
                {
                    var pa = new SqlParameter[] 
                        { 
                            new SqlParameter("@MaCa", SqlDbType.NVarChar) { Value = StepID },
                            new SqlParameter("@GioThi", SqlDbType.DateTime) { Value = AlgorithmRunner.GroupsTime[GroupIndex] },
                        };
                    db.Database.ExecuteSqlCommand("INSERT INTO CaThi (MaCa, GioThi) VALUES (@MaCa, @GioThi)", pa);
                }
                aRecord.MaCa = StepID;
                List<SqlParameter> Param = new List<SqlParameter>();
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
