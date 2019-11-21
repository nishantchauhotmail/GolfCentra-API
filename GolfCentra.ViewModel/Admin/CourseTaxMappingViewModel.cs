﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For CourseTaxMappig's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class CourseTaxMappingViewModel
    {
        public long TaxId { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
    }
}
