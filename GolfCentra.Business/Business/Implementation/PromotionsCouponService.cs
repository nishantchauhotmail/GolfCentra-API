using GolfCentra.Business.Business.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Implementation
{
    public class PromotionsCouponService : IPromotionsCouponService
    {
        private readonly UnitOfWork _unitOfWork;

        public PromotionsCouponService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get Amount By Coupon Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CouponViewModel GetCouponAmountByCouponCode(string code)
        {
            CouponViewModel couponViewModel = new CouponViewModel();
            Coupon coupon = _unitOfWork.CouponRepository.Get(x => x.Code == code && x.IsActive == true);

            if (coupon == null) { throw new Exception("Invalid CouponCode."); }
            else
            {
                couponViewModel.Value = coupon.Amount;
                if (coupon.CouponTypeId == 1)
                {
                    couponViewModel.IsFlat = true;
                }
                else
                {
                    couponViewModel.IsFlat = false;
                }
                return couponViewModel;
            }
        }

        public List<PromotionViewModel> GetAllActivePromotion()
        {
            List<PromotionViewModel> promotionViewModels = new List<PromotionViewModel>();
            DateTime datex = Core.Helper.DateHelper.ConvertSystemDate();
            DateTime date = new DateTime(datex.Year, datex.Month, datex.Day);
          var date1x=  datex.AddDays(Core.Helper.Constants.BookingDetails.AdvanceBookingDays);
            DateTime date1 = new DateTime(date1x.Year, date1x.Month, date1x.Day,23,59,59);
            List<Promotion> promotions = _unitOfWork.PromotionRepository.GetMany(x => x.StartDate <= date1 && x.EndDate >= date && x.IsActive == true).ToList();

            foreach (var promotion in promotions)
            {
                PromotionViewModel promotionViewModel = new PromotionViewModel()
                {
                    PromotionsId = promotion.PromotionsId,
                    Name = promotion.Name,
                    StartDate = promotion.StartDate.ToShortDateString(),
                    EndDate = promotion.EndDate.ToShortDateString(),
                    StartTime = promotion.StartTime.GetValueOrDefault(),
                    EndTime = promotion.EndTime.GetValueOrDefault(),
                    Extra = promotion.Extra != null ? promotion.Extra :"",
                    HoleTypeId = promotion.HoleTypeId,
                    Price = promotion.Price,
                    CaddieFee = promotion.CaddieFee,
                    GreenFee = promotion.GreenFee,
                    CartFee = promotion.CartFee,
                    HoleTypeName= promotion.HoleType.Value.ToString()
                };
                foreach (var equ in promotion.PromotionsEquipmentMappings)
                {
                    if (promotionViewModel.EquipmentName != "" && promotionViewModel.EquipmentName != null)
                    {
                        promotionViewModel.EquipmentName = promotionViewModel.EquipmentName + "," + equ.Equipment.Name;
                    }
                    else
                    {
                        promotionViewModel.EquipmentName = equ.Equipment.Name;
                    }
                }

                promotionViewModels.Add(promotionViewModel);
            }

            return promotionViewModels;
        }

    }
}
