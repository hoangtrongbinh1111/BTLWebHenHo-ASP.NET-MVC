using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebHenHo.common
{
     [Serializable]
     public class userlogin
     {
          public int? UserID { set; get; }
          public string UserName { set; get; }
          public string Name { get; set; }
          public string Email { get; set; }
          public string Password { set; get; }
          public List<string> ListPermission { get; set; }
          public string Role { get; set; }
          public int? PermissionID { get; set; }
          public int? User_Type { set; get; }
     }
}