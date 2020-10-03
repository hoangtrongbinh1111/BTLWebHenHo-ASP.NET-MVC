using BTLWebHenHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebHenHo.EF.Infrastructure
{
     public class DbFactory : IDbFactory
     {
          private WebHenHoDbContext dbContext;

          public void Dispose()
          {
               if (dbContext != null)
                    dbContext.Dispose();
          }

          public WebHenHoDbContext Init()
          {
               return dbContext ?? (dbContext = new WebHenHoDbContext());
          }
     }
}