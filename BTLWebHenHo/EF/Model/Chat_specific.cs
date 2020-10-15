using BTLWebHenHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebHenHo.EF.Model
{
     public class Chat_specific
     {
          public tbl_chat tc { get; set; }
          public string Nickname { get; set; }

          public string address { get; set; }
          public string image_link { get; set; }
     }
}