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
    public class GenderTypeService : IGenderTypeService
    {

        private readonly UnitOfWork _unitOfWork;

        public GenderTypeService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Gender Type Detail
        /// </summary>
        /// <returns></returns>
        public List<GenderTypeViewModel> GetALLGenderType()
        {
            List<GenderType> genderTypes = _unitOfWork.GenderTypeRepository.GetMany(x => x.IsActive == true).ToList();

            List<GenderTypeViewModel> genderTypeViewModels = new List<GenderTypeViewModel>();
            foreach (var item in genderTypes)
            {
                GenderTypeViewModel genderTypeViewModel = new GenderTypeViewModel() {
                    GenderTypeId=item.GenderTypeId,
                    Value=item.Value
                };
                genderTypeViewModels.Add(genderTypeViewModel);
            }
            return genderTypeViewModels;
        }

    }
}
