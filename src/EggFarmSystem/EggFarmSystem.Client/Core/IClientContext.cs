﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core
{
    public interface IClientContext
    {
        Type MainViewType { get; set; }

        string ServiceUrl { get; }

        int PageSize { get; set; }

        string Culture { get; set; }
    }
}
