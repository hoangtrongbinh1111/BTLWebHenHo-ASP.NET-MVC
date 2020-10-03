using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(BTLWebHenHo.StartUp))]
namespace BTLWebHenHo
{
     public partial class StartUp
     {
          public void Configuration(IAppBuilder app)
          {
               //ConfigureAuth(app);
               //thêm dong code này nhé các bạn
               app.MapSignalR();
          }
     }
}