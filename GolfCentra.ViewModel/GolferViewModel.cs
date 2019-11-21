using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Golfer's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class GolferViewModel
    {
        public GolferViewModel()
        {
            this.OTP = string.Empty;
            this.EmailAddress = string.Empty;
            this.GolferId = 0;
            this.IpAddress = string.Empty;
            this.MacAddress = string.Empty;
            this.MemberShipId = string.Empty;
            this.MobileNumber = string.Empty;
            this.Name = string.Empty;
            this.Password = string.Empty;
            this.PlatformTypeId = 0;
            this.UniqueSessionId = string.Empty;
            this.ApiPassword = string.Empty;
            this.ApiUserName = string.Empty;
            this.IsMember = false;
            DeviceTokenId = string.Empty;
            SalutationId = 0;
            Address = string.Empty;
            DOB = string.Empty;
            Occuption = string.Empty;
            SalutationText = string.Empty;
            LastName = string.Empty;
            MemberTypeId = 0;
            IsFirstLogIn = false;
        }

        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string MemberShipId { get; set; }
        public string OTP { get; set; }
        public int PlatformTypeId { get; set; }
        public long GolferId { get; set; }
        public string UniqueSessionId { get; set; }
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public string ApiUserName { get; set; }

        public string ApiPassword { get; set; }
        public bool IsMember { get; set; }
        public long SalutationId { get; set; }
        public string DeviceTokenId { get; set; }
        public string Address { get; set; }
        public string DOB { get; set; }
        public long GenderId { get; set; }
        public string Occuption { get; set; }
        public string SalutationText { get; set; }
        public string LastName { get; set; }
        public long MemberTypeId { get; set; }
        public bool IsFirstLogIn { get; set; }
    }
}
