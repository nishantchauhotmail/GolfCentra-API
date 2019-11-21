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
 public   class TimeFormatService : ITimeFormatService
    {
        private readonly UnitOfWork _unitOfWork;

        public TimeFormatService()
        {
            _unitOfWork = new UnitOfWork();
        }


        /// <summary>
        /// Get All Time Format status Detail
        /// </summary>
        /// <returns></returns>
        public List<TimeFormatViewModel> GetAllTimeFormat()
        {
            List<TimeFormat> genderTypes = _unitOfWork.TimeFormatRepository.GetMany(x => x.IsActive == true).ToList();

            List<TimeFormatViewModel> maritalStatusViewModels = new List<TimeFormatViewModel>();
            foreach (var item in genderTypes)
            {
                TimeFormatViewModel maritalStatusViewModel = new TimeFormatViewModel()
                {
                    TimeFormatId = item.TimeFormatId,
                    Name = item.Name
                };
                maritalStatusViewModels.Add(maritalStatusViewModel);
            }
            return maritalStatusViewModels;
        }
    }
}
