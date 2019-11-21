using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.Core.Helper;
using GolfCentra.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static GolfCentra.Core.Helper.Enum;

namespace GolfCentra.Business.Business.Implementation
{
   public class CreateBookingService
    {
        private readonly UnitOfWork _unitOfWork;

        public CreateBookingService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public BookingViewModel SaveTeeTimeBooking(CommonViewModel commonViewModel)
        {
            Core.Helper.Vaildation.TimeSpanValidation(commonViewModel.TeeOffSlot);
            CheckBookingForGolfer(commonViewModel.BookingPlayerDetailViewModels, commonViewModel.TeeOffDate, commonViewModel.BookingTypeId);
            if (commonViewModel.TeeOffDate > DateHelper.ConvertSystemDate().AddDays(6))
                throw new Exception("You Can Book Only Upto 6 Days");
            if (commonViewModel.NoOfPlayer != commonViewModel.MemberTypeViewModels.Sum(x => x.PlayerCount))
                throw new Exception("Invalid Player Details");
            if (IsSlotAvailable(commonViewModel.TeeOffSlot, (int)EnumBookingType.BTT, commonViewModel.TeeOffDate, commonViewModel.NoOfPlayer, commonViewModel.CoursePairingId) == false)
                throw new Exception("Slot Is Already Booked");


            Booking booking = new Booking()
            {
                NoOfPlayer = commonViewModel.NoOfPlayer,
                Discount = commonViewModel.Discount,
                CaddieCount = commonViewModel.CaddieCount,
                CartCount = commonViewModel.CartCount,
                CaddieFee = commonViewModel.CaddieFee,
                CartFee = commonViewModel.CartFee,
                BookingStatusId = commonViewModel.BookingStatusId,
                GreenFee = commonViewModel.GreenFee,
                HoleTypeId = commonViewModel.HoleTypeId,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                BookingTypeId = (int)EnumBookingType.BTT,
                FeeAndTaxes = commonViewModel.FeeAndTaxes,
                TeeOffSlot = commonViewModel.TeeOffSlot,
                TeeOffDate = commonViewModel.TeeOffDate,
                OnSpot = commonViewModel.OnSpot,
                GolferId = commonViewModel.GolferId,
                BookingDate = DateTime.UtcNow,
                Amount = commonViewModel.Amount,
                PaymentStatusId = commonViewModel.PaymentStatusId,
                CoursePairingId = commonViewModel.CoursePairingId,
                
            };
            _unitOfWork.BookingRepository.Insert(booking);
            _unitOfWork.Save();
            Booking bk = _unitOfWork.BookingRepository.Get(x => x.BookingId == booking.BookingId);
            bk.PaymentGatewayBookingId = Core.Helper.Constants.BookingKey.BookingKeyId + booking.BookingId.ToString();
            _unitOfWork.BookingRepository.Update(bk);
            _unitOfWork.Save();
            SaveMemberTypePlayerMapping(booking.BookingId, commonViewModel.MemberTypeViewModels);
            if (commonViewModel.PromotionsId != null && commonViewModel.PromotionsId != 0)
            {
                SaveCourseTaxMapping(booking.BookingId, commonViewModel.CourseTaxMappingViewModels);
                 SaveBookingEquipmentMappingMapping(booking.BookingId, commonViewModel.BookingEquipmentMappingViewModels);
            }
            SaveBookingPlayerDetail(booking.BookingId, commonViewModel.BookingPlayerDetailViewModels);
            CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == booking.CoursePairingId);

            BookingViewModel bookingViewModel = new BookingViewModel()
            {
                BookingId = booking.BookingId,
                PaymentGatewayBookingId = booking.PaymentGatewayBookingId,
                TotalAmount = bk.Amount.GetValueOrDefault(),
                ConvertedAmount = ConvertCurrency(bk.Amount.GetValueOrDefault()),
                CoursePairingId = bk.CoursePairingId.GetValueOrDefault(),
                CoursePairingName = coursePairing != null ? (coursePairing.CourseName1.Value) + (coursePairing.EndCourseNameId != null ? " - " + coursePairing.CourseName.Value : "") : ""

            };
            return bookingViewModel;
        }

        private void SaveMemberTypePlayerMapping(long bookingId, List<MemberTypeViewModel> memberTypeViewModels)
        {
            foreach (var memberType in memberTypeViewModels)
            {
                if (memberType.PlayerCount != 0)
                {
                    BookingPlayerMapping bookingPlayerMapping = new BookingPlayerMapping()
                    {
                        BookingId = bookingId,
                        PlayerCount = memberType.PlayerCount,
                        MemberTypeId = memberType.MemberTypeId,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow,
                        MemberTypeName = memberType.Name

                    };
                    _unitOfWork.BookingPlayerMappingRepository.Insert(bookingPlayerMapping);
                }
            }
            _unitOfWork.Save();
        }

        /// <summary>
        /// Save Course Tax Mapping For Booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="courseTaxMappingViewModels"></param>
        private void SaveCourseTaxMapping(long bookingId, List<CourseTaxMappingViewModel> courseTaxMappingViewModels)
        {
            foreach (var tax in courseTaxMappingViewModels)
            {

                BookingCourseTaxMapping bookingCourseTaxMapping = new BookingCourseTaxMapping()
                {
                    BookingId = bookingId,
                    TaxId = tax.TaxId,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    TaxName = tax.Name,
                    Percentage = tax.Percentage

                };
                _unitOfWork.BookingCourseTaxMappingRepository.Insert(bookingCourseTaxMapping);

            }
            _unitOfWork.Save();
        }

        private void SaveBookingEquipmentMappingMapping(long bookingId, List<BookingEquipmentMappingViewModel> bookingEquipmentMappingViewModels)
        {
            foreach (var bookingEquipmentMappingViewModel in bookingEquipmentMappingViewModels)
            {
                if (bookingEquipmentMappingViewModel.EquipmentCount != 0)
                {
                    BookingEquipmentMapping bookingEquipmentMapping = new BookingEquipmentMapping()
                    {
                        BookingId = bookingId,
                        EquipmentCount = bookingEquipmentMappingViewModel.EquipmentCount,
                        EquipmentId = bookingEquipmentMappingViewModel.EquipmentId,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow,
                        EquipmentName = bookingEquipmentMappingViewModel.EquipmentName,
                        EquipmentPrice = bookingEquipmentMappingViewModel.EquipmentPrice,

                    };
                    _unitOfWork.BookingEquipmentMappingRepository.Insert(bookingEquipmentMapping);

                }
            }
            _unitOfWork.Save();
        }

        private void SaveBookingPlayerDetail(long bookingId, List<BookingPlayerDetailViewModel> bookingPlayerDetailViewModels)
        {
            if (bookingPlayerDetailViewModels != null)
            {
                BookingPlayerDetail bookingPlayerDetail = new BookingPlayerDetail
                {
                    BookingId = bookingId,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow
                };

                foreach (var playerDetail in bookingPlayerDetailViewModels)
                {
                    if (playerDetail.PlayerSerialNumber.Trim() == "Player1")
                    {
                        bookingPlayerDetail.Player1 = playerDetail.PlayerDetails;
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player2")
                    {
                        bookingPlayerDetail.Player2 = playerDetail.PlayerDetails;
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player3")
                    {
                        bookingPlayerDetail.Player3 = playerDetail.PlayerDetails;
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player4")
                    {
                        bookingPlayerDetail.Player4 = playerDetail.PlayerDetails;
                    }


                }
                _unitOfWork.BookingPlayerDetailRepository.Insert(bookingPlayerDetail);
                _unitOfWork.Save();
            }
        }

        private void CheckBookingForGolfer(List<BookingPlayerDetailViewModel> bookingPlayerDetailViewModels, DateTime teeOffDate, long bookingTypeId)
        {
            if (bookingPlayerDetailViewModels != null)
            {

                foreach (var playerDetail in bookingPlayerDetailViewModels)
                {
                    if (playerDetail.PlayerSerialNumber.Trim() == "Player1")
                    {
                        int bookingCount = 0;
                        var golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == playerDetail.PlayerDetails);
                        if (golfer != null)
                        {
                            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                            foreach (var golf in golferBookings1)
                            {
                                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                                {
                                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                                        bookingCount++;
                                }
                            }
                        }
                        var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == playerDetail.PlayerDetails && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        //Max Booking Allowed Per User
                        int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                        if (bookingLeft <= 0)
                            throw new Exception("Max Two Booking Per Day.Please Check Golfer 1");
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player2")
                    {
                        int bookingCount = 0;
                        var golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == playerDetail.PlayerDetails);
                        if (golfer != null)
                        {
                            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                            foreach (var golf in golferBookings1)
                            {
                                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                                {
                                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                                        bookingCount++;
                                }
                            }
                        }
                        var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == playerDetail.PlayerDetails && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        //Max Booking Allowed Per User
                        int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                        if (bookingLeft <= 0)
                            throw new Exception("Max Two Booking Per Day.Please Check Golfer 2");
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player3")
                    {
                        int bookingCount = 0;
                        var golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == playerDetail.PlayerDetails);
                        if (golfer != null)
                        {
                            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                            foreach (var golf in golferBookings1)
                            {
                                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                                {
                                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                                        bookingCount++;
                                }
                            }
                        }
                        var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == playerDetail.PlayerDetails && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        //Max Booking Allowed Per User
                        int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                        if (bookingLeft <= 0)
                            throw new Exception("Max Two Booking Per Day.Please Check Golfer 3");
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player4")
                    {
                        int bookingCount = 0;
                        var golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == playerDetail.PlayerDetails);
                        if (golfer != null)
                        {
                            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                            foreach (var golf in golferBookings1)
                            {
                                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                                {
                                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                                        bookingCount++;
                                }
                            }
                        }
                        var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == playerDetail.PlayerDetails && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        //Max Booking Allowed Per User
                        int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                        if (bookingLeft <= 0)
                            throw new Exception("Max Two Booking Per Day.Please Check Golfer 4");
                    }


                }

            }
        }

        private decimal ConvertCurrency(decimal amount)
        {
            decimal convertedAmount = 0;
            if (Core.Helper.Constants.Currency.CurrencyExchangeEnable)
            {
                CurrencyExchangeViewModel model = new CurrencyExchangeViewModel();
                CommonViewModel commonViewModel = new CommonViewModel()
                {
                    baseCurrency = Core.Helper.Constants.Currency.CurrencyName,
                    currencyISO = Core.Helper.Constants.Currency.ConvertedCurrencyName
                };
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Core.Helper.Constants.Currency.ConvertCurrencyBaseURL)
                };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage response = client.PostAsJsonAsync(Core.Helper.Constants.Currency.ConvertCurrencyRelativeURL, commonViewModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<ViewModel.CurrencyExchangeViewModel>(data);
                }
                convertedAmount = amount * model.Rate;
            }
            else
            {
                convertedAmount = amount;
            }
            return convertedAmount;
        }

        private bool IsSlotAvailable(string slotTime, long bookingTypeId, DateTime teeOffDate, int noOfPlayerBooking, long coursePairingId)
        {
            List<Booking> bookings = new List<Booking>();
            if (bookingTypeId == (int)EnumBookingType.BTT)
            {
                long coursePairingId1 = 0;
                if (coursePairingId == 1) { coursePairingId1 = 4; }
                if (coursePairingId == 2) { coursePairingId1 = 5; }
                if (coursePairingId == 3) { coursePairingId1 = 6; }
                if (coursePairingId == 4) { coursePairingId1 = 1; }
                if (coursePairingId == 5) { coursePairingId1 = 2; }
                if (coursePairingId == 6) { coursePairingId1 = 3; }
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.TeeOffSlot == slotTime && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow)) && (x.CoursePairingId == coursePairingId || x.CoursePairingId == coursePairingId1)).ToList();

            }
            else
            {
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.TeeOffSlot == slotTime && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();

            }
            DateTime currentTime = Core.Helper.DateHelper.ConvertSystemDate();
            int noOfPlayer = bookings.Select(x => x.NoOfPlayer).Sum();
            int playerLeft = 4 - (noOfPlayer + noOfPlayerBooking);
            if (noOfPlayerBooking <= 4 && playerLeft >= 0)
            {
                return true;
            }
            return false;
        }

    }
}
