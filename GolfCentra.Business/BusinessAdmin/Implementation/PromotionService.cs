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
    public class PromotionService : IPromotionService
    {
        private readonly UnitOfWork _unitOfWork;

        public PromotionService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public List<PromotionViewModel> GetAllPromotion()
        {
            List<PromotionViewModel> promotionViewModels = new List<PromotionViewModel>();
            DateTime date = Core.Helper.DateHelper.ConvertSystemDate();
            List<Promotion> promotions = _unitOfWork.PromotionRepository.GetMany(x => x.IsActive == true).ToList();

            foreach (var promotion in promotions)
            {
                PromotionViewModel promotionViewModel = new PromotionViewModel()
                {
                    PromotionsId = promotion.PromotionsId,
                    Name = promotion.Name,
                    StartDate = promotion.StartDate,
                    EndDate = promotion.EndDate,
                    StartTime = promotion.StartTime.GetValueOrDefault(),
                    EndTime = promotion.EndTime.GetValueOrDefault(),
                    Extra = promotion.Extra,
                    HoleTypeId = promotion.HoleTypeId,
                    Price = promotion.Price,
                    CaddieFee = promotion.CaddieFee,
                    GreenFee = promotion.GreenFee,
                    CartFee = promotion.CartFee,
                    HoleTypeName = promotion.HoleType.Value.ToString()
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


        public void SavePromotion(PromotionViewModel promotionViewModel,long uniqueSessionId)
        {

            Promotion promotion = new Promotion()
            {
                StartDate = promotionViewModel.StartDate,
                EndDate = promotionViewModel.EndDate,
                HoleTypeId = promotionViewModel.HoleTypeId,
                EndTime = promotionViewModel.EndTime,
                StartTime = promotionViewModel.StartTime,
                Extra = promotionViewModel.Extra,
                GreenFee = promotionViewModel.GreenFee,
                CaddieFee = promotionViewModel.CaddieFee,
                CartFee = promotionViewModel.CartFee,
                Name = promotionViewModel.Name,
                Price = promotionViewModel.Price,
                PromotionsEquipmentMappings = SaveEquipment(promotionViewModel.EquipmentIds),
                PromotionsTaxMappings = SavePromotionsTax(promotionViewModel.Taxs),
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow

            };
            _unitOfWork.PromotionRepository.Insert(promotion);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Save Promotion",
                    ActionName = "Save",
                    PerformOn = promotion.PromotionsId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Save a Promotion with id- " + promotion.PromotionsId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
        }


        private List<PromotionsTaxMapping> SavePromotionsTax(long[] taxes)
        {
            List<PromotionsTaxMapping> promotionsTaxMappings = new List<PromotionsTaxMapping>();
            if (taxes != null)
            {
                foreach (var item in taxes)
                {
                    PromotionsTaxMapping promotionsTaxMapping = new PromotionsTaxMapping()
                    {
                        TaxId = item,
                        IsActive = true,
                        CreatedOn = System.DateTime.UtcNow
                    };
                    promotionsTaxMappings.Add(promotionsTaxMapping);
                }
            }
            return promotionsTaxMappings;

        }

        private List<PromotionsEquipmentMapping> SaveEquipment(long[] equipmentId)
        {
            List<PromotionsEquipmentMapping> promotionsEquipmentMappings = new List<PromotionsEquipmentMapping>();
            if (equipmentId != null)
            {
                foreach (var item in equipmentId)
                {
                    PromotionsEquipmentMapping promotionsEquipmentMapping = new PromotionsEquipmentMapping()
                    {
                        EquipmentId = item,
                        IsActive = true,
                        CreatedOn = System.DateTime.UtcNow
                    };
                    promotionsEquipmentMappings.Add(promotionsEquipmentMapping);
                }
            }
            return promotionsEquipmentMappings;

        }

        public bool DeletePromotion(long id,long uniqueSessionId)
        {
            Promotion promotion = _unitOfWork.PromotionRepository.Get(x => x.PromotionsId == id);
            promotion.IsActive = false;
            promotion.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.PromotionRepository.Update(promotion);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Delete Promotion",
                    ActionName = "Delete",
                    PerformOn = promotion.PromotionsId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Promotion with id- " + promotion.PromotionsId.ToString()
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
