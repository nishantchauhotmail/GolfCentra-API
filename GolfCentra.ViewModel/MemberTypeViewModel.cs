using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Member Type's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class MemberTypeViewModel
    {
        public MemberTypeViewModel()
        {

            MemberTypeId = 0;
            Name = string.Empty;
            ValueToShow = string.Empty;
            PlayerCount = 0;
            RangeFee = 0;
            GreenFee = 0;
            Caddie9HolePrice = 0;
            Cart9HolePrice = 0;
            Cart18HolePrice = 0;
            Cart27HolePrice = 0;
            Caddie18HolePrice = 0;
            Caddie27HolePrice = 0;
            CurrencyName = string.Empty;
        }
        public long MemberTypeId { get; set; }
        public string Name { get; set; }
        public string ValueToShow { get; set; }
        public int PlayerCount { get; set; }
        public decimal RangeFee { get; set; }
        public decimal GreenFee { get; set; }
        public decimal Caddie9HolePrice { get; set; }
        public decimal Caddie18HolePrice { get; set; }
        public decimal Cart9HolePrice { get; set; }
        public decimal Cart18HolePrice { get; set; }
        public decimal Caddie27HolePrice { get; set; }
        public decimal Cart27HolePrice { get; set; }
        public string CurrencyName { get; set; }
    }
}
