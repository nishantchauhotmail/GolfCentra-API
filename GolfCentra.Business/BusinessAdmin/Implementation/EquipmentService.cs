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
   public class EquipmentService:IEquipmentService
    {
        private readonly UnitOfWork _unitOfWork;
        public EquipmentService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Equipment Type
        /// </summary>
        /// <returns></returns>
        public List<EquipmentViewModel> GetAllEquipmentType()
        {
            List<EquipmentViewModel> equipmentViewModels = new List<EquipmentViewModel>();

            List<Equipment> equipments = _unitOfWork.EquipmentRepository.GetMany(x => x.IsActive == true).ToList();

            foreach (var item in equipments)
            {
                EquipmentViewModel equipmentViewModel = new EquipmentViewModel()
                {
                    EquipmentId = item.EquipmentId,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description
                };

              
                List<EquipmentTaxMappingViewModel> equipmentTaxMappingViewModels = new List<EquipmentTaxMappingViewModel>();

                List<EquipmentTaxMapping> equipmentTaxMappings = _unitOfWork.EquipmentTaxMappingRepository.GetMany(x => x.IsActive == true& x.EquipmentId==item.EquipmentId).ToList();
                equipmentViewModel.Taxs = new long[equipmentTaxMappings.Count];
                int count = 0;
                foreach (var item1 in equipmentTaxMappings)
                {
                    equipmentViewModel.Taxs[count] = item1.TaxId;
                    EquipmentTaxMappingViewModel equipmentTaxMappingViewModel = new EquipmentTaxMappingViewModel()
                    {
                        TaxId = item1.TaxId,
                      TaxName=  item1.Tax.Name,
                      TaxPercentage=item1.Tax.Percentage
                    };
                    equipmentTaxMappingViewModels.Add(equipmentTaxMappingViewModel);
                    count ++;
                };

                equipmentViewModel.EquipmentTaxMappingViewModels = equipmentTaxMappingViewModels;
                equipmentViewModels.Add(equipmentViewModel); 
            }
        
            return equipmentViewModels;
        }

        /// <summary>
        /// Save Equipment Type Detail
        /// </summary>
        /// <param name="equipmentViewModel"></param>
        /// <returns></returns>
        public bool SaveEquipmentDetails(EquipmentViewModel equipmentViewModel,long uniqueSessionId)
        {

            Equipment equipmentDB = _unitOfWork.EquipmentRepository.Get(x => x.Name.Trim().ToLower() == equipmentViewModel.Name.Trim().ToLower() && x.IsActive == true);
            if (equipmentDB != null)
                throw new Exception("Equipment Already Exist");
            Equipment equipment = new Equipment()
            {
                Name = equipmentViewModel.Name,

                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                Price = equipmentViewModel.Price,
                Description=equipmentViewModel.Description
            };
            //List<EquipmentTaxMappingViewModel> equipmentTaxMappingViewModels = new List<EquipmentTaxMappingViewModel>();
            //  List<EquipmentTaxMapping> equipmentTaxMappings = _unitOfWork.EquipmentTaxMappingRepository.GetMany(x => x.IsActive == true & x.EquipmentId == item.EquipmentId).ToList();
            _unitOfWork.EquipmentRepository.Insert(equipment);
            _unitOfWork.Save();
            if (equipmentViewModel.Taxs != null)
            {
                foreach (var items in equipmentViewModel.Taxs)
                {
                    EquipmentTaxMapping equipmentTaxMapping = new EquipmentTaxMapping()
                    {
                        TaxId = items,
                        EquipmentId = equipment.EquipmentId,
                        IsActive = true,
                        CreatedOn = System.DateTime.UtcNow
                    };
                    _unitOfWork.EquipmentTaxMappingRepository.Insert(equipmentTaxMapping);
                    _unitOfWork.Save();
                }
            }
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Save Equipment ",
                    ActionName = "Save",
                    PerformOn = equipment.EquipmentId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Equipment with id- " + equipment.EquipmentId.ToString() 
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Update Equipment Type Detail
        /// </summary>
        /// <param name="equipmentViewModel"></param>
        /// <returns></returns>
        public bool UpdateEquipmentDetails(EquipmentViewModel equipmentViewModel,long uniqueSessionId)
        {
            Equipment equipmentDB = _unitOfWork.EquipmentRepository.Get(x => x.EquipmentId == equipmentViewModel.EquipmentId && x.IsActive == true);
            if (equipmentDB == null)
                throw new Exception("Equipment Type  Not Exist");
            Equipment equipmentDB1 = _unitOfWork.EquipmentRepository.Get(x => x.Name.Trim().ToLower() == equipmentViewModel.Name.Trim().ToLower() && x.IsActive == true);
            if (equipmentDB1 != null && equipmentDB1.Name.Trim().ToLower() != equipmentDB.Name.Trim().ToLower())
                throw new Exception("Equipment Already Exist");
         
            equipmentDB.Description = equipmentViewModel.Description;
            equipmentDB.Price = equipmentViewModel.Price;
            equipmentDB.Name = equipmentViewModel.Name;
            _unitOfWork.EquipmentRepository.Update(equipmentDB);
            _unitOfWork.Save();
            List<EquipmentTaxMappingViewModel> equipmentTaxMappingViewModels = new List<EquipmentTaxMappingViewModel>();
            List<EquipmentTaxMapping> equipmentTaxMappings = _unitOfWork.EquipmentTaxMappingRepository.GetMany(x => x.IsActive == true & x.EquipmentId == equipmentViewModel.EquipmentId).ToList();
            foreach (var item1 in equipmentTaxMappings)
            {
                item1.IsActive = false;
                _unitOfWork.EquipmentTaxMappingRepository.Update(item1);
                _unitOfWork.Save();
            };

            if (equipmentViewModel.Taxs != null)
            {
                foreach (var items in equipmentViewModel.Taxs)
                {
                    EquipmentTaxMapping equipmentTaxMapping = new EquipmentTaxMapping()
                    {
                        TaxId = items,
                        EquipmentId = equipmentViewModel.EquipmentId,
                        IsActive = true,
                        CreatedOn = System.DateTime.UtcNow
                    };
                    _unitOfWork.EquipmentTaxMappingRepository.Insert(equipmentTaxMapping);
                    _unitOfWork.Save();
                }
            }
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Update Equipment ",
                    ActionName = "Update",
                    PerformOn = equipmentViewModel.EquipmentId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a Equipment with id- " + equipmentViewModel.EquipmentId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }



            return true;
        }

        /// <summary>
        /// Delete Equipment Type Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEquipmentDetails(long id,long uniqueSessionId)
        {

            Equipment equipmentDB = _unitOfWork.EquipmentRepository.Get(x => x.EquipmentId == id && x.IsActive == true);
            if (equipmentDB == null)
                throw new Exception("Equipment Not Exist");
            equipmentDB.IsActive = false;
            equipmentDB.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.EquipmentRepository.Update(equipmentDB);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Delete Equipment ",
                    ActionName = "Delete",
                    PerformOn = id.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Equipment with id- " + id.ToString()
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
