using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core
{
    public class ClientContext : IClientContext
    {
        public Type MainViewType { get; set; }

        public string ServiceUrl
        {
            get { return ConfigurationManager.AppSettings["ServiceUrl"]; }
        }
    }
}
