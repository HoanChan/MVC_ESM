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
    
    public partial class CaThi
    {
        public CaThi()
        {
            this.This = new HashSet<Thi>();
            this.giaoviens = new HashSet<giaovien>();
        }
    
        public string MaCa { get; set; }
        public System.DateTime NgayThi { get; set; }
        public int TietBD { get; set; }
        public int SoTiet { get; set; }
    
        public virtual ICollection<Thi> This { get; set; }
        public virtual ICollection<giaovien> giaoviens { get; set; }
    }
}
