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
    
    public partial class khoa
    {
        public khoa()
        {
            this.bomons = new HashSet<bomon>();
            this.chuyennganhs = new HashSet<chuyennganh>();
            this.khois = new HashSet<khoi>();
            this.monhocs = new HashSet<monhoc>();
            this.phongs = new HashSet<phong>();
        }
    
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }
    
        public virtual ICollection<bomon> bomons { get; set; }
        public virtual ICollection<chuyennganh> chuyennganhs { get; set; }
        public virtual ICollection<khoi> khois { get; set; }
        public virtual ICollection<monhoc> monhocs { get; set; }
        public virtual ICollection<phong> phongs { get; set; }
    }
}
