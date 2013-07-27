using System.Globalization;
using System.Threading;
using Autofac;
using Autofac.Integration.WebApi;
using EggFarmSystem.Service.Core;
using EggFarmSystem.Service.Core.Installers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace EggFarmSystem.Service
{
    public class EggFarmService
    {
        private readonly HttpSelfHostServer server;

        public EggFarmService()
        {
            var config = new HttpSelfHostConfiguration("http://localhost:3000");
            config.Routes.MapHttpRoute(name: "DefaultApi",
                                       routeTemplate: "api/{controller}/{id}",
                                       defaults: new {id = RouteParameter.Optional}
                );
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule<ServiceModule>();
            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
            server = new HttpSelfHostServer(config);
            MappingInitializer.Initialize();
        }

        public void Start()
        {
            server.OpenAsync();
        }

        public void Stop()
        {
            server.CloseAsync();
            server.Dispose(); 
        }
    }
}
