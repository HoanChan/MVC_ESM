using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Collections.Specialized;
using System.Collections;
using Mvc_ESM.Static_Helper;
using System.Text;

namespace Mvc_ESM.Controllers
{ 
    public class SelectSubjectController : Controller
    {
        private DKMHEntities db = new DKMHEntities();

        //
        // GET: /SelectSubject/
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public String SelectSuccess(List<String> SubjectID, List<String> Class, List<int> Group)
        {
            InputHelper.Subjects = new Dictionary<String,List<Class>>();
            string paramInfo = "";
            for (int i = 0; i < SubjectID.Count; i++)
            {
                if (InputHelper.Subjects.ContainsKey(SubjectID[i]))
                {
                    InputHelper.Subjects[SubjectID[i]].Add(new Class() { ClassID = Class[i], Group = Group[i] });
                }
                else
                {
                    InputHelper.Subjects.Add(SubjectID[i], new List<Class>() { new Class() { ClassID = Class[i], Group = Group[i] } });
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
                        Groups.Add(GroupItem);
                    }
                }
            }
            OutputHelper.SaveOBJ("Groups", Groups);
            return paramInfo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}