using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebHenHo.EF.Model
{
     public class UserProfile
     {
          public string NickName { get; set; }
          public string birthday { get; set; }
          public string address { get; set; }
          public int count_img { get; set; }
     }
}