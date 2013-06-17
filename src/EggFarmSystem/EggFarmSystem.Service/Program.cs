using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace EggFarmSystem.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
                {
                    x.Service<EggFarmService>(s =>
                        {
                            s.ConstructUsing(factory => new EggFarmService());
                            s.WhenStarted(svc => svc.Start());
                            s.WhenStopped(svc => svc.Stop());
                        });
                    x.RunAsLocalSystem();
                    x.SetDescription("Egg farm service");
                    x.SetDisplayName("EggFarmService");
                    x.SetServiceName("EggFarmService");
                });
        }
    }
}
