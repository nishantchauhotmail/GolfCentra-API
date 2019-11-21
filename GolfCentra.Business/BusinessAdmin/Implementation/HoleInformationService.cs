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
    public class HoleInformationService : IHoleInformationService
    {
        private readonly UnitOfWork _unitOfWork;

        public HoleInformationService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Hole Details
        /// </summary>
        /// <returns></returns>
        public List<HoleViewModel> GetHoleNumberList()
        {
            List<HoleViewModel> holeViewModels = new List<HoleViewModel>();
            List<HoleNumber> holeNumbers = _unitOfWork.HoleNumberRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var item in holeNumbers)
            {
                HoleViewModel holeViewModel = new HoleViewModel()
                {
                    HoleNumberId = item.HoleNumberId,
                    HoleNumber = item.Value
                };

                holeViewModels.Add(holeViewModel);
            }
            return holeViewModels;
        }

        /// <summary>
        /// Get HoleNumber List Detail By HoleNumberId
        /// </summary>
        /// <param name="holeNumberId"></param>
        /// <returns></returns>
        public HoleViewModel GetHoleDetailsByHoleNumberId(long holeNumberId)
        {
            List<TeeViewModel> teeViewModels = new List<TeeViewModel>();
            HoleInformation holeInformation = _unitOfWork.HoleInformationRepository.Get(x => x.HoleNumberId == holeNumberId && x.IsActive == true);
            List<Tee> tees = _unitOfWork.TeeRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var item in tees)
            {
                HoleTeeYardage holeTeeYardages = _unitOfWork.HoleTeeYardageRepository.Get(x => x.HoleNumberId == holeNumberId && x.IsActive == true && x.TeeId == item.TeeId);

                TeeViewModel teeViewModel = new TeeViewModel()
                {
                    TeeId = item.TeeId,
                    TeeName = item.Name,
                    Yardage = holeTeeYardages != null ? holeTeeYardages.Yardage : 0,
                    HoleTeeYardageId = holeTeeYardages != null ? holeTeeYardages.HoleTeeYardageId : 0
                };

                teeViewModels.Add(teeViewModel);
            }
            HoleViewModel holeViewModel = new HoleViewModel()
            {
                HoleInformationId = holeInformation != null ? holeInformation.HoleInformationId : 0,
                Latitude = holeInformation != null ? holeInformation.Latitude : 0,
                Longitude = holeInformation != null ? holeInformation.Longitude : 0,
                HoleNumberId = holeNumberId,
                TeeViewList = teeViewModels
            };
            return holeViewModel;
        }
      
        /// <summary>
        /// Save and Update Hole Information
        /// </summary>
        /// <param name="holeViewModel"></param>
        public void SaveUpdateHoleInformation(HoleViewModel holeViewModel,long uniqueSessionId)
        {
            HoleInformation holeInformation = _unitOfWork.HoleInformationRepository.Get(x => x.HoleNumberId == holeViewModel.HoleNumberId && x.IsActive == true);
            if (holeInformation == null)
            {
                HoleInformation holeInformations = new HoleInformation()
                {
                    Latitude = holeViewModel.Latitude,
                    Longitude = holeViewModel.Longitude,
                    ImgUrl = "",
                    CreatedOn = System.DateTime.UtcNow,
                    IsActive = true,
                    HoleNumberId = holeViewModel.HoleNumberId
                };
                _unitOfWork.HoleInformationRepository.Insert(holeInformations);
            }
            else
            {
                holeInformation.Latitude = holeViewModel.Latitude;
                holeInformation.Longitude = holeViewModel.Longitude;
                _unitOfWork.HoleInformationRepository.Update(holeInformation);
            }

            foreach (var item in holeViewModel.TeeViewList)
            {
                HoleTeeYardage holeTeeYardage = _unitOfWork.HoleTeeYardageRepository.Get(x => x.HoleNumberId == holeViewModel.HoleNumberId && x.IsActive == true && x.TeeId == item.TeeId);
                if (holeTeeYardage == null)
                {
                    HoleTeeYardage holeTeeYardagedb = new HoleTeeYardage() {
                        TeeId=item.TeeId,
                        HoleNumberId=holeViewModel.HoleNumberId,
                        Yardage=item.Yardage,
                        IsActive=true,
                        CreatedOn=System.DateTime.UtcNow,
                        
                    };
                    _unitOfWork.HoleTeeYardageRepository.Insert(holeTeeYardagedb);

                }
                else
                {

                    holeTeeYardage.Yardage = item.Yardage;
                    _unitOfWork.HoleTeeYardageRepository.Update(holeTeeYardage);
                }
            }

            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Hole Information ",
                    ActionName = "Update",
                    PerformOn = holeViewModel.HoleNumberId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a Information at hole number- " + holeViewModel.HoleNumberId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
        }
      
        /// <summary>
        /// Get All Hole Type Detail
        /// </summary>
        /// <returns></returns>
        public List<HoleTypeViewModel> GetAllHoleType()
        {
            List<HoleTypeViewModel> holeTypeViewModels = new List<HoleTypeViewModel>();
            List<HoleType> holeTypes = _unitOfWork.HoleTypeRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (HoleType btype in holeTypes)
            {
                HoleTypeViewModel holeTypeViewModel = new HoleTypeViewModel()
                {
                    Name = btype.Value.ToString(),
                    HoleTypeId = btype.HoleTypeId
                };
                holeTypeViewModels.Add(holeTypeViewModel);

            }
            return holeTypeViewModels;

        }
    }
}
