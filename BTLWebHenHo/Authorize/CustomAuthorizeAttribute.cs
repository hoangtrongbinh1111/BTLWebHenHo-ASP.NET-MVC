using BTLWebHenHo.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BTLWebHenHo.Authorize
{
     //[AttributeUsage(AttributeTargets.Method)]
     public class CustomAuthorizeAttribute:AuthorizeAttribute
     {
          public string Permission { get; set; }

          protected override bool AuthorizeCore(HttpContextBase httpContext)
          {
               List<string> Permissions = Permission.Split(',').ToList();
               var session = (userlogin)HttpContext.Current.Session[CommonConstant.USER_SESSION];
               if (session != null)
               {

                   List<string> privilegeLevels = session.ListPermission;
                    //                   

                    foreach (var item in Permissions)
                    {
                         if (privilegeLevels.Contains(item))
                         {
                              //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["universityid"]))
                              //{
                              //     var uniID = session.UniversityID;
                              //     var uniID_req = Convert.ToInt32(HttpContext.Current.Request.Params["universityid"]);
                              //     if (uniID == uniID_req || uniID == 0) return true;
                              //     else return false;
                              //}
                              return true;
                         }
                    }

               }
               return false;
          }
          protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
          {
               var values = new RouteValueDictionary(new
               {
                    action = "Error401",
                    controller = "Home",
               });
               filterContext.Result = new RedirectToRouteResult(values);
          }
     }
}