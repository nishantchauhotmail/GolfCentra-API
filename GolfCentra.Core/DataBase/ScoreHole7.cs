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
    
    public partial class ScoreHole7
    {
        public long ScoreHole7Id { get; set; }
        public long ScoreId { get; set; }
        public Nullable<int> Storkes { get; set; }
        public Nullable<int> Putts { get; set; }
        public Nullable<int> Drive { get; set; }
        public Nullable<int> Clubs { get; set; }
        public Nullable<int> Chips { get; set; }
        public Nullable<int> Sand { get; set; }
        public Nullable<int> Saves { get; set; }
        public Nullable<int> Penalty { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string HoleName { get; set; }
        public Nullable<long> Par { get; set; }
    
        public virtual Score Score { get; set; }
    }
}