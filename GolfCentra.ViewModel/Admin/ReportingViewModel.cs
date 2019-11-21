using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    public class ReportingViewModel
    {
        public long GolferId { get; set; }
        public long BookingId { get; set; }
        public string Time { get; set; }
        public string TeeOffDate { get; set; }
        public string CoursePairingName { get; set; }
        public string BookingStatus { get; set; }
       
        public decimal CartFee { get; set; }
        public decimal CaddieFee { get; set; }
        public decimal GreenFee { get; set; }
        public decimal RangeFee { get; set; }
        public decimal BallFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
        public bool OnSpot { get; set; }
        public int NoOfCart { get; set; }
        public int NoOfCaddie { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Player3 { get; set; }
        public string Player4 { get; set; }
        public string CurrencyName { get; set; }
        public int NoOfPlayer { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
        public DateTime BookingDate { get; set; }
        //Search
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CoursePairingId { get; set; }
        public long MemberTypeId { get; set; }
        public long BookingStatusId { get; set; }
        public long PackageId { get; set; }
        public long SessionTypeId { get; set; }
        public string MembershipId { get; set; }
    }
}
