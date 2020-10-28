using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace BTLWebHenHo.App_Start
{
     public static class WebApiConfig
     {
          public static void Register(HttpConfiguration config)
          {
               
               // Web API routes
               config.MapHttpAttributeRoutes();

               config.Routes.MapHttpRoute(
                   name: "DefaultApi",
                   routeTemplate: "api/{controller}/{id}",
                   defaults: new { id = RouteParameter.Optional }
               );
          }
     }
}