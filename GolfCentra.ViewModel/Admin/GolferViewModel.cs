using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Golfer's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class GolferViewModel
    {
        public long GolferId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string ClubMemberId { get; set; }
        public string Password { get; set; }
        public Nullable<long> PlatformTypeId { get; set; }
        public string Mobile { get; set; }
        public string PhoneCode { get; set; }
        public Nullable<System.DateTime> MemberSince { get; set; }
        public string Occpoution { get; set; }
        public Nullable<long> MaritalStatusId { get; set; }
        public Nullable<long> MemberTypeId { get; set; }
        public Nullable<long> GenderTypeId { get; set; }
        public string SpouseName { get; set; }
        public string EmailAddress { get; set; }
        public string PlatformName { get; set; }
        public string SpecialComments { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
        public List<GenderTypeViewModel> GenderTypeViewModels { get; set; }

        public List<MemberTypeViewModel> MemberTypeViewModels { get; set; }
        public List<MaritalStatusViewModel> MaritalStatusViewModels { get; set; }
        public string MaritalStatusValue { get; set; }
        public string MemberTypeName { get; set; }
        public string GenderTypeValue { get; set; }

        public bool IsBlocked { get; set; }
        public string Attachment { get; set; }
        public ImportGolferExcelReportModel ImportGolferExcelReportModel { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }

        public long UpdateId { get; set; }


        public long SalutationId { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }


        public string SelectBoxDisplay { get; set; }

    }
}
