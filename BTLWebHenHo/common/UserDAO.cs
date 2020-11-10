using BTLWebHenHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebHenHo.common
{
     public class UserDAO
     {
          WebHenHoDbContext db = new WebHenHoDbContext();
          public List<string> GetListPermission(int? roleID)
          {
               return (from a in db.tbl_user_permission
                       join b in db.tbl_permission on a.id_permission equals b.id_permission
                       where a.id_User_Type == roleID
                       select b.name).ToList();
          }
     }
}