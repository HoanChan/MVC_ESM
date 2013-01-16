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

        [ConfigurationProperty("GroupMaxSpinnerNum", DefaultValue = 10, IsRequired = false)]
        [IntegerValidator(MinValue = 5, MaxValue = 100)]
        public int GroupMaxSpinnerNum
        {
            get { return (int)this["GroupMaxSpinnerNum"]; }
            set { this["GroupMaxSpinnerNum"] = value; }
        }

        [ConfigurationProperty("GroupMinSpinnerNum", DefaultValue = 1, IsRequired = false)]
        [IntegerValidator(MinValue = 0, MaxValue = 5)]
        public int GroupMinSpinnerNum
        {
            get { return (int)this["GroupMinSpinnerNum"]; }
            set { this["GroupMinSpinnerNum"] = value; }
        }

        [ConfigurationProperty("OptionMaxNumDate", DefaultValue = 200, IsRequired = false)]
        [IntegerValidator(MinValue = 7, MaxValue = 365)]
        public int OptionMaxNumDate
        {
            get { return (int)this["OptionMaxNumDate"]; }
            set { this["OptionMaxNumDate"] = value; }
        }

        [ConfigurationProperty("OptionMinNumDate", DefaultValue = 7, IsRequired = false)]
        [IntegerValidator(MinValue = 1, MaxValue = 365)]
        public int OptionMinNumDate
        {
            get { return (int)this["OptionMinNumDate"]; }
            set { this["OptionMinNumDate"] = value; }
        }

        [ConfigurationProperty("OptionMaxDateMin", DefaultValue = 5, IsRequired = false)]
        [IntegerValidator(MinValue = 1, MaxValue = 30)]
        public int OptionMaxDateMin
        {
            get { return (int)this["OptionMaxDateMin"]; }
            set { this["OptionMaxDateMin"] = value; }
        }

        [ConfigurationProperty("OptionMinDateMin", DefaultValue = 0, IsRequired = false)]
        [IntegerValidator(MinValue = 0, MaxValue = 7)]
        public int OptionMinDateMin
        {
            get { return (int)this["OptionMinDateMin"]; }
            set { this["OptionMinDateMin"] = value; }
        }


        [ConfigurationProperty("OptionMaxShiftTime", DefaultValue = 320, IsRequired = false)]
        [IntegerValidator(MinValue = 90, MaxValue = 720)]
        public int OptionMaxShiftTime
        {
            get { return (int)this["OptionMaxShiftTime"]; }
            set { this["OptionMaxShiftTime"] = value; }
        }

        [ConfigurationProperty("OptionMinShiftTime", DefaultValue = 60, IsRequired = false)]
        [IntegerValidator(MinValue = 30, MaxValue = 90)]
        public int OptionMinShiftTime
        {
            get { return (int)this["OptionMinShiftTime"]; }
            set { this["OptionMinShiftTime"] = value; }
        }

        [ConfigurationProperty("OptionStepShiftTime", DefaultValue = 5, IsRequired = false)]
        [IntegerValidator(MinValue = 1, MaxValue = 30)]
        public int OptionStepShiftTime
        {
            get { return (int)this["OptionStepShiftTime"]; }
            set { this["OptionStepShiftTime"] = value; }
        }
    }
}