using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For SaveBooking's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class SaveBookingViewModel
    {

        public long BookingId { get; set; }

        public long BookingTypeId { get; set; }
        public long SlotId { get; set; }
        public DateTime Date { get; set; }

        public long HoleTypeId { get; set; }
        public int NoOfPlayer { get; set; }
        public int NoOfMemberPlayer { get; set; }
        public int NoOfNonMemberPlayer { get; set; }
        public int NoOfHole { get; set; }
        public long GolferId { get; set; }
        public System.DateTime BookingDate { get; set; }
        public System.DateTime TeeOffDate { get; set; }
        public string TeeOffSlot { get; set; }
        public long BookingStatusId { get; set; }

        public Nullable<decimal> CartFee { get; set; }
        public int CartCount { get; set; }
        public Nullable<decimal> CaddieFee { get; set; }
        public int CaddieCount { get; set; }
        public Nullable<decimal> GreenFee { get; set; }
        public Nullable<decimal> RangeFee { get; set; }
        public Nullable<decimal> BallFee { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> FeeAndTaxes { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<bool> OnSpot { get; set; }
        public int NoOfBalls { get; set; }

        public int BucketId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public long? CoursePairingId { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }
        public List<MemberTypeViewModel> MemberTypeViewModels { get; set; }
        public List<BookingEquipmentMappingViewModel> BookingEquipmentMappingViewModels { get; set; }
        public List<BookingPlayerDetailViewModel> BookingPlayerDetailViewModels { get; set; }
    }
}
