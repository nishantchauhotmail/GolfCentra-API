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
    
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            this.BookingCourseTaxMappings = new HashSet<BookingCourseTaxMapping>();
            this.BookingEquipmentMappings = new HashSet<BookingEquipmentMapping>();
            this.BookingPlayerDetails = new HashSet<BookingPlayerDetail>();
            this.BookingPlayerMappings = new HashSet<BookingPlayerMapping>();
            this.Scores = new HashSet<Score>();
        }
    
        public long BookingId { get; set; }
        public long GolferId { get; set; }
        public System.DateTime BookingDate { get; set; }
        public Nullable<long> HoleTypeId { get; set; }
        public System.DateTime TeeOffDate { get; set; }
        public string TeeOffSlot { get; set; }
        public long BookingStatusId { get; set; }
        public Nullable<int> NoOfMember { get; set; }
        public Nullable<int> NoOfNonMember { get; set; }
        public int NoOfPlayer { get; set; }
        public Nullable<decimal> CartFee { get; set; }
        public Nullable<int> CartCount { get; set; }
        public Nullable<decimal> CaddieFee { get; set; }
        public Nullable<int> CaddieCount { get; set; }
        public Nullable<decimal> GreenFee { get; set; }
        public Nullable<decimal> RangeFee { get; set; }
        public Nullable<int> NoOfBalls { get; set; }
        public Nullable<decimal> BallFee { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> FeeAndTaxes { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<bool> OnSpot { get; set; }
        public long BookingTypeId { get; set; }
        public Nullable<long> CouponId { get; set; }
        public string PaymentGatewayBookingId { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<long> PaymentStatusId { get; set; }
        public Nullable<long> CurrencyId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> CoursePairingId { get; set; }
        public Nullable<long> ScoreId { get; set; }
        public Nullable<long> PromotionId { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<bool> IsPaymentGatewaySkip { get; set; }
    
        public virtual BookingStatu BookingStatu { get; set; }
        public virtual BookingType BookingType { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual CoursePairing CoursePairing { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Golfer Golfer { get; set; }
        public virtual HoleType HoleType { get; set; }
        public virtual PaymentStatu PaymentStatu { get; set; }
        public virtual Promotion Promotion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingCourseTaxMapping> BookingCourseTaxMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingEquipmentMapping> BookingEquipmentMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingPlayerDetail> BookingPlayerDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingPlayerMapping> BookingPlayerMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Score> Scores { get; set; }
    }
}
