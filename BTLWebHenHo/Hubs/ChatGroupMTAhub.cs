using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
namespace BTLWebHenHo.Hubs
{
     [HubName("chat")]
     public class ChatGroupMTAhub:Hub
     {
          public void Connect(string name)
          {
               //trả về cho client phương thức  connect
               Clients.Caller.connect(name);
          }
          public void Message(string name, string message)
          {
               //trả về cho client pt message vs 2 tham số
               Clients.All.message(name, message);
          }
     }
}