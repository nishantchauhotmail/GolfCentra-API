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
   public class CoursePairingService: ICoursePairingService
    {
        private readonly UnitOfWork _unitOfWork;

        public CoursePairingService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public List<CoursePairingViewModel> GetALLCoursePairing()
        {
            List<CoursePairing> coursePairings = _unitOfWork.CoursePairingRepository.GetMany(x => x.IsActive == true).ToList();

            List<CoursePairingViewModel> coursePairingViewModels = new List<CoursePairingViewModel>();
            foreach (var item in coursePairings)
            {
                CoursePairingViewModel coursePairingViewModel = new CoursePairingViewModel()
                {
                    CoursePairingId = item.CoursePairingId,
                    StartCourseNameId = item.StartCourseNameId,
                    EndCourseNameId=item.EndCourseNameId,
                    HoleTypeId=item.HoleTypeId
                };
                coursePairingViewModels.Add(coursePairingViewModel);
            }
            return coursePairingViewModels;
        }
    }
}
