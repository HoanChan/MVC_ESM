using System;
using System.Collections.Generic;
using System.Collections;
using Model;
using System.IO;
using System.Text;

namespace Mvc_ESM.Static_Helper
{
    
    public class InputHelper
    {
        /// <summary>
        /// danh sách môn học sẽ xếp lịch
        /// </summary>
        public static Dictionary<String, List<Class>> Subjects;
        public static Dictionary<String, List<String>> IgnoreSubjects;
        public static List<Room> Rooms;
        /// <summary>
        /// danh sách sinh viên sẽ bị cấm thi
        /// </summary>
        public static Dictionary<String, List<String>> Students;

        public static Options Options = new Options();


        public static String SaveIgnoreSubject(List<String> SubjectID, List<String> Class, List<String> Check)
        {
            InputHelper.IgnoreSubjects = new Dictionary<String, List<String>>();
            string paramInfo = "";
            for (int i = 0; i < SubjectID.Count; i++)
            {
                if (Check[i].Length > 0) // Check[i] == "checked"
                {
                    if (InputHelper.IgnoreSubjects.ContainsKey(SubjectID[i]))
                    {
                        if (!InputHelper.IgnoreSubjects[SubjectID[i]].Contains(Class[i]))
                        {
                            InputHelper.IgnoreSubjects[SubjectID[i]].Add(Class[i]);
                        }
                    }
                    else
                    {
                        InputHelper.IgnoreSubjects.Add(SubjectID[i], new List<String>() { Class[i] });
                    }
                    paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Group: " + Check[i] + "<br /><br />";
                }
            }
            Data.Subjects = Data.initSubjects();
            OutputHelper.SaveOBJ("IgnoreSubjects", InputHelper.IgnoreSubjects);
            return paramInfo;
        }
        public static String SaveGroups(List<String> SubjectID, List<String> Class, List<int> Group)
        {
            InputHelper.Subjects = new Dictionary<String, List<Class>>();
            string paramInfo = "";
            for (int i = 0; i < SubjectID.Count; i++)
            {
                Class aClass = new Class() { ClassID = Class[i], Group = Group[i] };
                if (InputHelper.Subjects.ContainsKey(SubjectID[i]))
                {
                    if (!InputHelper.Subjects[SubjectID[i]].Contains(aClass))
                    {
                        InputHelper.Subjects[SubjectID[i]].Add(aClass);
                    }
                }
                else
                {
                    InputHelper.Subjects.Add(SubjectID[i], new List<Class>() { aClass });
                }
                paramInfo += "MH:" + SubjectID[i] + " Class: " + Class[i] + " Group: " + Group[i] + "<br /><br />";
            }
            OutputHelper.SaveOBJ("Subjects", InputHelper.Subjects);
            List<String> Groups = new List<String>();
            foreach (String Subject in InputHelper.Subjects.Keys)
            {
                Boolean[] Progressed = new Boolean[InputHelper.Subjects[Subject].Count];
                for (int i = 0; i < InputHelper.Subjects[Subject].Count; i++)
                {
                    if (!Progressed[i])
                    {
                        String GroupItem = Subject;
                        for (int j = 0; j < InputHelper.Subjects[Subject].Count; j++)
                        {
                            if (InputHelper.Subjects[Subject][i].Group == InputHelper.Subjects[Subject][j].Group)
                            {
                                Progressed[j] = true;
                                GroupItem += "_" + InputHelper.Subjects[Subject][j].ClassID;
                            }
                        }
                        if (!Groups.Contains(GroupItem))
                        {
                            Groups.Add(GroupItem);
                        }
                    }
                }
            }
            OutputHelper.SaveOBJ("Groups", Groups);
            return paramInfo;
        }
    }
}