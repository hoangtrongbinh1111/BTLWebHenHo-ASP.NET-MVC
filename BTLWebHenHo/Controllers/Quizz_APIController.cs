using BTLWebHenHo.EF.Services;
using BTLWebHenHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BTLWebHenHo.Controllers
{
    public class Quizz_APIController : ApiController
    {
          WebHenHoDbContext db = new WebHenHoDbContext();
          [HttpGet]
          public List<tbl_quizz> GetQuizz()
          {
               List<tbl_quizz> ltq = db.tbl_quizz.ToList();
               return ltq;
          }
          [HttpGet]
          public string GetGame(int id)
          {
               string img_link = "11.png";
               try
               {                    
                    Random rd = new Random();
                    int qty=db.Profile_User.ToList().Count;
                    int get_id_img = rd.Next(1,qty);
                    img_link = db.Profile_User.Where(x => x.UserID == get_id_img).Select(x=>x.avatar).FirstOrDefault();
                    //update count player
                    F_tbl_quizz ftq = new F_tbl_quizz();
                    var row_upd = ftq.GetSingleByCondition(x => x.id_quizz == id);
                    row_upd.qty_player++;
                    ftq.Update(row_upd);                                      
               }
               catch
               {

               }
               return img_link;

          }
          [HttpGet]
          public int del_chat_peo(int id_chat)
          {
               var content = db.tbl_chat.Where(x => x.id_chat == id_chat).Select(x => x.content).FirstOrDefault();
               var list_id = db.tbl_chat.Where(x => x.content == content).Select(x => x.id_chat).ToList();
               foreach(var item in list_id)
               {
                    var chat = new tbl_chat { id_chat=item};
                    db.tbl_chat.Attach(chat);
                    db.tbl_chat.Remove(chat);
                    db.SaveChanges();
               }
               var list_id_history = db.tbl_history_chat.Where(x => x.content == content).Select(x => x.id_chat_history).ToList();
               foreach (var item in list_id_history)
               {
                    var chat_history = new tbl_history_chat { id_chat_history = item };
                    db.tbl_history_chat.Attach(chat_history);
                    db.tbl_history_chat.Remove(chat_history);
                    db.SaveChanges();
               }
               return 1;
          }
          [HttpPost]
          public int create_Chat_Online_To_Friend(int id_main_user,int id_other_user)
          {
               var max_content = db.tbl_chat.OrderByDescending(x => x.content).Select(x => x.content).FirstOrDefault();
               int content_add = Convert.ToInt32(max_content);
               content_add += 1;
               tbl_chat tc = new tbl_chat();
               tc.id_main_user = id_main_user;
               tc.id_other_user = id_other_user;
               tc.content = content_add.ToString();
               db.tbl_chat.Add(tc);
               tc = new tbl_chat();
               tc.id_main_user = id_other_user;
               tc.id_other_user = id_main_user;
               tc.content = content_add.ToString();
               db.tbl_chat.Add(tc);
               db.SaveChanges();
               return content_add;
          }
         
     }
}
