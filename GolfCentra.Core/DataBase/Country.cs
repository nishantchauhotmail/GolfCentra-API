//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GolfCentra.Core.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Country
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ISO { get; set; }
        public Nullable<int> NumCode { get; set; }
        public int PhoneCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Nicename { get; set; }
    }
}