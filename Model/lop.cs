//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class lop
    {
        public lop()
        {
            this.sinhviens = new HashSet<sinhvien>();
        }
    
        public string MaLop { get; set; }
        public string CVHT { get; set; }
        public Nullable<short> Siso { get; set; }
        public string MaKhoi { get; set; }
        public string MaLopDT { get; set; }
    
        public virtual khoi khoi { get; set; }
        public virtual ICollection<sinhvien> sinhviens { get; set; }
    }
}
