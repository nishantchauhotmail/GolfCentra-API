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
    public class TimeSlotService : ITimeSlotService
    {
        private readonly UnitOfWork _unitOfWork;

        public TimeSlotService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Time Slot Detail
        /// </summary>
        /// <returns></returns>
        public List<SlotViewModel> GetAllTimeSlot()
        {
            List<SlotViewModel> slotViewModels = new List<SlotViewModel>();
            List<Slot> slots = _unitOfWork.SlotRepository.GetMany(x => x.IsActive == true).ToList();

            foreach (var item in slots)
            {
                SlotViewModel slotViewModel = new SlotViewModel()
                {
                    Time = item.Time,
                    SlotId = item.SlotId
                };
                slotViewModels.Add(slotViewModel);
            }
            return slotViewModels;
        }

        /// <summary>
        /// Save All Time Slot Detail
        /// </summary>
        /// <param name="slotTime"></param>
        /// <returns></returns>
        public bool SaveTimeSlotDetails(TimeSpan slotTime, long uniqueSessionId)
        {


            Slot slotDB = _unitOfWork.SlotRepository.Get(x => x.Time == slotTime && x.IsActive == true);
            if (slotDB != null)
                throw new Exception("Slot Time Already Exist");
            Slot slot = new Slot()
            {
                Time = slotTime,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow
            };
            _unitOfWork.SlotRepository.Insert(slot);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Time Slot",
                    ActionName = "Save",
                    PerformOn = slot.SlotId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Slot with id- " + slot.SlotId.ToString() + ". Slot had following details" + Environment.NewLine + "Time " + slot.Time
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Update All Time Slot Detail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="slotTime"></param>
        /// <returns></returns>
        public bool UpdateTimeSlotDetails(long id, TimeSpan slotTime, long uniqueSessionId)
        {
           
            Slot slotDB1 = _unitOfWork.SlotRepository.Get(x => x.Time == slotTime && x.IsActive == true);
            if (slotDB1 != null)
                throw new Exception("Slot Time Already Exist");
            Slot slotDB = _unitOfWork.SlotRepository.Get(x => x.SlotId == id && x.IsActive == true);
            if (slotDB == null)
                throw new Exception("Slot Time Not Exist");
            slotDB.Time = slotTime;
            _unitOfWork.SlotRepository.Update(slotDB);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Time Slot",
                    ActionName = "Update",
                    PerformOn = slotDB.SlotId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a Slot with id- " + slotDB.SlotId.ToString() + ". Slot had following details" + Environment.NewLine + "Time " + slotTime
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Delete All Time Slot Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTimeSlotDetails(long id, long uniqueSessionId)
        {

            Slot slotDB = _unitOfWork.SlotRepository.Get(x => x.SlotId == id && x.IsActive == true);
            if (slotDB == null)
                throw new Exception("Slot Time Not Exist");
            slotDB.IsActive = false;


            slotDB.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.SlotRepository.Update(slotDB);
            _unitOfWork.Save();

            DeletedSlotSessionWise(id);

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Time Slot",
                    ActionName = "Delete",
                    PerformOn = slotDB.SlotId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Slot with id- " + slotDB.SlotId.ToString() + ". Slot had following details " + Environment.NewLine + "Time " + slotDB.Time
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;

        }

        private bool DeletedSlotSessionWise(long id)
        {
            List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.SlotId == id).ToList();
            foreach (SlotSessionWise slotSessionWise in slotSessionWises)
            {
                slotSessionWise.IsActive = false;
                slotSessionWise.UpdatedOn = System.DateTime.UtcNow;
                _unitOfWork.SlotSessionWiseRepository.Update(slotSessionWise);
                _unitOfWork.Save();

                List<BlockSlot> blockSlots = _unitOfWork.BlockSlotRepository.GetMany(x => x.SlotSessionWiseId == slotSessionWise.SlotSessionWiseId).ToList();
                foreach (BlockSlot blockSlot in blockSlots)
                {
                    blockSlot.IsActive = false;
                    blockSlot.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.BlockSlotRepository.Update(blockSlot);
                    _unitOfWork.Save();

                }
            }
            return true;

        }


    }
}
