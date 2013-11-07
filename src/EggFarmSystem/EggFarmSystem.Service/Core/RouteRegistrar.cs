using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace EggFarmSystem.Service.Core
{
    public static class RouteRegistrar
    {
        public static void RegisterMappings(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(name: "GetPopulation", 
                routeTemplate:"api/henhouses/{id}/population",
                defaults:new {controller="HenHousePopulation", action="Get"}
                );

            //feed consumsion
            routes.MapHttpRoute(name:"DailyFeedAmount",
                routeTemplate:"api/henhouses/{houseId}/feed/{date}",
                defaults: new { controller = "Usage", action = "GetDailyFeedAmount" }
                );

            routes.MapHttpRoute(name: "HenDepreciationInitialValues",
                                routeTemplate: "api/hendepreciation/initialvalues/{date}",
                                defaults: new {controller = "HenDepreciation", action = "GetInitialValues"}
                );

            //reporting
            routes.MapHttpRoute(name: "EmployeeCostSummary",
                                routeTemplate: "api/report/employeecost/{start}/{end}",
                                defaults: new {controller = "EmployeeCostSummary", action = "Get"});
            routes.MapHttpRoute(name: "UsageSummary",
                                routeTemplate: "api/report/usagesummary/{start}/{end}",
                                defaults: new { controller = "UsageSummary", action = "Get" });

            routes.MapHttpRoute(name: "EggProductionReport",
                                routeTemplate: "api/report/eggproduction/{start}/{end}",
                                defaults: new { controller = "EggProductionReport", action = "Get" });



            //default routings
            routes.MapHttpRoute(name: "DefaultApi",
                           routeTemplate: "api/{controller}/{id}",
                           defaults: new { id = RouteParameter.Optional }
    );
        }
    }
}
