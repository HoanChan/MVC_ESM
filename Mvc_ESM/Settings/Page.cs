using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Mvc_ESM.Settings
{
    public class Page : ConfigurationSection
    {
        private static Page settings = ConfigurationManager.GetSection("PageSettings") as Page;

        public static Page Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("SelectAllString", DefaultValue = "Tất cả", IsRequired = false)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;’\"|\\", MinLength = 1, MaxLength = 25)]
        public String SelectAllString
        {
            get { return (String)this["SelectAllString"]; }
            set { this["SelectAllString"] = value; }
        }

        [ConfigurationProperty("MaxSpinnerGroupNum", DefaultValue = 10, IsRequired = false)]
        [IntegerValidator(MinValue = 5, MaxValue = 100)]
        public int MaxSpinnerGroupNum
        {
            get { return (int)this["MaxSpinnerGroupNum"]; }
            set { this["MaxSpinnerGroupNum"] = value; }
        }
    }
}