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
    
    public partial class HoleTeeYardage
    {
        public long HoleTeeYardageId { get; set; }
        public long HoleNumberId { get; set; }
        public long TeeId { get; set; }
        public decimal Yardage { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
    
        public virtual HoleNumber HoleNumber { get; set; }
        public virtual Tee Tee { get; set; }
    }
}
