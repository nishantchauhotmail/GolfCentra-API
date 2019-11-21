
using GolfCentra.Business.BusinessAdmin.Interface;
using System;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
   public class SlotBlockReasonService:ISlotBlockReasonService
    {
        private readonly UnitOfWork _unitOfWork;

        public SlotBlockReasonService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public List<SlotBlockReasonViewModel> GetALLSlotBlockReason()
        {
            List<SlotBlockReason> slotBlockReasons = _unitOfWork.SlotBlockReasonRepository.GetMany(x => x.IsActive == true).ToList();

            List<SlotBlockReasonViewModel> slotBlockReasonViewModels = new List<SlotBlockReasonViewModel>();
            foreach (var item in slotBlockReasons)
            {
                SlotBlockReasonViewModel slotBlockReasonViewModel = new SlotBlockReasonViewModel()
                {
                    SlotBlockReasonId = item.SlotBlockReasonId,
                    Value = item.Value
                };
                slotBlockReasonViewModels.Add(slotBlockReasonViewModel);
            }
            return slotBlockReasonViewModels;
        }

    }
}
