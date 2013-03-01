using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace Mvc_ESM.Static_Helper
{
    class Handmade
    {
        public class HandmadeData
        {
            public String SubjectData { get; set; }
            public List<String> Class { get; set; }
            public DateTime Date { get; set; }
            public List<String> Room { get; set; }
            public List<int> Num { get; set; }
        }
        class StudentInfo
        {
            public string MaSinhVien { get; set; }
            public byte Nhom { get; set; }
        }
        public static void Run()
        {
            AlgorithmRunner.IsBusy = true;
            AlgorithmRunner.SaveOBJ("Status", "inf Đang lưu dữ liệu xếp lịch thủ công");
            Save(AlgorithmRunner.HandmadeData);
            AlgorithmRunner.SaveOBJ("Status", "inf Hoàn tất lưu dữ liệu xếp lịch thủ công");
            AlgorithmRunner.IsBusy = false;
        }
        public static void Save(HandmadeData Data)
        {
            DKMHEntities db = new DKMHEntities();
            var ClassList = "";
            foreach (String cl in Data.Class)
            {
                ClassList += (ClassList.Length > 0 ? ", " : "") + "'" + cl + "'";
            }
            String IgnoreStudents = InputHelper.IgnoreStudents.ContainsKey(Data.SubjectData) ? JsonConvert.SerializeObject(InputHelper.IgnoreStudents[Data.SubjectData]) : "[]";
            IgnoreStudents = IgnoreStudents.Substring(1, IgnoreStudents.Length - 2).Replace("\"", "'");
            var StudentList = db.Database.SqlQuery<StudentInfo>("select pdkmh.MaSinhVien, pdkmh.Nhom from pdkmh, sinhvien " +
                                                                "where pdkmh.MaSinhVien = sinhvien.MaSinhVien and MaMonHoc = '" + Data.SubjectData + "' and Nhom in (" + ClassList + ") " +
                                                                (IgnoreStudents.Length > 0 ? "and not(sinhvien.MaSinhVien in (" + IgnoreStudents + ")) " : "") +
                                                                "order by (sinhvien.Ten + sinhvien.ho)").ToList();

            DateTime FirstShiftTime = InputHelper.Options.StartDate.AddHours(InputHelper.Options.Times[0].Hour)
                                                                      .AddMinutes(InputHelper.Options.Times[0].Minute);
            String ShiftID = "";//InputHelper.Options.StartDate.Year + "" + InputHelper.Options.StartDate.Month + "" + InputHelper.Options.StartDate.Day;
            ShiftID += RoomArrangement.CalcShift(FirstShiftTime, Data.Date).ToString();
            if ((from ct in db.CaThis where ct.MaCa == ShiftID select ct).Count() == 0)
            {
                var pa = new SqlParameter[] 
                        { 
                            new SqlParameter("@MaCa", SqlDbType.NVarChar) { Value = ShiftID },
                            new SqlParameter("@GioThi", SqlDbType.DateTime) { Value = Data.Date },
                        };
                db.Database.ExecuteSqlCommand("INSERT INTO CaThi (MaCa, GioThi) VALUES (@MaCa, @GioThi)", pa);
            }
            Thi aRecord = new Thi();
            aRecord.MaMonHoc = Data.SubjectData;
            aRecord.MaCa = ShiftID;
            var ClassGroup = Data.SubjectData;
            foreach (var cl in Data.Class)
            {
                ClassGroup += "_" + cl;
            }
            aRecord.Nhom = ClassGroup;
            String SQLQuery = "";
            int StudentIndex = 0;
            for (int Index = 0; Index < Data.Room.Count; Index++)
            {
                aRecord.MaPhong = Data.Room[Index];
                for (int i = 0; i < Data.Num[Index]; i++)
                {
                    aRecord.MaSinhVien = StudentList[StudentIndex].MaSinhVien;
                    SQLQuery += String.Format("INSERT INTO Thi (MaCa, MaMonHoc, Nhom, MaPhong, MaSinhVien) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')\r\n",
                                                aRecord.MaCa,
                                                aRecord.MaMonHoc,
                                                aRecord.Nhom,
                                                aRecord.MaPhong,
                                                aRecord.MaSinhVien
                                            );
                    StudentIndex++;
                }
            } 
            db.Database.ExecuteSqlCommand(SQLQuery);  
        }
    }
}
