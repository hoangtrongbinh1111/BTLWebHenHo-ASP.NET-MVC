using BTLWebHenHo.EF.Model;
using BTLWebHenHo.EF.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTLWebHenHo.Controllers
{
    public class ProfileController : Controller
    {
          BTLWebHenHo.Models.WebHenHoDbContext db = new Models.WebHenHoDbContext();
          int? id = 0;
        // GET: Profile
        public ActionResult Index()
        {
               if (Session["UserID"] != null || Request.Cookies["usercredentials"] != null)
               {
                    var id_user=0;
                    if (Request.Cookies["usercredentials"] != null)
                    {
                         HttpCookie reqCookie = Request.Cookies["usercredentials"];                    
                         id_user = Convert.ToInt32(reqCookie["UserID"].ToString());
                         id = id_user;
                    }
                    else//if not have cookies
                    {                        
                         id_user = Convert.ToInt32(Session["UserID"].ToString());
                         id = id_user;
                    }
                    var info = db.Profile_User.Where(x => x.UserID == id_user).FirstOrDefault();
                    ViewBag.info = info;
                    return View();
               }
               else
               {
                    return RedirectToAction("Index", "Login");
               }               
          }
          public ActionResult Chat_Group()
          {
               
               //if have cookies
               if (Request.Cookies["usercredentials"] != null)
               {
                    HttpCookie reqCookie = Request.Cookies["usercredentials"];
                   // ViewBag.Name = reqCookie["UserID"].ToString();
                                       
                    ViewBag.Name = getNickName(Convert.ToInt32(reqCookie["UserID"].ToString()));
                    ViewBag.UserID = Convert.ToInt32(reqCookie["UserID"].ToString());
               }
               else//if not have cookies
               {                    
                    ViewBag.Name = getNickName(Convert.ToInt32(Session["UserID"].ToString()));
                    ViewBag.UserID = Convert.ToInt32(Session["UserID"].ToString());
               }
               //get list user id and list chat to show in group chat 
               F_History_Chat fhc = new F_History_Chat();
               var row_upd = fhc.GetSingleByCondition(x => x.id_chat_history == 1);
               var list_id = row_upd.list_id_User;
               var list_msg = row_upd.list_msg;    
               //split string to per msg
               string[] msg_split = list_msg.ToString().Split(new string[] { "/*space*/" },StringSplitOptions.None);
               string[] id_split = list_id.ToString().Split('/');
               List<History_Chat> list_chat = new List<History_Chat>();
               for (int i = 0; i < msg_split.Count()-1; i++)
               {
                    History_Chat hc = new History_Chat();
                    hc.idUser = Convert.ToInt32(id_split[i]);
                    hc.NickName = getNickName(Convert.ToInt32(id_split[i]));
                    hc.msg= msg_split[i];                    
                    list_chat.Add(hc);
                    //list_chat.Add(getNickName(Convert.ToInt32(id_split[i]))+": "+ msg_split[i]);
               }
               ViewBag.list_chat = list_chat;
               return View();
          }
          [HttpPost]
          public JsonResult History_Chat(string UserID,string msg) {
               F_History_Chat fhc = new F_History_Chat();
               //get history_chat with id=1, because we save it only 1 row with id=1
               var row_upd = fhc.GetSingleByCondition(x=>x.id_chat_history==1);
               row_upd.list_id_User += UserID + "/";
               row_upd.list_msg += msg + "/*space*/";
               fhc.Update(row_upd);
               return Json("Update to database success");
          }

          [NonAction]
          public string getNickName(int userID)
          {
               F_Profile_User fpu = new F_Profile_User();
               Profile_User pu = new Profile_User();
               pu.NickName = fpu.GetSingleByCondition(x => x.UserID == userID).NickName;
               return pu.NickName;
          }

          [HttpPost]
          public JsonResult Update_Info(BTLWebHenHo.Models.Profile_User obj)
          {
               
               F_Profile_User fpu = new F_Profile_User();
               var row_upd = fpu.GetSingleByCondition(x => x.UserID == obj.UserID);
               row_upd.NickName = obj.NickName;
               row_upd.birthday = obj.birthday;
               row_upd.height = obj.height;
               row_upd.body = obj.body;
               row_upd.blood = obj.blood;
               row_upd.national_user = obj.national_user;
               row_upd.address_user = obj.address_user;
               row_upd.family = obj.family;
               row_upd.language_user = obj.language_user;
               row_upd.education = obj.education;
               row_upd.job = obj.job;
               row_upd.income = obj.income;
               row_upd.martial_status = obj.martial_status;
               row_upd.baby_status = obj.baby_status;
               row_upd.want_baby_status = obj.want_baby_status;
               row_upd.live_status = obj.live_status;
               row_upd.hobbies = obj.hobbies;
               row_upd.character_user = obj.character_user;
               row_upd.public_relationship = obj.public_relationship;
               row_upd.want_meet = obj.want_meet;
               row_upd.want_marry = obj.want_marry;
               row_upd.ready_do_homework = obj.ready_do_homework;
               row_upd.freeday = obj.freeday;
               row_upd.wine = obj.wine;
               row_upd.smoke = obj.smoke;
               row_upd.pay_first_meet = obj.pay_first_meet;
               fpu.Update(row_upd);

               //return Json(obj.NickName+"  "+obj.birthday+"  "+obj.body + "  " + obj.address_user + "  " + obj.height + "  " + obj.blood + "  " + obj.national_user + "  " + obj.language_user + "  " + obj.education
               //     + "  " + obj.job + "  " + obj.income + "  " + obj.martial_status + "  " + obj.baby_status + "  " + obj.want_baby_status + "  " + obj.live_status + "  " + obj.hobbies + "  " + obj.character_user
               //     + "  " + obj.public_relationship + "  " + obj.want_meet + "  " + obj.want_marry + "  " + obj.ready_do_homework + "  " + obj.freeday
               //     +"  " + obj.wine+"  " + obj.smoke+"  " + obj.pay_first_meet+"  ");
               return Json(new { status=true});
          }
          [HttpGet]
          public ActionResult Upload_Img()
          {
               return View();
          }
          [HttpPost]
          public ActionResult Upload_Img(HttpPostedFileBase imageFile)
          {
               if (imageFile.ContentLength > 0)
               {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Public/Asset/img/Avatar"), fileName);
                    imageFile.SaveAs(path);                    
                    if (path != null)
                    {
                         var upd = db.Profile_User.Where(x => x.UserID == 1).FirstOrDefault();
                         upd.avatar = fileName;
                         db.SaveChanges();
                         
                    }
                    
               }
               
               //string fileName = Path.GetFileNameWithoutExtension(imageFile.ImageFile.FileName);
               //string extension = Path.GetExtension(imageFile.ImageFile.FileName);
               //fileName=fileName+ DateTime.Now.ToString("yymmssfff") + extension;
               //imageFile.ImagePath = "~/Public/Asset/img/"+fileName;
               //fileName = Path.Combine(Server.MapPath("~/Public/Asset/img/"),fileName);
               //imageFile.ImageFile.SaveAs(fileName);

               //F_Profile_User fpu = new F_Profile_User();
               //var row_upd=fpu.GetSingleByCondition(x => x.UserID == Convert.ToInt32(Session["UserID"].ToString()));

               //return RedirectToAction("Index","Profile");
               return View();
          }
     }
}