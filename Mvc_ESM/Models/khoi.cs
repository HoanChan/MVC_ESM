//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mvc_ESM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class khoi
    {
        public khoi()
        {
            this.lops = new HashSet<lop>();
        }
    
        public string MaKhoi { get; set; }
        public Nullable<short> SiSo { get; set; }
        public string BacHoc { get; set; }
        public Nullable<short> KhoaHoc { get; set; }
        public string MaNganh { get; set; }
        public string KhoaQL { get; set; }
    
        public virtual khoa khoa { get; set; }
        public virtual ICollection<lop> lops { get; set; }
    }
}
