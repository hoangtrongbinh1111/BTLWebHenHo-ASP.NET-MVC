using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.IO;
using System.Web.Mvc;
namespace BTLWebHenHo.Hubs
{
     
     public interface IUserIdProvider
     {
          string GetUserId(IRequest request);
     }

     [HubName("chat")]
     public class ChatGroupMTAhub:Hub
     {
          BTLWebHenHo.Models.WebHenHoDbContext db = new Models.WebHenHoDbContext();
          public void Connect(string name)
          {
               //trả về cho client phương thức  connect
               Clients.Caller.connect(name);
          }
          public void Message(string name, string message)
          {
               //trả về cho client pt message vs 2 tham số
               //Clients.All.message(name, message);
               Clients.All.message(name, message);
          }
          public void Message(string name, string message,string conID,string stt,string UserID)
          {
               var get = db.UserInfoes.Where(x => x.con_ID == conID).FirstOrDefault();
               var get_stt = db.tbl_chat.Where(x => x.id_other_user == get.UserID).Where(x=>x.id_main_user.ToString()==UserID).FirstOrDefault();
               if(stt==get_stt.content)
                    Clients.Client(conID).message(name,message,stt);
          }
     }
}