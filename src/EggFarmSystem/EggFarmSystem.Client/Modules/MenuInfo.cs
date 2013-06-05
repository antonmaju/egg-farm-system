using EggFarmSystem.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules
{
    public class MenuInfo
    {
        public Func<string> Title { get; set; }

        public Type CommandType { get; set; } 
    }
}
