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
    
    public partial class UsersRoles
    {
        public string User { get; set; }
        public string Role { get; set; }
        public string Detail { get; set; }
    
        public virtual Role Roles { get; set; }
        public virtual User Users { get; set; }
    }
}
