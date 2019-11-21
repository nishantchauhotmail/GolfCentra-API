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
    public class TaxManagementService:ITaxManagementService
    {
        private readonly UnitOfWork _unitOfWork;

        public TaxManagementService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Tax Type
        /// </summary>
        /// <returns></returns>
        public List<TaxManagementViewModel> GetAllTaxType()
        {
            List<TaxManagementViewModel> taxManagementViewModels = new List<TaxManagementViewModel>();
            List<Tax> taxes = _unitOfWork.TaxRepository.GetMany(x => x.IsActive == true).ToList();

            foreach (var item in taxes)
            {
                TaxManagementViewModel taxManagementViewModel = new TaxManagementViewModel()
                {
                    TaxId = item.TaxId,
                    Name = item.Name,
                    Percentage = item.Percentage
                };
                taxManagementViewModels.Add(taxManagementViewModel);
            }
            return taxManagementViewModels;
        }
       
        /// <summary>
        /// Save Tax Type
        /// </summary>
        /// <param name="memberTypeName"></param>
        /// <param name="memberTypeValue"></param>
        /// <returns></returns>
        public bool SaveTaxTypeDetails(string taxTypeName, decimal percentage, long uniqueSessionId)
        {

            Tax taxTypeDB = _unitOfWork.TaxRepository.Get(x => x.Name.Trim().ToLower() == taxTypeName.Trim().ToLower() && x.IsActive == true);
            if (taxTypeDB != null)
                throw new Exception("Tax Type Already Exist");
            Tax tax = new Tax()
            {
                Name = taxTypeName,

                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                Percentage = percentage
            };
            _unitOfWork.TaxRepository.Insert(tax);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Tax Type",
                    ActionName = "Save",
                    PerformOn = tax.TaxId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Tax with id- " + tax.TaxId.ToString() + ". Tax had following details " + Environment.NewLine + "Name " + tax.Name + "" + Environment.NewLine + " Value " + tax.Percentage
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }

            return true;
        }

        /// <summary>
        /// Update Tax Type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberType"></param>
        /// <param name="valueToShow"></param>
        /// <returns></returns>
        public bool UpdateTaxTypeDetails(long id, string taxTypeName, decimal percentage, long uniqueSessionId)
        {
           
            Tax taxDB1 = _unitOfWork.TaxRepository.Get(x => x.Name.Trim().ToLower() == taxTypeName.Trim().ToLower() && x.IsActive == true);
            if (taxDB1 != null)
                throw new Exception("Tax Type Already Exist");
            Tax taxDB = _unitOfWork.TaxRepository.Get(x => x.TaxId == id && x.IsActive == true);
            if (taxDB == null)
                throw new Exception("Tax Type  Not Exist");
            
            taxDB.Percentage= percentage;
            taxDB.Name = taxTypeName;
            _unitOfWork.TaxRepository.Update(taxDB);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Tax Type",
                    ActionName = "Update",
                    PerformOn = taxDB.TaxId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a Tax with id- " + taxDB.TaxId.ToString() + ". Tax had following details " + Environment.NewLine + "Name " + taxDB.Name + "" + Environment.NewLine + " Value " + taxDB.Percentage
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Delete Tax Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTaxTypeDetails(long id, long uniqueSessionId)
        {

            Tax taxDB = _unitOfWork.TaxRepository.Get(x => x.TaxId == id && x.IsActive == true);
            if (taxDB == null)
                throw new Exception("Tax Type Not Exist");
            taxDB.IsActive = false;
            taxDB.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.TaxRepository.Update(taxDB);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Tax Type",
                    ActionName = "Delete",
                    PerformOn = taxDB.TaxId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Tax with id- " + taxDB.TaxId.ToString() + ". Tax had following details " + Environment.NewLine + "Name " + taxDB.Name + "" + Environment.NewLine + " Value " + taxDB.Percentage
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
