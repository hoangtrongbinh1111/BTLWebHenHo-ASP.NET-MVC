using BTLWebHenHo.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BTLWebHenHo
{
     public class MvcApplication : System.Web.HttpApplication
     {
          protected void Application_Start()
          {
               //TLS
               if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
               {
                    ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
               }
               //end TLS
               //adding it to execute WEb API
               GlobalConfiguration.Configure(WebApiConfig.Register);
               GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
               //end
               AreaRegistration.RegisterAllAreas();
               FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
               RouteConfig.RegisterRoutes(RouteTable.Routes);
               BundleConfig.RegisterBundles(BundleTable.Bundles);
          }
          protected void Application_BeginRequest()
          {
               Response.Cache.SetCacheability(HttpCacheability.NoCache);
               Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
               Response.Cache.SetNoStore();
          }
     }
}
