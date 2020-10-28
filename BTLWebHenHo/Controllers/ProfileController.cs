using BTLWebHenHo.EF.Model;
using BTLWebHenHo.EF.Services;
using BTLWebHenHo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BTLWebHenHo.Controllers
{
     public class ProfileController : Controller
    {
          BTLWebHenHo.Models.WebHenHoDbContext db = new Models.WebHenHoDbContext();
        // GET: Profile
        public ActionResult Index()
        {
               if (Session["UserID"] != null || Request.Cookies["usercredentials"] != null)
               {
                    int id_user=get_ID_User();
                    var info_online = db.Profile_User.Where(x=>x.UserID != id_user).ToList();
                    ViewBag.info_online = info_online.Take(4).ToList();
                    var info = db.Profile_User.Where(x => x.UserID == id_user).FirstOrDefault();
                    ViewBag.info = info;
                    ViewBag.cal_percent = cal_percent(info);
                    //hottest
                    var Max_thumb = 0;
                    int? id_hot = 0;
                    string[] spl = { };
                    foreach (var item in info_online)
                    {
                         if (item.list_thumb != null)
                         {
                              spl = item.list_thumb.Split('/');
                         }
                         
                         if (spl.Count() > Max_thumb)
                         {
                              Max_thumb = spl.Count();
                              id_hot = item.UserID;
                         }                             
                    }
                    if (id_hot == 0)
                    {
                         id_hot = id_user;
                         var hottest = db.Profile_User.Where(x => x.UserID == id_hot).FirstOrDefault();
                         if (hottest.list_thumb != null)
                         {
                              spl = hottest.list_thumb.Split('/');
                              Max_thumb = spl.Count();
                         }
                         ViewBag.hottest_user = hottest;
                         ViewBag.num_hot_thumb = Max_thumb;
                    }
                    else
                    {
                         var hottest = db.Profile_User.Where(x => x.UserID == id_hot).FirstOrDefault();
                         ViewBag.hottest_user = hottest;
                         ViewBag.num_hot_thumb = Max_thumb;
                    }     
                    
                    //new image
                    var new_img = db.new_imgage.OrderByDescending(x => x.id_new_img).ToList();
                    ViewBag.new_img = new_img.Take(4).ToList();
                    //get list chat
                    var list_user_chat = db.tbl_chat.Where(x => x.id_main_user == id_user).ToList();
                    List<Chat_specific> ls = new List<Chat_specific>();
                   
                    foreach(var item in list_user_chat)
                    {
                         var getinf = db.Profile_User.Where(x=>x.UserID==item.id_other_user).FirstOrDefault();
                         Chat_specific cs = new Chat_specific();
                         cs.tc = item;
                         cs.Nickname = getinf.NickName;
                         cs.address = getinf.address_user;
                         cs.image_link = getinf.avatar;
                         ls.Add(cs);
                    }
                    ViewBag.list_user_chat = ls;
                    return View();
               }
               else
               {
                    return RedirectToAction("Index", "Login");
               }               
          }
          public int cal_percent(Profile_User pu)
          {
               int count = 0;
               int qty = 0;
               if(pu.NickName==null ||pu.NickName=="Không có") { count++; }
               qty++;
               if (pu.email == null || pu.email == "Không có") { count++; }
               qty++;
               if (pu.phone == null || pu.phone == "Không có") { count++; }
               qty++;
               if (pu.address_user == null || pu.address_user == "Không có") { count++; }
               qty++;
               if (pu.coins == null || pu.coins == "Không có") { count++; }
               qty++;
               if (pu.avatar == null || pu.avatar == "Không có") { count++; }
               qty++;
               if (pu.birthday == null || pu.birthday == "Không có") { count++; }
               qty++;
               if (pu.height == null ) { count++; }
               qty++;
               if (pu.body == null || pu.body == "Không có") { count++; }
               qty++;
               if (pu.blood == null || pu.blood == "Không có") { count++; }
               qty++;
               if (pu.national_user == null || pu.national_user == "Không có") { count++; }
               qty++;
               if (pu.language_user == null || pu.language_user == "Không có") { count++; }
               qty++;
               if (pu.education == null || pu.education == "Không có") { count++; }
               qty++;
               if (pu.job == null || pu.job == "Không có") { count++; }
               qty++;
               if (pu.income == null || pu.income == "Không có") { count++; }
               qty++;
               if (pu.martial_status == null || pu.martial_status == "Không có") { count++; }
               qty++;
               if (pu.baby_status == null || pu.baby_status == "Không có") { count++; }
               qty++;
               if (pu.want_baby_status == null || pu.want_baby_status == "Không có") { count++; }
               qty++;
               if (pu.live_status == null || pu.live_status == "Không có") { count++; }
               qty++;
               if (pu.hobbies == null || pu.hobbies == "Không có") { count++; }
               qty++;
               if (pu.character_user == null || pu.character_user == "Không có") { count++; }
               qty++;
               if (pu.public_relationship == null || pu.public_relationship == "Không có") { count++; }
               qty++;
               if (pu.want_meet == null || pu.want_meet == "Không có") { count++; }
               qty++;
               if (pu.want_marry == null || pu.want_marry == "Không có") { count++; }
               qty++;
               if (pu.ready_do_homework == null || pu.ready_do_homework == "Không có") { count++; }
               qty++;
               if (pu.freeday == null || pu.freeday == "Không có") { count++; }
               qty++;
               if (pu.wine == null || pu.wine == "Không có") { count++; }
               qty++;
               if (pu.smoke == null || pu.smoke == "Không có") { count++; }
               qty++;
               if (pu.pay_first_meet == null || pu.pay_first_meet == "Không có") { count++; }
               qty++;
               if (pu.family == null || pu.family == "Không có") { count++; }
               qty++;
               if (pu.gender == null || pu.gender == "Không có") { count++; }
               qty++;
               int percentComplete = (int)Math.Round((double)(100 * (qty - count)) / qty);
               return percentComplete;
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
          public ActionResult Chat_ToFriend(int? id_other_user,int ?stt_id_chat)
          {
               if (id_other_user==null) return RedirectToAction("Index");
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
               //var id_chat = Request.Url.Segments[3];
               //var stt = Request.Url.Segments[4];
               //get list user id and list chat to show in group chat 
               F_History_Chat fhc = new F_History_Chat();
               var row_upd = fhc.GetSingleByCondition(x => x.content == stt_id_chat.ToString());
               List<History_Chat> list_chat = new List<History_Chat>();
               var name_other_user = db.Profile_User.Where(x => x.UserID == id_other_user).Select(x => x.NickName).FirstOrDefault();
               if (row_upd == null) {
                    ViewBag.list_chat = list_chat;
                    ViewBag.id_chat = id_other_user;
                    ViewBag.stt_id_chat = stt_id_chat;
                    ViewBag.NickName_other = name_other_user;
                    return View();
               }              
               var list_id = row_upd.list_id_User;
               var list_msg = row_upd.list_msg;
               //split string to per msg
               string[] msg_split = list_msg.ToString().Split(new string[] { "/*space*/" }, StringSplitOptions.None);
               string[] id_split = list_id.ToString().Split('/');
               
               for (int i = 0; i < msg_split.Count() - 1; i++)
               {
                    History_Chat hc = new History_Chat();
                    hc.idUser = Convert.ToInt32(id_split[i]);
                    hc.NickName = getNickName(Convert.ToInt32(id_split[i]));
                    hc.msg = msg_split[i];
                    list_chat.Add(hc);
                    //list_chat.Add(getNickName(Convert.ToInt32(id_split[i]))+": "+ msg_split[i]);
               }
               ViewBag.list_chat = list_chat;
               ViewBag.id_chat = id_other_user;
               ViewBag.stt_id_chat = stt_id_chat;
               ViewBag.NickName_other = name_other_user;
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
          [HttpPost]
          public JsonResult History_Chat_Private(string from_id_user,string stt, string msg)
          {
               F_History_Chat fhc = new F_History_Chat();
               var getHis = fhc.GetSingleByCondition(x=>x.content==stt);
               if (getHis == null)
               {
                    tbl_history_chat thc = new tbl_history_chat();
                    thc.content = stt;
                    fhc.Add(thc);
                    fhc.Save();
               }
               var row_upd = fhc.GetSingleByCondition(x => x.content == stt);
               row_upd.list_id_User += from_id_user + "/";
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
          [NonAction]
          public int get_ID_User()
          {
               if (Request.Cookies["usercredentials"] != null)
               {
                    HttpCookie reqCookie = Request.Cookies["usercredentials"];
                    return Convert.ToInt32(reqCookie["UserID"].ToString());
               }
               else//if not have cookies
               {
                    return Convert.ToInt32(Session["UserID"].ToString());
               }
          }
          private string RemoveEmptyLines(string lines)
          {
               if (lines == null) return "Không có";
               return Regex.Replace(lines, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).Trim();
          }
          [HttpPost]
          public JsonResult Update_Info(BTLWebHenHo.Models.Profile_User obj)
          {              
               F_Profile_User fpu = new F_Profile_User();
               var row_upd = fpu.GetSingleByCondition(x => x.UserID == obj.UserID);
               row_upd.NickName = RemoveEmptyLines(obj.NickName);
               row_upd.gender= RemoveEmptyLines(obj.gender);
               row_upd.birthday = RemoveEmptyLines(obj.birthday);
               row_upd.height = obj.height;
               row_upd.body = RemoveEmptyLines(obj.body);
               row_upd.blood = RemoveEmptyLines( obj.blood);
               row_upd.national_user = RemoveEmptyLines(obj.national_user);
               row_upd.address_user = RemoveEmptyLines(obj.address_user);
               row_upd.family = RemoveEmptyLines(obj.family);
               row_upd.language_user = RemoveEmptyLines(obj.language_user);
               row_upd.education = RemoveEmptyLines(obj.education);
               row_upd.job = RemoveEmptyLines(obj.job);
               row_upd.income = RemoveEmptyLines(obj.income);
               row_upd.martial_status = RemoveEmptyLines(obj.martial_status);
               row_upd.baby_status = RemoveEmptyLines(obj.baby_status);
               row_upd.want_baby_status = RemoveEmptyLines(obj.want_baby_status);
               row_upd.live_status = RemoveEmptyLines(obj.live_status);
               row_upd.hobbies = RemoveEmptyLines(obj.hobbies);
               row_upd.character_user = RemoveEmptyLines(obj.character_user);
               row_upd.public_relationship = RemoveEmptyLines(obj.public_relationship);
               row_upd.want_meet = RemoveEmptyLines(obj.want_meet);
               row_upd.want_marry = RemoveEmptyLines(obj.want_marry);
               row_upd.ready_do_homework = RemoveEmptyLines(obj.ready_do_homework);
               row_upd.freeday = RemoveEmptyLines(obj.freeday);
               row_upd.wine = RemoveEmptyLines(obj.wine);
               row_upd.smoke = RemoveEmptyLines(obj.smoke);
               row_upd.pay_first_meet = RemoveEmptyLines(obj.pay_first_meet);
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
                         int id_user=get_ID_User();                        
                         var upd = db.Profile_User.Where(x => x.UserID == id_user).FirstOrDefault();
                         upd.avatar = fileName;
                         db.SaveChanges();
                         
                    }
                    
               }                           
               return RedirectToAction("Index","Profile");
               //return View();
          }
          [HttpPost]
          public ActionResult Upload_List_Img(HttpPostedFileBase[] file)
          {
               foreach(var imageFile in file)
               {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Public/Asset/img/List_image_upload"), fileName);
                    imageFile.SaveAs(path);

                    int id_user=get_ID_User();                   
                    var upd = db.Profile_User.Where(x => x.UserID == id_user).FirstOrDefault();
                    //country is list image
                    upd.list_thumb += fileName+"/";
                    //new image
                    new_imgage ni = new new_imgage();
                    ni.new_img = fileName;
                    db.new_imgage.Add(ni);

                    db.SaveChanges();               
               }
               
               return RedirectToAction("Index", "Profile");
          }
          [HttpPost]
          public ActionResult Delete_image_user(string name_img)
          {
               int id_user = get_ID_User();
               var upd = db.Profile_User.Where(x => x.UserID == id_user).FirstOrDefault();
               upd.list_thumb = upd.list_thumb.Replace(name_img+"/","");
               db.SaveChanges();
               return Json(new { status=true});
          }
          [HttpPost]
          public ActionResult Delete_all_img()
          {
               int id_user = get_ID_User();
               var upd = db.Profile_User.Where(x => x.UserID == id_user).FirstOrDefault();
               upd.list_thumb = null;
               db.SaveChanges();
               return Json(new { status = true });
          }
          [HttpPost]
          public ActionResult Update_ConID(int UserID,string con_ID)
          {
               F_UserInfo fu = new F_UserInfo();
               var row_upd = fu.GetSingleByCondition(x => x.UserID == UserID);
               row_upd.con_ID = con_ID;
               fu.Update(row_upd);
               return Json(new { status=true});
          }
          [HttpPost]
          public ActionResult Get_ConID(int chat_id)
          {
               F_UserInfo fu = new F_UserInfo();
               var row_upd = fu.GetSingleByCondition(x => x.UserID == chat_id);              
               return Json(row_upd.con_ID);
          }
          public ActionResult getInfoFriend_by_find_basic(int age_start,int age_end,string country_val,string country)
          {
               List<Profile_User> ui = new List<Profile_User>();
               var pu = db.Profile_User.ToList();
               foreach(var info in pu)
               {                    
                   var age = 0;
//<<<<<<< HEAD
//                    string[] spl = info.birthday.Split('-');
//                    if (spl.Count() > 1)
//                    {                        
//                         age = DateTime.Now.Year - Convert.ToInt32(spl[2]);         
//                    }
//                    else
//                    {
//                         string[] spl1 = info.birthday.Split('/');                        
//                         age = DateTime.Now.Year - Convert.ToInt32(spl[2]);
//                    }
//=======
                    string[] spl = info.birthday.Split('-');                                       
                    age = DateTime.Now.Year - Convert.ToInt32(spl[0]);                          
//>>>>>>> 329a9eec7d1765ed4bf95ebeb05543820371205b
                    if (country_val == "none")
                    {
                         if(age_start==age_end && age_end == 0)
                         {
                              ui.Add(info);
                         }
                         if (age_end == 0)
                         {
                              if (age >= age_start)
                              {
                                   ui.Add(info);
                              }
                         }
                         else if (age>=age_start && age <= age_end)
                         {
                              ui.Add(info);
                         }
                    }
                    else
                    {
                         if (age_start == age_end && age_end == 0 && info.national_user == country)
                         {
                              ui.Add(info);
                         }
                         if (age_end == 0)
                         {
                              if (age >= age_start  && info.national_user == country)
                              {
                                   ui.Add(info);
                              }
                         }
                         else if (age >= age_start && age <= age_end &&info.national_user==country)
                         {
                              ui.Add(info);
                         }
                    }

               }
               return View(ui);
          }
          public ActionResult hottest_info(int id)
          {
               var info_online = db.Profile_User.Where(x => x.UserID == id-2).FirstOrDefault();
               if (info_online == null)
               {
                    info_online = db.Profile_User.Where(x => x.UserID == id + 1).FirstOrDefault();
               }
               var Max_thumb = 0;
               
               string[] spl = { };
               if (info_online.list_thumb != null)
               {
                    spl = info_online.list_thumb.Split('/');
                    Max_thumb = spl.Count();
               }
               ViewBag.hottest_user = info_online;
               ViewBag.num_hot_thumb = Max_thumb;
               return View(info_online);
          }
     }
}