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
    public class SlotReservationService : ISlotReservationService
    {
        private readonly UnitOfWork _unitOfWork;

        public SlotReservationService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public List<BlockSlotRangeViewModel> GetSlotBlockRangeDetails()
        {
            DateTime date = Core.Helper.DateHelper.ConvertSystemDateToCurrent( DateTime.UtcNow);
            var date1 = new DateTime(date.Year, date.Month, date.Day);
            List<BlockSlotRange> blockSlotRange = _unitOfWork.BlockSlotRangeRepository.GetMany(x => x.IsActive == true && x.StartDate >= date1).ToList();

            List<BlockSlotRangeViewModel> blockSlotRangeViewModels = new List<BlockSlotRangeViewModel>();
            foreach (var item in blockSlotRange)
            {
                BlockSlotRangeViewModel blockSlotRangeViewModel = new BlockSlotRangeViewModel()
                {
                    BlockSlotRangeId = item.BlockSlotRangeId,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    SlotBlockReasonId = item.SlotBlockReasonId,
                    CoursePairingId = item.CoursePairingId.GetValueOrDefault(),
                    IsCustomerAvailable = item.IsCustomerAvailable,
                    IsBookingavailable = item.IsBookingavailable,
                    SlotBlockReason = item.Reason != null? item.Reason :"",
                   // CoursePairingName = item.CoursePairingId != null ? (item.CoursePairing.CourseName1 != null ? item.CoursePairing.CourseName1.Value : "") + (item.CoursePairing.CourseName != null ? " - " + item.CoursePairing.CourseName.Value : "") : "ALL",
                    BlockSlotViewModels = GetAllBlockSlotDetail(item.BlockSlotRangeId)
                    //  Id name
                    //List of BlockSlot
                };
                if (item.CoursePairingId==4 || item.CoursePairingId == 1 )
                {
                    blockSlotRangeViewModel.CoursePairingName = "Ridge";
                }
               else if (item.CoursePairingId == 5 || item.CoursePairingId == 2)
                {
                    blockSlotRangeViewModel.CoursePairingName = "Valley";
                }
              else  if (item.CoursePairingId == 6 || item.CoursePairingId == 3)
                {
                    blockSlotRangeViewModel.CoursePairingName = "Canyon";
                }
                else
                {
                    blockSlotRangeViewModel.CoursePairingName = "All";
                }

                blockSlotRangeViewModels.Add(blockSlotRangeViewModel);
            }
            return blockSlotRangeViewModels;
        }

        // Get data from block slot table by blockslotrangeID
        private List<BlockSlotViewModel> GetAllBlockSlotDetail(long blockSlotRangeId)
        {
            List<BlockSlotViewModel> blockSlotViewModels = new List<BlockSlotViewModel>();

            List<BlockSlot> blockSlots = _unitOfWork.BlockSlotRepository.GetMany(x => x.IsActive == true & x.BlockSlotRangeId == blockSlotRangeId).ToList();
            foreach (var item1 in blockSlots)
            {

                BlockSlotViewModel blockSlotViewModel = new BlockSlotViewModel()
                {
                    BlockSlotId = item1.BlockSlotId,
                    BlockSlotRangeId = item1.BlockSlotRangeId,
                    SlotSessionWiseId = item1.SlotSessionWiseId,
                    SlotTime = item1.SlotSessionWise.Slot.Time.ToString()
                };
                blockSlotViewModels.Add(blockSlotViewModel);
            }

            return blockSlotViewModels;

        }


        public List<BlockSlotViewModel> GetAllSlotDetails(DateTime startDate, DateTime endDate)
        {
            List<BlockSlotViewModel> blockSlotViewModels = new List<BlockSlotViewModel>();
            List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.IsActive == true && x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT).ToList();

            List<BlockSlotRange> blockSlotRanges = _unitOfWork.BlockSlotRangeRepository.GetMany(x => x.StartDate >= startDate && x.EndDate <= endDate && x.IsActive == true).ToList();
            foreach (BlockSlotRange blockSlotRange in blockSlotRanges)
            {
                List<BlockSlot> blockSlots = _unitOfWork.BlockSlotRepository.GetMany(x => x.IsActive == true && x.BlockSlotRangeId == blockSlotRange.BlockSlotRangeId).ToList();
                foreach (BlockSlot blockSlot in blockSlots)
                {
                    var itemToRemove = slotSessionWises.SingleOrDefault(r => r.SlotSessionWiseId == blockSlot.SlotSessionWiseId);
                    if (itemToRemove != null)
                    {
                        slotSessionWises.Remove(itemToRemove);
                        BlockSlotViewModel blockSlotViewModel = new BlockSlotViewModel()
                        {
                            SlotSessionWiseId = blockSlot.SlotSessionWiseId,
                            SlotTime = itemToRemove.Slot.Time.ToString(),
                            IsAlreadyDisabled = true
                        };
                        blockSlotViewModels.Add(blockSlotViewModel);
                    }
                }
            }
            foreach (var slotSessioWise in slotSessionWises)
            {
                BlockSlotViewModel blockSlotViewModel = new BlockSlotViewModel()
                {
                    SlotSessionWiseId = slotSessioWise.SlotSessionWiseId,
                    SlotTime = slotSessioWise.Slot.Time.ToString(),
                    IsAlreadyDisabled = false
                };
                blockSlotViewModels.Add(blockSlotViewModel);
            }

            return blockSlotViewModels.OrderBy(x=>x.SlotTime).ToList();
        }

        public void SaveBlockSlotRangeDetails(BlockSlotRangeViewModel blockSlotRangeViewModel,long uniqueSessionId)
        {
            foreach (long id in blockSlotRangeViewModel.CoursePairingIds)
            {
                blockSlotRangeViewModel.CoursePairingId = id;
                BlockSlotRange blockSlotRange = new BlockSlotRange()
                {
                    StartDate = blockSlotRangeViewModel.StartDate,
                    EndDate = blockSlotRangeViewModel.EndDate,
                    IsBookingavailable = blockSlotRangeViewModel.IsBookingavailable,
                    IsCustomerAvailable = blockSlotRangeViewModel.IsCustomerAvailable,
                    SlotBlockReasonId = 1,
                    CoursePairingId = blockSlotRangeViewModel.CoursePairingId,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow,
                    BlockSlots = ConvertIdToDatabaseBlockSlot(blockSlotRangeViewModel.DisabledSlot),
                    Reason = blockSlotRangeViewModel.SlotBlockReason,
                };
                if (blockSlotRangeViewModel.CoursePairingId == 999)
                {
                    blockSlotRange.CoursePairingId = null;
                }
                _unitOfWork.BlockSlotRangeRepository.Insert(blockSlotRange);
                _unitOfWork.Save();
                try
                {

                    SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                    {
                        ControllerName = "Block Slot",
                        ActionName = "Save",
                        PerformOn = blockSlotRange.BlockSlotRangeId.ToString(),
                        LoginHistoryId = uniqueSessionId,

                        Info = "Created a Block Slot with id- " + blockSlotRange.BlockSlotRangeId.ToString()
                    };
                    new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

                }
                catch (Exception)
                {

                }
            }
        }

        private List<BlockSlot> ConvertIdToDatabaseBlockSlot(List<long> ids)
        {
            List<BlockSlot> blockSlots = new List<BlockSlot>();
            foreach (long id in ids)
            {
                BlockSlot blockSlot = new BlockSlot()
                {
                    SlotSessionWiseId = id,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow
                };
                blockSlots.Add(blockSlot);
            }
            return blockSlots;
        }

        public bool DeleteBlockSlotRangeDetails(long id,long uniqueSessionId)
        {

            BlockSlotRange slotDB = _unitOfWork.BlockSlotRangeRepository.Get(x => x.BlockSlotRangeId == id && x.IsActive == true);
            if (slotDB == null)
                throw new Exception("Block Slot Not Exist");
            slotDB.IsActive = false;
            slotDB.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.BlockSlotRangeRepository.Update(slotDB);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Block Slot",
                    ActionName = "Delete",
                    PerformOn = id.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Slot Block with id- " + id.ToString() 
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
