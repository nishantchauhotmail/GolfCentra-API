using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Booking's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class BookingViewModel
    {
        public BookingViewModel()
        {
            BookingId = 0;
            Time = string.Empty;
            Date = string.Empty;
            NoOfPlayer = 0;
            NoOfBalls = 0;
            NoOfHole = 0;
            TotalAmount = 0;
            CurrencyName = string.Empty;
            PaymentGatewayBookingId = string.Empty;
            ConvertedAmount = 0;
            BookingTypeId = 0;
            CoursePairingId = 0;
            CoursePairingName = string.Empty;
            BookedBy = string.Empty;
            CanCancelBooking = false;
            CanSubmitScore = false;
            BookingPlayerDetailViewModels = new List<BookingPlayerDetailViewModel>();
            IsUpcomingBookiong = false;
            BookingStatusId = 0;
            TeeOffDate = new DateTime();
            ENB = string.Empty;
            BookingStatus = string.Empty;
        }

        public long BookingId { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public int NoOfPlayer { get; set; }
        public long BookingTypeId { get; set; }
        public int NoOfBalls { get; set; }
        public int NoOfHole { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyName { get; set; }
        public string PaymentGatewayBookingId { get; set; }
        public decimal ConvertedAmount { get; set; }
        public long CoursePairingId { get; set; }
        public String CoursePairingName { get; set; }
        public String BookedBy { get; set; }
        public bool CanCancelBooking { get; set; }
        public bool CanSubmitScore { get; set; }

        public List<BookingPlayerDetailViewModel> BookingPlayerDetailViewModels { get; set; }

        public bool IsUpcomingBookiong { get; set; }
        public long BookingStatusId { get; set; }
        public DateTime TeeOffDate { get; set; }
        public string ENB { get; set; }
        public string BookingStatus { get; set; }
    }
}
