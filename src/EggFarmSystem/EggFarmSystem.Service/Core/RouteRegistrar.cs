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
                defaults:new {controller="HenHouses", action="GetPopulation"}
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


            //default routings
            routes.MapHttpRoute(name: "DefaultApi",
                           routeTemplate: "api/{controller}/{id}",
                           defaults: new { id = RouteParameter.Optional }
    );
        }
    }
}
