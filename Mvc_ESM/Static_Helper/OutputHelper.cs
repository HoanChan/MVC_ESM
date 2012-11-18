using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace Mvc_ESM.Static_Helper
{
    public class OutputHelper
    {
        public static void SaveOBJ(String Name, Object OBJ)
        {
            System.IO.File.WriteAllText(
                Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Win_App"), Name + ".jso"),
                JsonConvert.SerializeObject(OBJ, Formatting.Indented),
                Encoding.UTF8
            );            
        }
    }
}