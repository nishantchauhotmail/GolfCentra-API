﻿using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
  public  interface ITimeFormatService
    {
        List<TimeFormatViewModel> GetAllTimeFormat();
    }
}