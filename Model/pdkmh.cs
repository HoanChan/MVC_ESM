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
    
    public partial class pdkmh
    {
        public string MaSinhVien { get; set; }
        public string MaMonHoc { get; set; }
        public short Dot { get; set; }
        public byte Nhom { get; set; }
        public byte TinhTrang { get; set; }
    
        public virtual nhom nhom1 { get; set; }
        public virtual sinhvien sinhvien { get; set; }
    }
}