using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Modules.Usage.Commands;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.Usage
{
    public class ConsumableUsageModule : IModule
    {
        public ConsumableUsageModule()
        {
            Registry = new UsageRegistry();
            AvailableMenus = new List<MenuInfo>()
                {
                    new MenuInfo
                        {
                            CommandType = typeof(ShowUsageCommand),
                            Title = () => LanguageData.Usage_Title
                        }
                };
        }

        public string Name
        {
            get { return "Usage"; }
        }

        public IList<MenuInfo> AvailableMenus { get; private set; }

        public Autofac.Module Registry { get; private set; }

        public void RegisterMessageSubscriber(Core.IMessageBroker broker)
        {
            throw new NotImplementedException();
        }
    }
}
