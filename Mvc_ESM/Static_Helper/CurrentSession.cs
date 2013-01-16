using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
namespace Mvc_ESM.Static_Helper
{
    public class CurrentSession
    {
        private static Dictionary<String, Object> Data = new Dictionary<string,object>();
        public static Object Get(String Name)
        {
            if (Data.ContainsKey(HttpContext.Current.User.Identity.Name + Name))
            {
                return Data[HttpContext.Current.User.Identity.Name + Name];
            }
            else
            {
                return null;
            }
        }
        public static void Set(String Name, Object Value)
        {
            if (Data.ContainsKey(HttpContext.Current.User.Identity.Name + Name))
            {
                Data[HttpContext.Current.User.Identity.Name + Name] = Value;
            }
            else
            {
                Data.Add(HttpContext.Current.User.Identity.Name + Name, Value);
            }
        }
        public static void Reset(String Name)
        {
            if (Data.ContainsKey(HttpContext.Current.User.Identity.Name + Name))
            {
                Data.Remove(HttpContext.Current.User.Identity.Name + Name);
            }
        }
    }
}