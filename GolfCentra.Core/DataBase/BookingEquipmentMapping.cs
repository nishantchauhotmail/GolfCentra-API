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
    
    public partial class BookingEquipmentMapping
    {
        public long BookingEquipmentMappingId { get; set; }
        public long BookingId { get; set; }
        public long EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public Nullable<decimal> EquipmentPrice { get; set; }
        public int EquipmentCount { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual Booking Booking { get; set; }
        public virtual Equipment Equipment { get; set; }
    }
}
