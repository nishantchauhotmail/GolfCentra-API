using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class MaritalStatusService:IMaritalStatusService
    {
        private readonly UnitOfWork _unitOfWork;

        public MaritalStatusService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Marital status Detail
        /// </summary>
        /// <returns></returns>
        public List<MaritalStatusViewModel> GetAllMaritalStatusType()
        {
            List<MaritalStatu> genderTypes = _unitOfWork.MaritalStatuRepository.GetMany(x => x.IsActive == true).ToList();

            List<MaritalStatusViewModel> maritalStatusViewModels = new List<MaritalStatusViewModel>();
            foreach (var item in genderTypes)
            {
                MaritalStatusViewModel maritalStatusViewModel = new MaritalStatusViewModel()
                {
                    MaritalStatusId = item.MaritalStatusId,
                    Value = item.Value
                };
                maritalStatusViewModels.Add(maritalStatusViewModel);
            }
            return maritalStatusViewModels;
        }
    }
}
