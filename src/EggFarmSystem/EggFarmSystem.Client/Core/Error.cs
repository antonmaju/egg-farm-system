using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core
{
    public class Error
    {
        public Exception Exception { get; set; }

        public object Data { get; set; }
    }
}
