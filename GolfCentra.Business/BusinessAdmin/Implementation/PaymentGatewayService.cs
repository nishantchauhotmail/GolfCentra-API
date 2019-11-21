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
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly UnitOfWork _unitOfWork;

        public PaymentGatewayService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public void SavePaymentGatewayControl(PaymentGatewayControlViewModel paymentGatewayControlViewModel,long uniqueSessionId)
        {
            PaymentGatewayControl paymentGatewayControl = new PaymentGatewayControl()
            {
                PaymentGatewayEnable = paymentGatewayControlViewModel.PaymentGatewayEnable,
                AllMembers = paymentGatewayControlViewModel.AllMembers,
                CaddieFee = paymentGatewayControlViewModel.CaddieFee,
                CartFee = paymentGatewayControlViewModel.CartFee,
                GreenFee = paymentGatewayControlViewModel.GreenFee,
                CreatedOn = System.DateTime.UtcNow,
                IsActive = true,
                SelectedGolferIds = paymentGatewayControlViewModel.SelectedGolferIds,
                EquipmentOffIds = paymentGatewayControlViewModel.EquipmentPriceOffIds
            };
            _unitOfWork.PaymentGatewayControlRepository.Insert(paymentGatewayControl);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Save Payment Gateway",
                    ActionName = "Save",
                    PerformOn = paymentGatewayControl.PaymentGatewayControlId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Payment Gateway Control with id- " + paymentGatewayControl.PaymentGatewayControlId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
        }


        public List<PaymentGatewayControlViewModel> SearchAllPaymentGatewayControl()
        {
            List<PaymentGatewayControlViewModel> paymentGatewayControlViewModels = new List<PaymentGatewayControlViewModel>();
            List<PaymentGatewayControl> paymentGatewayControls = _unitOfWork.PaymentGatewayControlRepository.GetMany(x => x.IsActive == true).ToList();

            foreach (PaymentGatewayControl payment in paymentGatewayControls)
            {
                PaymentGatewayControlViewModel paymentGatewayControlViewModel = new PaymentGatewayControlViewModel()
                {
                    PaymentGatewayEnable = payment.PaymentGatewayEnable,
                    AllMembers = payment.AllMembers,
                    CaddieFee = payment.CaddieFee,
                    CartFee = payment.CartFee,
                    GreenFee = payment.GreenFee,
                    EquipmentName = GetAllEquipmentName(payment.EquipmentOffIds),
                    GolferNames = GetAllGolferName(payment.SelectedGolferIds),
                    PaymentGatewayControlId=payment.PaymentGatewayControlId
                };
                paymentGatewayControlViewModels.Add(paymentGatewayControlViewModel);
            }
            return paymentGatewayControlViewModels;
        }

        public string GetAllEquipmentName(string ids)
        {
            string equipmentName = "";
            string[] equipmentIds = ids != null ? ids.Split(',') : new string[0];
            foreach (var id in equipmentIds)
            {
                long equipmentId = Convert.ToInt64(id);
                Equipment equipment = _unitOfWork.EquipmentRepository.Get(x => x.EquipmentId == equipmentId);
                if (equipment != null)
                {
                    if (equipmentName != "")
                    {
                        equipmentName = equipmentName + "," + equipment.Name;
                    }
                    else
                    {

                        equipmentName = equipment.Name;
                    }

                }
            }
            return equipmentName;
        }

        public string GetAllGolferName(string ids)
        {
            string golferName = "";
            string[] golferIds = ids !=null ?ids.Split(',') : new string[0];
            foreach (var id in golferIds)
            {
                long golferId = Convert.ToInt64(id);
                Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
                if (golfer != null)
                {
                    if (golferName != "")
                    {
                        golferName = golferName + "," + golfer.Name + " ( " + golfer.ClubMemberId + " )";
                    }
                    else
                    {
                        golferName = golfer.Name + " ( " + golfer.ClubMemberId + " )";
                    }

                }
            }
            return golferName;
        }

        public bool DeletePaymentGatewayControl(long id,long uniqueSessionId)
        {
         PaymentGatewayControl paymentGatewayControl=   _unitOfWork.PaymentGatewayControlRepository.Get(x => x.PaymentGatewayControlId == id);
            paymentGatewayControl.IsActive = false;
            paymentGatewayControl.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.PaymentGatewayControlRepository.Update(paymentGatewayControl);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Deleted Payment Gateway",
                    ActionName = "Save",
                    PerformOn = paymentGatewayControl.PaymentGatewayControlId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Payment Gateway Control with id- " + paymentGatewayControl.PaymentGatewayControlId.ToString()
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
