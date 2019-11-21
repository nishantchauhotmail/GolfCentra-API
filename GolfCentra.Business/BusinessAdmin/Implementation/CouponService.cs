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
    public class CouponService : ICouponService
    {
        private readonly UnitOfWork _unitOfWork;

        public CouponService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Save All Coupon Type Detail
        /// </summary>
        /// <param name="couponViewModel"></param>
        /// <returns></returns>
        public bool SaveCouponDetail(CouponViewModel couponViewModel, long uniqueSessionId)
        {

            Coupon couponName = _unitOfWork.CouponRepository.Get(x => x.Name == couponViewModel.Name && x.IsActive == true);
            if (couponName != null)
                throw new Exception("Coupon Name Already Exist.");
            Coupon couponCode = _unitOfWork.CouponRepository.Get(x => x.Code == couponViewModel.Code && x.IsActive == true);
            if (couponCode != null)
                throw new Exception("Coupon Code Already Exist.");
            Coupon coupon = new Coupon()
            {
                Name = couponViewModel.Name,
                Code = couponViewModel.Code,
                Amount = couponViewModel.Amount,
                CurrencyId = 1,
                IsActive = couponViewModel.Status,
                CreatedOn = System.DateTime.UtcNow,
                CouponTypeId = couponViewModel.CouponTypeId
            };

            _unitOfWork.CouponRepository.Insert(coupon);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Coupon",
                    ActionName = "Save",
                    PerformOn = coupon.CouponId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Coupon with Coupon id- " + coupon.CouponId.ToString() + ". Coupon had following details <br/>Name " + coupon.Name + "  <br/> Type " + coupon.CouponType.Name + "  <br/> Code " + coupon.Code + "  <br/> Value " + coupon.Amount 
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }

            return true;
        }

        public List<CouponViewModel> GetAllActiveCoupon()
        {
            List<CouponViewModel> couponViewModels = new List<CouponViewModel>();
            List<Coupon> coupons = _unitOfWork.CouponRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var item in coupons)
            {
                CouponViewModel couponViewModel = new CouponViewModel()
                {
                    CouponId = item.CouponId,
                    Code = item.Code,
                    Name = item.Name,
                    Amount = item.Amount,
                    CouponTypeId = item.CouponTypeId.GetValueOrDefault()
                };
                couponViewModels.Add(couponViewModel);
            }
            return couponViewModels;
        }

        public bool DeleteCoupon(long couponId,long uniqueSessionId)
        {
            Coupon coupon = _unitOfWork.CouponRepository.Get(x => x.CouponId == couponId);
            if (coupon == null)
                throw new Exception("Coupon Not Found");

            coupon.IsActive = false;
            coupon.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.CouponRepository.Update(coupon);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Coupon",
                    ActionName = "Delete",
                    PerformOn = coupon.CouponId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Coupon with Coupon id- " + coupon.CouponId.ToString() + ". Coupon had following details <br/>Name " + coupon.Name + "  <br/> Type " + coupon.CouponType.Name + "  <br/> Code " + coupon.Code + "  <br/> Value " + coupon.Amount
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }
    }
}
