using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Core.Helper
{
    public static class RamdomNumber
    {
        /// <summary>
        /// Create Random number of 6 digit
        /// </summary>
        /// <returns></returns>
        public static string RandomNumber()
        {
            Random generator = new Random();
            String r = generator.Next(0, 999999).ToString("D6");
            return r;
        }

    }
}
