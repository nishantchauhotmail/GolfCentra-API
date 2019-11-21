using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class PricingService : IPricingService
    {
        private readonly UnitOfWork _unitOfWork;

        public PricingService()
        {
            _unitOfWork = new UnitOfWork();
        }
        #region old Pricing 
        /// <summary>
        /// Save Pricing Detail
        /// </summary>
        /// <param name="updatePricingViewModel"></param>
        public void SavePricing(UpdatePricingViewModel updatePricingViewModel)
        {

            if (updatePricingViewModel.SlotId == 0)
            {
                updatePricingViewModel.SlotId = null;
            }
            //9 hole
            //weekday
            //  WDHole9Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            WDHole18Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            WEHole18Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            //  WEHole9Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            //weekend
            WDDR(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            WEDR(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            //18 hole
            //weekday
            //weekend

            //WDHole27Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            //WEHole27Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);


        }

        private void WDHole9Pricing(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {

            Pricing pricing = new Pricing()
            {
                HoleTypeId = (int)Core.Helper.Enum.EnumHoleType.Hole9,
                EndDate = updatePricingViewModel.EndDate,
                StartDate = updatePricingViewModel.StartDate,
                GreenFee = updatePricingViewModel.WDH9UpdatePriceViewModel.GreenFee,
                MemberTypeId = memberTypeId,
                AddOnCaddie = updatePricingViewModel.WDH9UpdatePriceViewModel.AddOnCaddie,
                AddOnCart = updatePricingViewModel.WDH9UpdatePriceViewModel.AddOnCart,
                BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT,
                ConvenienceFee = updatePricingViewModel.WDH18UpdatePriceViewModel.ConvenienceFee,
                CurrencyId = 1,
                DayTypeId = (int)Core.Helper.Enum.EnumDayType.WeekDay,
                NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                TaxAndFee = updatePricingViewModel.WDH9UpdatePriceViewModel.TaxAndFee,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                SlotId = updatePricingViewModel.SlotId
            };
            _unitOfWork.PricingRepository.Insert(pricing);
            _unitOfWork.Save();

        }

        private void WDHole18Pricing(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {

            Pricing pricing = new Pricing()
            {
                HoleTypeId = (int)Core.Helper.Enum.EnumHoleType.Hole18,
                EndDate = updatePricingViewModel.EndDate,
                StartDate = updatePricingViewModel.StartDate,
                GreenFee = updatePricingViewModel.WDH18UpdatePriceViewModel.GreenFee,
                MemberTypeId = memberTypeId,
                AddOnCaddie = updatePricingViewModel.WDH18UpdatePriceViewModel.AddOnCaddie,
                AddOnCart = updatePricingViewModel.WDH18UpdatePriceViewModel.AddOnCart,
                BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT,
                ConvenienceFee = updatePricingViewModel.WDH18UpdatePriceViewModel.ConvenienceFee,
                CurrencyId = 1,
                DayTypeId = (int)Core.Helper.Enum.EnumDayType.WeekDay,
                NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                TaxAndFee = updatePricingViewModel.WDH18UpdatePriceViewModel.TaxAndFee,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                SlotId = updatePricingViewModel.SlotId
            };
            _unitOfWork.PricingRepository.Insert(pricing);
            _unitOfWork.Save();

        }

        private void WEHole18Pricing(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {

            Pricing pricing = new Pricing()
            {
                HoleTypeId = (int)Core.Helper.Enum.EnumHoleType.Hole18,
                EndDate = updatePricingViewModel.EndDate,
                StartDate = updatePricingViewModel.StartDate,
                GreenFee = updatePricingViewModel.WEH18UpdatePriceViewModel.GreenFee,
                MemberTypeId = memberTypeId,
                AddOnCaddie = updatePricingViewModel.WEH18UpdatePriceViewModel.AddOnCaddie,
                AddOnCart = updatePricingViewModel.WEH18UpdatePriceViewModel.AddOnCart,
                BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT,
                ConvenienceFee = updatePricingViewModel.WEH18UpdatePriceViewModel.ConvenienceFee,
                CurrencyId = 1,
                DayTypeId = (int)Core.Helper.Enum.EnumDayType.Weekend,
                NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                TaxAndFee = updatePricingViewModel.WEH18UpdatePriceViewModel.TaxAndFee,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                SlotId = updatePricingViewModel.SlotId
            };
            _unitOfWork.PricingRepository.Insert(pricing);
            _unitOfWork.Save();

        }

        private void WEHole9Pricing(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {

            Pricing pricing = new Pricing()
            {
                HoleTypeId = (int)Core.Helper.Enum.EnumHoleType.Hole9,
                EndDate = updatePricingViewModel.EndDate,
                StartDate = updatePricingViewModel.StartDate,
                GreenFee = updatePricingViewModel.WEH9UpdatePriceViewModel.GreenFee,
                MemberTypeId = memberTypeId,
                AddOnCaddie = updatePricingViewModel.WEH9UpdatePriceViewModel.AddOnCaddie,
                AddOnCart = updatePricingViewModel.WEH9UpdatePriceViewModel.AddOnCart,
                BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT,
                ConvenienceFee = updatePricingViewModel.WEH18UpdatePriceViewModel.ConvenienceFee,
                CurrencyId = 1,
                DayTypeId = (int)Core.Helper.Enum.EnumDayType.Weekend,
                NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                TaxAndFee = updatePricingViewModel.WEH9UpdatePriceViewModel.TaxAndFee,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                SlotId = updatePricingViewModel.SlotId
            };
            _unitOfWork.PricingRepository.Insert(pricing);
            _unitOfWork.Save();
        }


        private void WDHole27Pricing(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {

            Pricing pricing = new Pricing()
            {
                HoleTypeId = (int)Core.Helper.Enum.EnumHoleType.Hole27,
                EndDate = updatePricingViewModel.EndDate,
                StartDate = updatePricingViewModel.StartDate,
                GreenFee = updatePricingViewModel.WDH27UpdatePriceViewModel.GreenFee,
                MemberTypeId = memberTypeId,
                AddOnCaddie = updatePricingViewModel.WDH27UpdatePriceViewModel.AddOnCaddie,
                AddOnCart = updatePricingViewModel.WDH27UpdatePriceViewModel.AddOnCart,
                BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT,
                ConvenienceFee = updatePricingViewModel.WDH18UpdatePriceViewModel.ConvenienceFee,
                CurrencyId = 1,
                DayTypeId = (int)Core.Helper.Enum.EnumDayType.WeekDay,
                NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                TaxAndFee = updatePricingViewModel.WDH27UpdatePriceViewModel.TaxAndFee,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                SlotId = updatePricingViewModel.SlotId
            };
            _unitOfWork.PricingRepository.Insert(pricing);
            _unitOfWork.Save();

        }




        private void WEHole27Pricing(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {

            Pricing pricing = new Pricing()
            {
                HoleTypeId = (int)Core.Helper.Enum.EnumHoleType.Hole27,
                EndDate = updatePricingViewModel.EndDate,
                StartDate = updatePricingViewModel.StartDate,
                GreenFee = updatePricingViewModel.WEH27UpdatePriceViewModel.GreenFee,
                MemberTypeId = memberTypeId,
                AddOnCaddie = updatePricingViewModel.WEH27UpdatePriceViewModel.AddOnCaddie,
                AddOnCart = updatePricingViewModel.WEH27UpdatePriceViewModel.AddOnCart,
                BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT,
                ConvenienceFee = updatePricingViewModel.WEH18UpdatePriceViewModel.ConvenienceFee,
                CurrencyId = 1,
                DayTypeId = (int)Core.Helper.Enum.EnumDayType.Weekend,
                NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                TaxAndFee = updatePricingViewModel.WEH27UpdatePriceViewModel.TaxAndFee,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                SlotId = updatePricingViewModel.SlotId
            };
            _unitOfWork.PricingRepository.Insert(pricing);
            _unitOfWork.Save();

        }


        private void WEDR(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {
            if (updatePricingViewModel.WEDRUpdatePriceViewModel != null)
            {
                Pricing pricing = new Pricing()
                {
                    EndDate = updatePricingViewModel.EndDate,
                    StartDate = updatePricingViewModel.StartDate,
                    RangeFee = updatePricingViewModel.WEDRUpdatePriceViewModel.RangeFee,
                    MemberTypeId = memberTypeId,
                    HoleTypeId = null,
                    BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BDR,
                    ConvenienceFee = updatePricingViewModel.WEDRUpdatePriceViewModel.ConvenienceFee,
                    CurrencyId = 1,
                    DayTypeId = (int)Core.Helper.Enum.EnumDayType.Weekend,
                    NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                    TaxAndFee = updatePricingViewModel.WEDRUpdatePriceViewModel.TaxAndFee,
                    BucketFeePerPlayer = updatePricingViewModel.WEDRUpdatePriceViewModel.BucketFeePerPlayer,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow
                };
                _unitOfWork.PricingRepository.Insert(pricing);
                _unitOfWork.Save();
            }
        }

        private void WDDR(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {
            if (updatePricingViewModel.WDDRUpdatePriceViewModel != null)
            {
                Pricing pricing = new Pricing()
                {
                    EndDate = updatePricingViewModel.EndDate,
                    StartDate = updatePricingViewModel.StartDate,
                    RangeFee = updatePricingViewModel.WDDRUpdatePriceViewModel.RangeFee,
                    MemberTypeId = memberTypeId,
                    BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BDR,
                    ConvenienceFee = updatePricingViewModel.WDDRUpdatePriceViewModel.ConvenienceFee,
                    CurrencyId = 1,
                    HoleTypeId = null,
                    DayTypeId = (int)Core.Helper.Enum.EnumDayType.WeekDay,
                    NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                    TaxAndFee = updatePricingViewModel.WDDRUpdatePriceViewModel.TaxAndFee,
                    BucketFeePerPlayer = updatePricingViewModel.WDDRUpdatePriceViewModel.BucketFeePerPlayer,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow
                };
                _unitOfWork.PricingRepository.Insert(pricing);
                _unitOfWork.Save();

            }
        }


        public List<UpdatePricingViewModel> SearchAllPricing()
        {
            List<UpdatePricingViewModel> updatePricingViewModels = new List<UpdatePricingViewModel>();
            List<Pricing> pricings = _unitOfWork.PricingRepository.GetMany(x => x.IsActive == true).ToList();
            List<Pricing> pricings1 = pricings.OrderBy(x => x.StartDate).OrderBy(x => x.EndDate).OrderBy(x => x.MemberTypeId).OrderBy(x => x.SlotId).ToList();
            UpdatePricingViewModel updatePricingViewModel = new UpdatePricingViewModel();
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            long memberTypeId = 0;
            long slotId = 0;
            foreach (Pricing pricing in pricings1)
            {
                if (pricing.StartDate != startDate || pricing.EndDate != endDate || pricing.MemberType.MemberTypeId != memberTypeId || pricing.SlotId.GetValueOrDefault() != slotId)
                {
                    if (startDate == new DateTime()) { }
                    else
                    {
                        updatePricingViewModels.Add(updatePricingViewModel);
                    }
                    updatePricingViewModel = new UpdatePricingViewModel();
                }
                startDate = pricing.StartDate;
                endDate = pricing.EndDate;
                memberTypeId = pricing.MemberType.MemberTypeId;
                updatePricingViewModel.StartDate = pricing.StartDate;
                updatePricingViewModel.EndDate = pricing.EndDate;
                slotId = pricing.SlotId.GetValueOrDefault();
                updatePricingViewModel.SlotId = pricing.SlotId.GetValueOrDefault();
                updatePricingViewModel.MemberTypeId = pricing.MemberType.MemberTypeId;
                updatePricingViewModel.MemberType = pricing.MemberType.Name;
                updatePricingViewModel.SlotTime = pricing.SlotId != null ? pricing.Slot.Time.ToString() : "0";
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.WeekDay && pricing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9 && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WDH9UpdatePriceViewModel wDH9UpdatePriceViewModel = new WDH9UpdatePriceViewModel()
                    {
                        GreenFee = pricing.GreenFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault(),
                        AddOnCart = pricing.AddOnCart.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WDH9UpdatePriceViewModel = wDH9UpdatePriceViewModel;

                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.WeekDay && pricing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18 && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WDH18UpdatePriceViewModel wDH18UpdatePriceViewModel = new WDH18UpdatePriceViewModel()
                    {
                        GreenFee = pricing.GreenFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault(),
                        AddOnCart = pricing.AddOnCart.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WDH18UpdatePriceViewModel = wDH18UpdatePriceViewModel;

                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.Weekend && pricing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9 && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WEH9UpdatePriceViewModel wEH9UpdatePriceViewModel = new WEH9UpdatePriceViewModel()
                    {
                        GreenFee = pricing.GreenFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault(),
                        AddOnCart = pricing.AddOnCart.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WEH9UpdatePriceViewModel = wEH9UpdatePriceViewModel;

                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.Weekend && pricing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18 && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WEH18UpdatePriceViewModel wEH18UpdatePriceViewModel = new WEH18UpdatePriceViewModel()
                    {
                        GreenFee = pricing.GreenFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault(),
                        AddOnCart = pricing.AddOnCart.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WEH18UpdatePriceViewModel = wEH18UpdatePriceViewModel;
                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.WeekDay && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BDR)
                {
                    WDDRUpdatePriceViewModel wDDRUpdatePriceViewModel = new WDDRUpdatePriceViewModel()
                    {
                        RangeFee = pricing.RangeFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        BucketFeePerPlayer = pricing.BucketFeePerPlayer.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WDDRUpdatePriceViewModel = wDDRUpdatePriceViewModel;
                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.Weekend && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BDR)
                {
                    WEDRUpdatePriceViewModel wEDRUpdatePriceViewModel = new WEDRUpdatePriceViewModel()
                    {
                        RangeFee = pricing.RangeFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        BucketFeePerPlayer = pricing.BucketFeePerPlayer.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WEDRUpdatePriceViewModel = wEDRUpdatePriceViewModel;
                }

            }
            updatePricingViewModels.Add(updatePricingViewModel);
            return updatePricingViewModels;
        }

        public List<UpdatePricingViewModel> SearchAllPricing(long memberTypeId1, long bookingTypeId, long sessionId)
        {
            List<UpdatePricingViewModel> updatePricingViewModels = new List<UpdatePricingViewModel>();
            List<Pricing> pricings = _unitOfWork.PricingRepository.GetMany(x => x.IsActive == true && x.MemberTypeId == memberTypeId1 && x.BookingTypeId == bookingTypeId).ToList();
            List<Pricing> pricings1 = pricings.OrderBy(x => x.StartDate).OrderBy(x => x.EndDate).OrderBy(x => x.MemberTypeId).OrderBy(x => x.SlotId).ToList();
            UpdatePricingViewModel updatePricingViewModel = new UpdatePricingViewModel();
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            long memberTypeId = 0;
            long slotId = 0;
            foreach (Pricing pricing in pricings1)
            {
                if (pricing.StartDate != startDate || pricing.EndDate != endDate || pricing.MemberType.MemberTypeId != memberTypeId || pricing.SlotId.GetValueOrDefault() != slotId)
                {
                    if (startDate == new DateTime()) { }
                    else
                    {
                        updatePricingViewModels.Add(updatePricingViewModel);
                    }
                    updatePricingViewModel = new UpdatePricingViewModel();
                }
                startDate = pricing.StartDate;
                endDate = pricing.EndDate;
                memberTypeId = pricing.MemberType.MemberTypeId;
                updatePricingViewModel.StartDate = pricing.StartDate;
                updatePricingViewModel.EndDate = pricing.EndDate;
                updatePricingViewModel.SlotId = slotId;
                slotId = pricing.SlotId.GetValueOrDefault();
                updatePricingViewModel.SlotId = pricing.SlotId.GetValueOrDefault();
                updatePricingViewModel.MemberTypeId = pricing.MemberType.MemberTypeId;
                updatePricingViewModel.MemberType = pricing.MemberType.Name;
                updatePricingViewModel.SlotTime = pricing.SlotId != null ? pricing.Slot.Time.ToString() : "0";
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.WeekDay && pricing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9 && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WDH9UpdatePriceViewModel wDH9UpdatePriceViewModel = new WDH9UpdatePriceViewModel()
                    {
                        GreenFee = pricing.GreenFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault(),
                        AddOnCart = pricing.AddOnCart.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WDH9UpdatePriceViewModel = wDH9UpdatePriceViewModel;

                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.WeekDay && pricing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18 && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WDH18UpdatePriceViewModel wDH18UpdatePriceViewModel = new WDH18UpdatePriceViewModel()
                    {
                        GreenFee = pricing.GreenFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault(),
                        AddOnCart = pricing.AddOnCart.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WDH18UpdatePriceViewModel = wDH18UpdatePriceViewModel;

                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.Weekend && pricing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9 && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WEH9UpdatePriceViewModel wEH9UpdatePriceViewModel = new WEH9UpdatePriceViewModel()
                    {
                        GreenFee = pricing.GreenFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault(),
                        AddOnCart = pricing.AddOnCart.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WEH9UpdatePriceViewModel = wEH9UpdatePriceViewModel;

                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.Weekend && pricing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18 && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WEH18UpdatePriceViewModel wEH18UpdatePriceViewModel = new WEH18UpdatePriceViewModel()
                    {
                        GreenFee = pricing.GreenFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault(),
                        AddOnCart = pricing.AddOnCart.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WEH18UpdatePriceViewModel = wEH18UpdatePriceViewModel;
                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.WeekDay && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    WDDRUpdatePriceViewModel wDDRUpdatePriceViewModel = new WDDRUpdatePriceViewModel()
                    {
                        RangeFee = pricing.RangeFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        BucketFeePerPlayer = pricing.BucketFeePerPlayer.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WDDRUpdatePriceViewModel = wDDRUpdatePriceViewModel;
                }
                if (pricing.DayTypeId == (int)Core.Helper.Enum.EnumDayType.Weekend && pricing.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BDR)
                {
                    WEDRUpdatePriceViewModel wEDRUpdatePriceViewModel = new WEDRUpdatePriceViewModel()
                    {
                        RangeFee = pricing.RangeFee.GetValueOrDefault(),
                        TaxAndFee = pricing.TaxAndFee.GetValueOrDefault(),
                        BucketFeePerPlayer = pricing.BucketFeePerPlayer.GetValueOrDefault(),
                        ConvenienceFee = pricing.ConvenienceFee.GetValueOrDefault()
                    };
                    updatePricingViewModel.WEDRUpdatePriceViewModel = wEDRUpdatePriceViewModel;
                }

            }
            updatePricingViewModels.Add(updatePricingViewModel);
            return updatePricingViewModels;
        }


        public void SaveNationalHoildayPricing(UpdatePricingViewModel updatePricingViewModel)
        {

            if (updatePricingViewModel.SlotId == 0)
            {
                updatePricingViewModel.SlotId = null;
            }
            //9 hole
            //weekday
            //  WDHole9Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            NHHole18Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            //  NHHole27Pricing(updatePricingViewModel, updatePricingViewModel.MemberTypeId);
            NHDR(updatePricingViewModel, updatePricingViewModel.MemberTypeId);

        }


        private void NHHole18Pricing(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {

            Pricing pricingDB = _unitOfWork.PricingRepository.Get(x => x.IsActive == true && x.DayTypeId == (int)Core.Helper.Enum.EnumDayType.NationalHoliday && x.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18);
            if (pricingDB == null)
            {
                Pricing pricing = new Pricing()
                {
                    HoleTypeId = (int)Core.Helper.Enum.EnumHoleType.Hole18,
                    EndDate = updatePricingViewModel.EndDate,
                    StartDate = updatePricingViewModel.StartDate,
                    GreenFee = updatePricingViewModel.WDH18UpdatePriceViewModel.GreenFee,
                    MemberTypeId = memberTypeId,
                    AddOnCaddie = updatePricingViewModel.WDH18UpdatePriceViewModel.AddOnCaddie,
                    AddOnCart = updatePricingViewModel.WDH18UpdatePriceViewModel.AddOnCart,
                    BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT,
                    ConvenienceFee = updatePricingViewModel.WDH18UpdatePriceViewModel.ConvenienceFee,
                    CurrencyId = 1,
                    DayTypeId = (int)Core.Helper.Enum.EnumDayType.NationalHoliday,
                    NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                    TaxAndFee = updatePricingViewModel.WDH18UpdatePriceViewModel.TaxAndFee,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow,
                    SlotId = updatePricingViewModel.SlotId
                };
                _unitOfWork.PricingRepository.Insert(pricing);
            }
            else
            {
                pricingDB.GreenFee = updatePricingViewModel.WDH18UpdatePriceViewModel.GreenFee;
                pricingDB.MemberTypeId = memberTypeId;
                pricingDB.AddOnCaddie = updatePricingViewModel.WDH18UpdatePriceViewModel.AddOnCaddie;
                pricingDB.AddOnCart = updatePricingViewModel.WDH18UpdatePriceViewModel.AddOnCart;
                pricingDB.BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT;
                pricingDB.ConvenienceFee = updatePricingViewModel.WDH18UpdatePriceViewModel.ConvenienceFee;
                pricingDB.CurrencyId = 1;
                pricingDB.DayTypeId = (int)Core.Helper.Enum.EnumDayType.NationalHoliday;
                pricingDB.NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions;
                pricingDB.TaxAndFee = updatePricingViewModel.WDH18UpdatePriceViewModel.TaxAndFee;
                _unitOfWork.PricingRepository.Update(pricingDB);
            }
            _unitOfWork.Save();

        }

        private void NHHole27Pricing(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {
            Pricing pricingDB = _unitOfWork.PricingRepository.Get(x => x.IsActive == true && x.DayTypeId == (int)Core.Helper.Enum.EnumDayType.NationalHoliday && x.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole27);
            if (pricingDB == null)
            {
                Pricing pricing = new Pricing()
                {
                    HoleTypeId = (int)Core.Helper.Enum.EnumHoleType.Hole27,
                    EndDate = updatePricingViewModel.EndDate,
                    StartDate = updatePricingViewModel.StartDate,
                    GreenFee = updatePricingViewModel.WEH27UpdatePriceViewModel.GreenFee,
                    MemberTypeId = memberTypeId,
                    AddOnCaddie = updatePricingViewModel.WEH27UpdatePriceViewModel.AddOnCaddie,
                    AddOnCart = updatePricingViewModel.WEH27UpdatePriceViewModel.AddOnCart,
                    BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT,
                    ConvenienceFee = updatePricingViewModel.WEH18UpdatePriceViewModel.ConvenienceFee,
                    CurrencyId = 1,
                    DayTypeId = (int)Core.Helper.Enum.EnumDayType.NationalHoliday,
                    NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                    TaxAndFee = updatePricingViewModel.WEH27UpdatePriceViewModel.TaxAndFee,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow,
                    SlotId = updatePricingViewModel.SlotId
                };
                _unitOfWork.PricingRepository.Insert(pricing);
            }
            else
            {

                pricingDB.GreenFee = updatePricingViewModel.WDH27UpdatePriceViewModel.GreenFee;
                pricingDB.MemberTypeId = memberTypeId;
                pricingDB.AddOnCaddie = updatePricingViewModel.WDH27UpdatePriceViewModel.AddOnCaddie;
                pricingDB.AddOnCart = updatePricingViewModel.WDH27UpdatePriceViewModel.AddOnCart;
                pricingDB.BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT;
                pricingDB.ConvenienceFee = updatePricingViewModel.WDH18UpdatePriceViewModel.ConvenienceFee;
                pricingDB.CurrencyId = 1;
                pricingDB.DayTypeId = (int)Core.Helper.Enum.EnumDayType.NationalHoliday;
                pricingDB.NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions;
                pricingDB.TaxAndFee = updatePricingViewModel.WDH27UpdatePriceViewModel.TaxAndFee;
                _unitOfWork.PricingRepository.Update(pricingDB);
            }
            _unitOfWork.Save();
        }

        private void NHDR(UpdatePricingViewModel updatePricingViewModel, long memberTypeId)
        {
            Pricing pricingDB = _unitOfWork.PricingRepository.Get(x => x.IsActive == true && x.DayTypeId == (int)Core.Helper.Enum.EnumDayType.NationalHoliday);
            if (pricingDB == null)
            {
                Pricing pricing = new Pricing()
                {
                    EndDate = updatePricingViewModel.EndDate,
                    StartDate = updatePricingViewModel.StartDate,
                    RangeFee = updatePricingViewModel.WDDRUpdatePriceViewModel.RangeFee,
                    MemberTypeId = memberTypeId,
                    HoleTypeId = null,
                    BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BDR,
                    ConvenienceFee = updatePricingViewModel.WDDRUpdatePriceViewModel.ConvenienceFee,
                    CurrencyId = 1,
                    DayTypeId = (int)Core.Helper.Enum.EnumDayType.NationalHoliday,
                    NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions,
                    TaxAndFee = updatePricingViewModel.WDDRUpdatePriceViewModel.TaxAndFee,
                    BucketFeePerPlayer = updatePricingViewModel.WDDRUpdatePriceViewModel.BucketFeePerPlayer,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow
                };
                _unitOfWork.PricingRepository.Insert(pricing);
            }
            else
            {

                pricingDB.RangeFee = updatePricingViewModel.WDDRUpdatePriceViewModel.RangeFee;
                pricingDB.MemberTypeId = memberTypeId;
                pricingDB.HoleTypeId = null;
                pricingDB.BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BDR;
                pricingDB.ConvenienceFee = updatePricingViewModel.WDDRUpdatePriceViewModel.ConvenienceFee;
                pricingDB.CurrencyId = 1;
                pricingDB.DayTypeId = (int)Core.Helper.Enum.EnumDayType.NationalHoliday;
                pricingDB.NumberOfAllowedSessions = updatePricingViewModel.NumberOfAllowedSessions;
                pricingDB.TaxAndFee = updatePricingViewModel.WDDRUpdatePriceViewModel.TaxAndFee;
                pricingDB.BucketFeePerPlayer = updatePricingViewModel.WDDRUpdatePriceViewModel.BucketFeePerPlayer;
                _unitOfWork.PricingRepository.Update(pricingDB);

            }
            _unitOfWork.Save();
        }
        #endregion

        #region New Pricing 

        public void SavePricing(PricingViewModel pricingViewModel, long uniqueSessionId)
        {
            if (pricingViewModel.IsSpecialPricing == true)
            {
                if (pricingViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    SavePricingBTTSP(pricingViewModel, uniqueSessionId);
                }
                else if (pricingViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BDR)
                {
                    SavePricingBDRSP(pricingViewModel);
                }
                else
                {
                    throw new Exception("Invalid Booking Id");
                }
            }
            else
            {
                if (pricingViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {
                    SaveUpdatePricingBTT(pricingViewModel, uniqueSessionId);
                }
                else if (pricingViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BDR)
                {
                    SaveUpdatePricingBDR(pricingViewModel);
                }
                else
                {
                    throw new Exception("Invalid Booking Id");
                }

            }

        }


        private void SaveUpdatePricingBTT(PricingViewModel pricingViewModel, long uniqueSessionId)
        {
            if (pricingViewModel.PricingId == 0)
            {
                Pricing pricing = new Pricing()
                {
                    AddOnCaddie = pricingViewModel.AddOnCaddie,
                    AddOnCart = pricingViewModel.AddOnCart,
                    GreenFee = pricingViewModel.GreenFee,
                    HoleTypeId = pricingViewModel.HoleTypeId,
                    MemberTypeId = pricingViewModel.MemberTypeId,
                    DayTypeId = pricingViewModel.DayTypeId,
                    BookingTypeId = pricingViewModel.BookingTypeId,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    StartDate = new DateTime(2000, 1, 1),
                    EndDate = new DateTime(2099, 1, 1),
                    IsSpecialPrice = false,
                    TimeFormatId = pricingViewModel.TimeFormatId
                };
                if (pricingViewModel.SlotId == 0)
                {
                    pricing.SlotId = null;
                }
                _unitOfWork.PricingRepository.Insert(pricing);
                _unitOfWork.Save();
                Tuple<string, string, string> tax = SaveUpdateTax(pricingViewModel, pricing.PricingId);

                try
                {
                    UnitOfWork _unitOfWork = new UnitOfWork();
                    Pricing pricing1 = _unitOfWork.PricingRepository.GetById(pricing.PricingId);
                    SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                    {
                        ControllerName = "Standard Pricing",
                        ActionName = "Save",
                        PerformOn = pricing.PricingId.ToString(),
                        LoginHistoryId = uniqueSessionId,

                        Info = "Update a BTT Price with id- " + pricing.PricingId.ToString() + ".Pricing had following details:-"
                        + Environment.NewLine+
                        "Member Type" + pricing1.MemberType.Name + "" + Environment.NewLine
                        + "Hole Type" + pricing1.HoleType.Value + "" + Environment.NewLine
                        + "Time Type" + pricing1.TimeFormat.Name + "" + Environment.NewLine
                        + "Day Type" + pricing1.DayType.Name + "" + Environment.NewLine
                        + "Green Fee" + pricing.GreenFee + "" + Environment.NewLine +
                        "Cart Fee" + pricing.AddOnCart + "" + Environment.NewLine
                        + "Caddie Fee" + pricing.AddOnCaddie + "" + Environment.NewLine
                        + "Green Fee Tax" + tax.Item1 + "" + Environment.NewLine +
                        "Cart Fee Tax" + tax.Item2 + "" + Environment.NewLine
                        + "Caddie Fee Tax" + tax.Item3 + "" + Environment.NewLine
                    };
                    new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

                }
                catch (Exception)
                {

                }

            }
            else
            {
                Pricing pricing = _unitOfWork.PricingRepository.GetById(pricingViewModel.PricingId);
                if (pricing != null)
                {
                    pricing.AddOnCaddie = pricingViewModel.AddOnCaddie;
                    pricing.AddOnCart = pricingViewModel.AddOnCart;
                    pricing.GreenFee = pricingViewModel.GreenFee;
                    _unitOfWork.PricingRepository.Update(pricing);
                    _unitOfWork.Save();
                    Tuple<string, string, string> tax = SaveUpdateTax(pricingViewModel, pricing.PricingId);
                    try
                    {

                        SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                        {
                            ControllerName = "Standard Pricing",
                            ActionName = "Save",
                            PerformOn = pricing.PricingId.ToString(),
                            LoginHistoryId = uniqueSessionId,

                            Info = "Update a BTT Price with id- " + pricing.PricingId.ToString() + ".Pricing had following details:-"
                            + Environment.NewLine + "Day " + pricing.DayType.Name + "" + Environment.NewLine +
                            "Member Type" + pricing.MemberType.Name + "" + Environment.NewLine
                            + "Hole Type" + pricing.HoleType.Value + "" + Environment.NewLine
                            + "Time Type" + pricing.TimeFormat.Name + "" + Environment.NewLine
                            + "Day Type" + pricing.DayType.Name + "" + Environment.NewLine
                            + "Green Fee" + pricing.GreenFee + "" + Environment.NewLine +
                            "Cart Fee" + pricing.AddOnCart + "" + Environment.NewLine
                            + "Caddie Fee" + pricing.AddOnCaddie + "" + Environment.NewLine
                            + "Green Fee Tax" + tax.Item1 + "" + Environment.NewLine +
                            "Cart Fee Tax" + tax.Item2 + "" + Environment.NewLine
                            + "Caddie Fee Tax" + tax.Item3 + "" + Environment.NewLine
                        };
                        new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

                    }
                    catch (Exception)
                    {

                    }
                }


            }

        }

        private void SaveUpdatePricingBDR(PricingViewModel pricingViewModel)
        {
            if (pricingViewModel.PricingId == 0)
            {
                Pricing pricing = new Pricing()
                {
                    AddOnCaddie = pricingViewModel.AddOnCaddie,
                    AddOnCart = pricingViewModel.AddOnCart,
                    RangeFee = pricingViewModel.RangeFee,
                    MemberTypeId = pricingViewModel.MemberTypeId,
                    DayTypeId = pricingViewModel.DayTypeId,
                    BookingTypeId = pricingViewModel.BookingTypeId,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    StartDate = new DateTime(2000, 1, 1),
                    EndDate = new DateTime(2099, 1, 1),
                    IsSpecialPrice = false,
                    TimeFormatId = pricingViewModel.TimeFormatId

                };
                if (pricingViewModel.SlotId == 0)
                {
                    pricing.SlotId = null;
                }
                _unitOfWork.PricingRepository.Insert(pricing);
                _unitOfWork.Save();
                SaveUpdateTax(pricingViewModel, pricing.PricingId);
                _unitOfWork.Save();
            }
            else
            {
                Pricing pricing = _unitOfWork.PricingRepository.GetById(pricingViewModel.PricingId);
                if (pricing != null)
                {
                    pricing.RangeFee = pricingViewModel.RangeFee;

                    _unitOfWork.PricingRepository.Update(pricing);
                }

                SaveUpdateTax(pricingViewModel, pricing.PricingId);
            }

            _unitOfWork.Save();
        }


        private void SavePricingBTTSP(PricingViewModel pricingViewModel, long uniqueSessionId)
        {

            Pricing pricing = new Pricing()
            {
                AddOnCaddie = pricingViewModel.AddOnCaddie,
                AddOnCart = pricingViewModel.AddOnCart,
                GreenFee = pricingViewModel.GreenFee,
                HoleTypeId = pricingViewModel.HoleTypeId,
                MemberTypeId = pricingViewModel.MemberTypeId,
                BookingTypeId = pricingViewModel.BookingTypeId,
                StartDate = pricingViewModel.StartDate,
                EndDate = pricingViewModel.EndDate,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                IsSpecialPrice = true

            };
            if (pricingViewModel.SlotId == 0)
            {
                pricing.SlotId = null;
            }
            else { pricing.SlotId = pricingViewModel.SlotId; }
            _unitOfWork.PricingRepository.Insert(pricing);
            _unitOfWork.Save();
            Tuple<string, string, string> tax = SaveUpdateTax(pricingViewModel, pricing.PricingId);
            _unitOfWork.Save();
            
            try
            {
                UnitOfWork _unitOfWork = new UnitOfWork();
                Pricing pricing1 = _unitOfWork.PricingRepository.GetById(pricing.PricingId);
                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Special Pricing",
                    ActionName = "Save",
                    PerformOn = pricing.PricingId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Added a Special BTT Price with id- " + pricing.PricingId.ToString() + ".Pricing had following details:-"
                     + "" + Environment.NewLine +
                    "Member Type" + pricing1.MemberType.Name + "" + Environment.NewLine
                    + "Hole Type " + pricing1.HoleType.Value + "" + Environment.NewLine
                    + "Slot " + (pricing1.SlotId != null ? pricing1.Slot.Time.ToString() : "") + "" + Environment.NewLine
                    + "Start Date" + pricing.StartDate + "" + Environment.NewLine
                    + "End Date" + pricing.EndDate + "" + Environment.NewLine
                    + "Green Fee" + pricing.GreenFee + "" + Environment.NewLine +
                    "Cart Fee" + pricing.AddOnCart + "" + Environment.NewLine
                    + "Caddie Fee" + pricing.AddOnCaddie + "" + Environment.NewLine
                    + "Green Fee Tax" + tax.Item1 + "" + Environment.NewLine +
                    "Cart Fee Tax" + tax.Item2 + "" + Environment.NewLine
                    + "Caddie Fee Tax" + tax.Item3 + "" + Environment.NewLine
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
        }

        private void SavePricingBDRSP(PricingViewModel pricingViewModel)
        {

            Pricing pricing = new Pricing()
            {
                AddOnCaddie = pricingViewModel.AddOnCaddie,
                AddOnCart = pricingViewModel.AddOnCart,
                RangeFee = pricingViewModel.RangeFee,
                MemberTypeId = pricingViewModel.MemberTypeId,
                DayTypeId = pricingViewModel.DayTypeId,
                BookingTypeId = pricingViewModel.BookingTypeId,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                IsSpecialPrice = true

            };
            if (pricingViewModel.SlotId == 0)
            {
                pricing.SlotId = null;
            }
            else { pricing.SlotId = pricingViewModel.SlotId; }
            _unitOfWork.PricingRepository.Insert(pricing);

            SaveUpdateTax(pricingViewModel, pricing.PricingId);
            _unitOfWork.Save();
        }

        private Tuple<string, string, string> SaveUpdateTax(PricingViewModel pricingViewModel, long pricingId)
        {
            string greenFeeTax = "", cartFeeTax = "", caddieFeeTax = "";

            List<PriceTaxMapping> priceTaxMappings = _unitOfWork.PriceTaxMappingRepository.GetMany(x => x.IsActive == true && x.PricingId == pricingId).ToList();

            foreach (PriceTaxMapping priceTaxMap in priceTaxMappings)
            {
                priceTaxMap.IsActive = false;
                _unitOfWork.PriceTaxMappingRepository.Update(priceTaxMap);
                _unitOfWork.Save();
            }
            if (pricingViewModel.GreenFeeTax != null)
            {

                foreach (long id in pricingViewModel.GreenFeeTax)
                {
                    PriceTaxMapping priceTaxMapping = new PriceTaxMapping()
                    {
                        PricingId = pricingId,
                        TaxId = Convert.ToInt64(id),
                        FeeCategoryId = (int)Core.Helper.Enum.EnumFeeCategory.GreenFee,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow
                    };
                    _unitOfWork.PriceTaxMappingRepository.Insert(priceTaxMapping);
                    _unitOfWork.Save();
                    Tax c = _unitOfWork.TaxRepository.Get(x => x.TaxId == id);
                    greenFeeTax = (greenFeeTax != "" ? greenFeeTax + "," + c.Name : c.Name);
                }
            }
            if (pricingViewModel.RangeFeeTax != null)
            {

                foreach (long id in pricingViewModel.RangeFeeTax)
                {
                    PriceTaxMapping priceTaxMapping = new PriceTaxMapping()
                    {
                        PricingId = pricingId,
                        TaxId = Convert.ToInt64(id),
                        FeeCategoryId = (int)Core.Helper.Enum.EnumFeeCategory.RangeFee,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow
                    };
                    _unitOfWork.PriceTaxMappingRepository.Insert(priceTaxMapping);
                }
            }
            if (pricingViewModel.AddOnCaddieTax != null)
            {

                foreach (long id in pricingViewModel.AddOnCaddieTax)
                {
                    PriceTaxMapping priceTaxMapping = new PriceTaxMapping()
                    {
                        PricingId = pricingId,
                        TaxId = Convert.ToInt64(id),
                        FeeCategoryId = (int)Core.Helper.Enum.EnumFeeCategory.AddOnCaddie,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow
                    };
                    _unitOfWork.PriceTaxMappingRepository.Insert(priceTaxMapping);
                    _unitOfWork.Save();
                    Tax c = _unitOfWork.TaxRepository.Get(x => x.TaxId == id);

                    caddieFeeTax = (caddieFeeTax != "" ? caddieFeeTax + "," + c.Name : c.Name);

                }
            }
            if (pricingViewModel.AddOnCartTax != null)
            {

                foreach (long id in pricingViewModel.AddOnCartTax)
                {
                    PriceTaxMapping priceTaxMapping = new PriceTaxMapping()
                    {
                        PricingId = pricingId,
                        TaxId = Convert.ToInt64(id),
                        FeeCategoryId = (int)Core.Helper.Enum.EnumFeeCategory.AddOnCart,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow
                    };
                    _unitOfWork.PriceTaxMappingRepository.Insert(priceTaxMapping);
                    _unitOfWork.Save();

                    Tax c = _unitOfWork.TaxRepository.Get(x => x.TaxId == id);

                    cartFeeTax = (cartFeeTax != "" ? cartFeeTax + "," + c.Name : c.Name);

                }
            }

            return new Tuple<string, string, string>(greenFeeTax, cartFeeTax, caddieFeeTax);

        }



        public PricingViewModel SearchPricing(PricingViewModel pricingViewModel)
        {
            PricingViewModel pricingVM = new PricingViewModel();
            Pricing pricing = new Pricing();
            if (pricingViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
            {
                pricing = _unitOfWork.PricingRepository.Get(x => x.IsActive == true && x.IsSpecialPrice == false && x.BookingTypeId == pricingViewModel.BookingTypeId && x.HoleTypeId == pricingViewModel.HoleTypeId && x.MemberTypeId == pricingViewModel.MemberTypeId && x.DayTypeId == pricingViewModel.DayTypeId && x.TimeFormatId == pricingViewModel.TimeFormatId);
            }
            else
            {
                pricing = _unitOfWork.PricingRepository.Get(x => x.IsActive == true && x.IsSpecialPrice == false && x.BookingTypeId == pricingViewModel.BookingTypeId && x.MemberTypeId == pricingViewModel.MemberTypeId && x.DayTypeId == pricingViewModel.DayTypeId && x.TimeFormatId == pricingViewModel.TimeFormatId);
            }

            pricingVM.BookingTypeId = pricingViewModel.BookingTypeId;
            pricingVM.MemberTypeId = pricingViewModel.MemberTypeId;
            pricingVM.DayTypeId = pricingViewModel.DayTypeId;
            pricingVM.SlotId = pricingViewModel.SlotId;
            pricingVM.HoleTypeId = pricingViewModel.HoleTypeId;
            pricingVM.StartDate = pricingViewModel.StartDate;
            pricingVM.EndDate = pricingViewModel.EndDate;

            if (pricing != null)
            {
                pricingVM.PricingId = pricing.PricingId;
                pricingVM.AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault();
                pricingVM.AddOnCart = pricing.AddOnCart.GetValueOrDefault();
                pricingVM.GreenFee = pricing.GreenFee.GetValueOrDefault();
                pricingVM.RangeFee = pricing.RangeFee.GetValueOrDefault();
                pricingVM.RangeFeeTax = pricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.RangeFee && x.IsActive == true).Select(x => x.TaxId).ToArray();
                pricingVM.GreenFeeTax = pricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.GreenFee && x.IsActive == true).Select(x => x.TaxId).ToArray();
                pricingVM.AddOnCaddieTax = pricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCaddie && x.IsActive == true).Select(x => x.TaxId).ToArray();
                pricingVM.AddOnCartTax = pricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCart && x.IsActive == true).Select(x => x.TaxId).ToArray();

            }
            return pricingVM;
        }

        /// <summary>
        /// Get All Pricing Detail
        /// </summary>
        /// <returns></returns>
        public List<PricingViewModel> GetAllPricing()
        {
            List<PricingViewModel> pricingViewModels = new List<PricingViewModel>();
            List<Pricing> pricings = _unitOfWork.PricingRepository.GetMany(x => x.IsActive == true && x.MemberType.IsActive == true).ToList();

            foreach (var pricing in pricings)
            {
                PricingViewModel pricingVM = new PricingViewModel
                {
                    BookingTypeId = pricing.BookingTypeId,
                    MemberTypeId = pricing.MemberTypeId,
                    DayTypeId = pricing.DayTypeId.GetValueOrDefault(),
                    SlotId = pricing.SlotId.GetValueOrDefault(),
                    HoleTypeId = pricing.HoleTypeId.GetValueOrDefault(),
                    StartDate = pricing.StartDate,
                    EndDate = pricing.EndDate,
                    SlotTime = pricing.SlotId != null ? pricing.Slot.Time.ToString() : ""
                };
                if (pricing != null)
                {
                    pricingVM.PricingId = pricing.PricingId;
                    pricingVM.AddOnCaddie = pricing.AddOnCaddie.GetValueOrDefault();
                    pricingVM.AddOnCart = pricing.AddOnCart.GetValueOrDefault();
                    pricingVM.GreenFee = pricing.GreenFee.GetValueOrDefault();
                    pricingVM.RangeFee = pricing.RangeFee.GetValueOrDefault();

                    pricingVM.TimeFormatName = pricing.TimeFormatId != null ? pricing.TimeFormat.Name : "";

                    pricingVM.MemberTypeName = pricing.MemberTypeId != null ? pricing.MemberType.Name : "";
                    pricingVM.HoleTypeName = pricing.HoleTypeId != null ? pricing.HoleType.Value.ToString() : "";
                    pricingVM.DayTypeName = pricing.DayTypeId != null ? pricing.DayType.Name : "";
                    pricingVM.BookingTypeName = pricing.BookingTypeId != null ? pricing.BookingType.Value : "";


                    pricingVM.PricingTaxMappingViewModels = new List<PricingTaxMappingViewModel>();
                    foreach (var tax in pricing.PriceTaxMappings)
                    {
                        PricingTaxMappingViewModel pricingTaxMappingViewModel = new PricingTaxMappingViewModel()
                        {
                            TaxName = tax.Tax.Name,
                            Percentage = tax.Tax.Percentage.ToString(),
                            FeeCategoryName = tax.FeeCategory.Name

                        };
                        pricingVM.PricingTaxMappingViewModels.Add(pricingTaxMappingViewModel);
                    }

                }

                pricingViewModels.Add(pricingVM);
            };

            return pricingViewModels;
        }
        #endregion

        public void SavePricingMuliple(PricingViewModel pricingViewModel, long uniqueSessionId)
        {
            if (pricingViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
            { MultipleBTTSavePricing(pricingViewModel, uniqueSessionId); }
            else
            {
                MultipleBDRSavePricing(pricingViewModel, uniqueSessionId);
            }

        }

        public void MultipleBTTSavePricing(PricingViewModel pricingViewModel, long uniqueSessionId)
        {
            if (pricingViewModel.IsSpecialPricing == false)
            {
                if (pricingViewModel.HoleTypeArray.Count() != 0)
                {
                    foreach (int holeId in pricingViewModel.HoleTypeArray)
                    {
                        if (pricingViewModel.MemberTypeArray.Count() != 0)
                        {
                            foreach (int memberTypeId in pricingViewModel.MemberTypeArray)
                            {
                                if (pricingViewModel.DayArray.Count() != 0)
                                {
                                    foreach (int dayId in pricingViewModel.DayArray)
                                    {
                                        if (pricingViewModel.TimeFormatArray.Count() != 0)
                                        {
                                            foreach (int timeFormatId in pricingViewModel.TimeFormatArray)
                                            {
                                                PricingViewModel cc = SearchPricing(pricingViewModel);

                                                if (cc != null)
                                                    pricingViewModel.PricingId = cc.PricingId;
                                                pricingViewModel.TimeFormatId = timeFormatId;
                                                pricingViewModel.DayTypeId = dayId;
                                                pricingViewModel.MemberTypeId = memberTypeId;
                                                pricingViewModel.HoleTypeId = holeId;
                                                SavePricing(pricingViewModel, uniqueSessionId);
                                            }
                                        };

                                    };

                                }
                            };
                        }
                    };
                }
            }
            else
            {
                if (pricingViewModel.HoleTypeArray.Count() != 0)
                {
                    foreach (int holeId in pricingViewModel.HoleTypeArray)
                    {
                        if (pricingViewModel.MemberTypeArray.Count() != 0)
                        {
                            foreach (int memberTypeId in pricingViewModel.MemberTypeArray)
                            {
                                if (pricingViewModel.SlotArray.Count() != 0)
                                {
                                    foreach (int slotId in pricingViewModel.SlotArray)
                                    {
                                        pricingViewModel.SlotId = slotId;
                                        pricingViewModel.MemberTypeId = memberTypeId;
                                        pricingViewModel.HoleTypeId = holeId;
                                        SavePricing(pricingViewModel, uniqueSessionId);
                                    }
                                }
                                else
                                {
                                    pricingViewModel.MemberTypeId = memberTypeId;
                                    pricingViewModel.HoleTypeId = holeId;
                                    SavePricing(pricingViewModel, uniqueSessionId);
                                }
                            };
                        }
                    };
                }
            }
        }

        public void MultipleBDRSavePricing(PricingViewModel pricingViewModel, long uniqueSessionId)
        {
            if (pricingViewModel.IsSpecialPricing == false)
            {
                if (pricingViewModel.MemberTypeArray.Count() != 0)
                {
                    foreach (int memberTypeId in pricingViewModel.MemberTypeArray)
                    {
                        if (pricingViewModel.DayArray.Count() != 0)
                        {
                            foreach (int dayId in pricingViewModel.DayArray)
                            {
                                if (pricingViewModel.TimeFormatArray.Count() != 0)
                                {
                                    foreach (int timeFormatId in pricingViewModel.TimeFormatArray)
                                    {
                                        PricingViewModel cc = SearchPricing(pricingViewModel);

                                        if (cc != null)
                                            pricingViewModel.PricingId = cc.PricingId;
                                        pricingViewModel.TimeFormatId = timeFormatId;
                                        pricingViewModel.DayTypeId = dayId;
                                        pricingViewModel.MemberTypeId = memberTypeId;
                                        SavePricing(pricingViewModel, uniqueSessionId);
                                    }
                                };

                            };

                        }
                    };
                }
            }
            else
            {

                if (pricingViewModel.MemberTypeArray.Count() != 0)
                {
                    foreach (int memberTypeId in pricingViewModel.MemberTypeArray)
                    {
                        if (pricingViewModel.SlotArray.Count() != 0)
                        {
                            foreach (int slotId in pricingViewModel.SlotArray)
                            {
                                pricingViewModel.SlotId = slotId;

                                SavePricing(pricingViewModel, uniqueSessionId);
                            }

                        }
                        else
                        {
                            pricingViewModel.MemberTypeId = memberTypeId;

                            SavePricing(pricingViewModel, uniqueSessionId);
                        }
                    };
                }
            }
        }
    }
}
