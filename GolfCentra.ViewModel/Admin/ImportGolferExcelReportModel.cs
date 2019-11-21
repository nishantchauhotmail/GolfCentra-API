using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    public class ImportGolferExcelReportModel
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string MembershipId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
