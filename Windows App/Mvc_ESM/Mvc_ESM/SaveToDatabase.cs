using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Mvc_ESM.Static_Helper
{
    class SaveToDatabase
    {   
        public static void Run()
        {
            Save();
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
                        db.This.Add(aRecord);
                        
                    } // sinh viên
                } // phòng
                db.SaveChanges();
            }// môn
        }
    }
}
