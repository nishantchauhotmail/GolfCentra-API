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
    public class BucketService : IBucketService
    {
        private readonly UnitOfWork _unitOfWork;

        public BucketService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Bucket Detail
        /// </summary>
        /// <returns></returns>
        public List<BucketViewModel> GetAllBucketType()
        {
            List<BucketViewModel> bucketViewModels = new List<BucketViewModel>();
            List<BucketDetail> bucketDetails = _unitOfWork.BucketDetailRepository.GetMany(x => x.IsActive == true && x.IsActive == true).ToList();

            foreach (var item in bucketDetails)
            {
                BucketViewModel bucketViewModel = new BucketViewModel()
                {
                    BucketDetailId = item.BucketDetailId,

                    Balls = item.Balls,
                    Price = item.Price,
                    DayTypeId = item.DayTypeId,
                    MemberTypeId = item.MemberTypeId,
                    DayTypeName = item.DayTypeId != null ? item.DayType.Name : "",
                    MemberTypeName = item.MemberTypeId != null ? item.MemberType.Name : ""
                };


                List<BucketTaxMappingViewModel> bucketTaxMappingViewModels = new List<BucketTaxMappingViewModel>();
                List<BucketTaxMapping> bucketTaxMappings = _unitOfWork.BucketTaxMappingRepository.GetMany(x => x.IsActive == true & x.BucketId == item.BucketDetailId).ToList();
                bucketViewModel.Taxs = new long[bucketTaxMappings.Count];
                int count = 0;
                foreach (var item1 in bucketTaxMappings)
                {

                    bucketViewModel.Taxs[count] = item1.TaxId;
                    BucketTaxMappingViewModel bucketTaxMappingViewModel = new BucketTaxMappingViewModel()
                    {
                        TaxId = item1.TaxId,
                        TaxName = item1.Tax.Name,
                        TaxPercentage = item1.Tax.Percentage
                    };
                    bucketTaxMappingViewModels.Add(bucketTaxMappingViewModel);
                    count++;
                };

                bucketViewModel.BucketTaxMappingViewModels = bucketTaxMappingViewModels;
                bucketViewModels.Add(bucketViewModel);
            }

            return bucketViewModels;
        }

        /// <summary>
        /// Save Bucket Type Detail
        /// </summary>
        /// <param name="bucketViewModel"></param>
        /// <returns></returns>
        public bool SaveBucketDetails(BucketViewModel bucketViewModel, long uniqueSessionId)
        {

            BucketDetail bucketDB = _unitOfWork.BucketDetailRepository.Get(x => x.Balls == bucketViewModel.Balls && x.MemberTypeId == bucketViewModel.MemberTypeId && x.DayTypeId == bucketViewModel.DayTypeId && x.IsActive == true);
            if (bucketDB != null)
                throw new Exception("Bucket Already Exist");
            BucketDetail bucketDetail = new BucketDetail()
            {
                Balls = bucketViewModel.Balls,

                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                Price = bucketViewModel.Price,

                DayTypeId = bucketViewModel.DayTypeId,
                MemberTypeId = bucketViewModel.MemberTypeId,

            };
            //List<EquipmentTaxMappingViewModel> equipmentTaxMappingViewModels = new List<EquipmentTaxMappingViewModel>();
            //  List<EquipmentTaxMapping> equipmentTaxMappings = _unitOfWork.EquipmentTaxMappingRepository.GetMany(x => x.IsActive == true & x.EquipmentId == item.EquipmentId).ToList();
            _unitOfWork.BucketDetailRepository.Insert(bucketDetail);
            _unitOfWork.Save();
            if (bucketViewModel.Taxs != null)
            {
                foreach (var items in bucketViewModel.Taxs)
                {
                    BucketTaxMapping bucketTaxMapping = new BucketTaxMapping()
                    {
                        TaxId = items,
                        BucketId = bucketDetail.BucketDetailId,
                        IsActive = true,
                        CreatedOn = System.DateTime.UtcNow
                    };
                    _unitOfWork.BucketTaxMappingRepository.Insert(bucketTaxMapping);
                    _unitOfWork.Save();
                }
            }
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Bucket",
                    ActionName = "Save",
                    PerformOn = bucketDetail.BucketDetailId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Bucket with Bucket id- " + bucketDetail.BucketDetailId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }


            return true;
        }

        /// <summary>
        ///  Delete Bucket Type Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteBucketDetails(long id, long uniqueSessionId)
        {

            BucketDetail bucketDetailDB = _unitOfWork.BucketDetailRepository.Get(x => x.BucketDetailId == id && x.IsActive == true);
            if (bucketDetailDB == null)
                throw new Exception("Bucket Not Exist");
            bucketDetailDB.IsActive = false;
            bucketDetailDB.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.BucketDetailRepository.Update(bucketDetailDB);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Bucket",
                    ActionName = "Delete",
                    PerformOn = bucketDetailDB.BucketDetailId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Bucket with Bucket id- " + bucketDetailDB.BucketDetailId.ToString() + ". Bucket had following details <br/>balls " + bucketDetailDB.Balls + "  <br/> price " + bucketDetailDB.Price + "  <br/> Member Type " + bucketDetailDB.MemberType.Name + "  <br/> Day " + bucketDetailDB.DayType.Name
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }

            return true;
        }

        /// <summary>
        /// Update Bucket Type Detail
        /// </summary>
        /// <param name="bucketViewModel"></param>
        /// <returns></returns>
        public bool UpdateBucketDetails(BucketViewModel bucketViewModel, long uniqueSessionId)
        {
            BucketDetail bucketDetail = new BucketDetail();
            BucketDetail bucketDetailDB = _unitOfWork.BucketDetailRepository.Get(x => x.BucketDetailId == bucketViewModel.BucketDetailId && x.IsActive == true);
            bucketDetail = bucketDetailDB;
            if (bucketDetailDB == null)
                throw new Exception("Bucket Type  Not Exist");

            BucketDetail bucketDetailDB1 = _unitOfWork.BucketDetailRepository.Get(x => x.Balls == bucketViewModel.Balls && x.MemberTypeId == bucketViewModel.MemberTypeId && x.DayTypeId == bucketViewModel.DayTypeId && x.IsActive == true);
            if (bucketDetailDB1 != null && bucketDetailDB1.Balls == bucketDetailDB.Balls && bucketDetailDB1.MemberTypeId != bucketDetailDB.MemberTypeId && bucketDetailDB1.DayTypeId != bucketDetailDB.DayTypeId)
                throw new Exception("Bucket Already Exist");

            bucketDetailDB.Price = bucketViewModel.Price;
            bucketDetailDB.Balls = bucketViewModel.Balls;
            bucketDetailDB.DayTypeId = bucketViewModel.DayTypeId;
            bucketDetailDB.MemberTypeId = bucketViewModel.MemberTypeId;
            _unitOfWork.BucketDetailRepository.Update(bucketDetailDB);
            _unitOfWork.Save();
            List<BucketTaxMappingViewModel> bucketTaxMappingViewModels = new List<BucketTaxMappingViewModel>();
            List<BucketTaxMapping> bucketTaxMappings = _unitOfWork.BucketTaxMappingRepository.GetMany(x => x.IsActive == true & x.BucketId == bucketViewModel.BucketDetailId).ToList();
            string tax = "";
            foreach (var item1 in bucketTaxMappings)
            {
                item1.IsActive = false;
                item1.UpdatedOn = System.DateTime.UtcNow;
                _unitOfWork.BucketTaxMappingRepository.Update(item1);
                _unitOfWork.Save();
                tax = tax != "" ? tax + "," + item1.Tax.Name : item1.Tax.Name;
            };

            if (bucketViewModel.Taxs != null)
            {
                foreach (var items in bucketViewModel.Taxs)
                {
                    BucketTaxMapping bucketTaxMapping = new BucketTaxMapping()
                    {
                        TaxId = items,
                        BucketId = bucketViewModel.BucketDetailId,
                        IsActive = true,
                        CreatedOn = System.DateTime.UtcNow
                    };
                    _unitOfWork.BucketTaxMappingRepository.Insert(bucketTaxMapping);
                    _unitOfWork.Save();
                }
            }

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Bucket",
                    ActionName = "Update",
                    PerformOn = bucketDetailDB.BucketDetailId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a Bucket with Bucket id- " + bucketDetailDB.BucketDetailId.ToString() + ". Bucket had following details <br/>balls " + bucketDetail.Balls + "  <br/> price " + bucketDetail.Price + "  <br/> Member Type " + bucketDetail.MemberType.Name + "  <br/> Day " + bucketDetail.DayType.Name + "<br/> Tax " + tax
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
