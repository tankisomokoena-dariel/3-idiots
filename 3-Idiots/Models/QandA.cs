//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _3_Idiots.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class QandA
    {
        public int qaID { get; set; }
        public Nullable<int> userID { get; set; }
        public string answer { get; set; }
        public string question { get; set; }
    
        public virtual userTable userTable { get; set; }
    }
}