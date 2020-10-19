using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebHenHo.Models;
using System.Security.Cryptography;
using System.Web.SessionState;
using System.Web.UI;
using BTLWebHenHo.EF.Model;

namespace BTLWebHenHo.Controllers
{
    public class LoginController : Controller
    {
          private WebHenHoDbContext _db = new WebHenHoDbContext();//khởi tạo 1 đối tượng của database
          // GET: Login
          
          public ActionResult Index()
        {

               if (Request.Cookies["usercredentials"] != null)
               {
                    return RedirectToAction("Index", "Profile");
               }


               return View();
        }
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Index(string username, string password,string remember_me)
          {
               if (ModelState.IsValid)
               {
                    var f_password = GetMD5(password);
                    var data = _db.UserInfoes.Where(s => s.username.Equals(username) && s.passw.Equals(password)).ToList();  //So sánh đối chiếu tài khoản                 
                    if (data.Count() > 0)
                    {
                         if (remember_me == "True")//remember-me is set
                         {
                             //Create a cookie
                             HttpCookie usercredentialsCookie = new HttpCookie("usercredentials");
                              //add cookies
                              usercredentialsCookie.Values["username"] = username;
                              usercredentialsCookie.Values["password"] = password;
                              usercredentialsCookie.Values["UserID"] = data.FirstOrDefault().UserID.ToString();
                              //set expired
                              usercredentialsCookie.Expires = DateTime.Now.AddDays(10);
                              Response.Cookies.Add(usercredentialsCookie);
                         }                         
                         Session["UserID"] = data.FirstOrDefault().UserID;//lấy IDUser vào Session 
                         return RedirectToAction("Index", "Profile");
                         //add session                        
                         //Session["idUser"] = data.FirstOrDefault().UserID;//lấy IDUser vào Session
                         //return RedirectToAction("Index", "Profile");
                    }
                    else
                    {
                         ViewBag.error = "Login failed";
                         TempData["error"]= "Login failed";
                         return RedirectToAction("Index");
                    }
               }
               return View();
          }

          //GET:Register
          public ActionResult Register()
          {
               return View();
          }
          //POST: Register
          [HttpPost]
          [ValidateAntiForgeryToken]
          
          public ActionResult Register(register _user)
          {
            if (ModelState.IsValid)
            {
                var check = _db.UserInfoes.FirstOrDefault(s => s.username == _user.username);
                if (check == null)
                {
                    if (_user.username == null)
                    {
                        ViewBag.error = "Please enter username";
                        return View();
                    }
                    else if (_user.passw == null)
                    {
                        ViewBag.error = "Please enter password";
                        return View();
                    }

                    else if (_user.fullname == null)
                    {


                        ViewBag.error = "Please enter fullname";
                        return View();
                    }
                    else
                    {
                        _db.Configuration.ValidateOnSaveEnabled = false;

                        if (((_user.month == "4" || _user.month == "6" || _user.month == "9" || _user.month == "11" || _user.month == "2") && (_user.day == "31")) || (_user.month == "2" && (_user.day == "30")) || (_user.month == "2" && _user.day == "29" && (Convert.ToInt32(_user.year) % 4 != 0)))
                        {
                            ViewBag.error = "Error birthday";
                            return View();
                        }
                        else
                        {
                            UserInfo new_user = new UserInfo();
                            new_user.username = _user.username;
                            //new_user.passw = GetMD5(_user.passw);
                            new_user.passw = _user.passw;
                            _db.UserInfoes.Add(new_user);
                            _db.SaveChanges();
                            int id = _db.UserInfoes.Where(x => x.username == _user.username).Select(x => x.UserID).FirstOrDefault();
                            Profile_User new_info_user = new Profile_User();
                            new_info_user.NickName = _user.fullname;
                            new_info_user.gender = _user.gender;
                            new_info_user.UserID = id;
                            string birthday = _user.day + "-" + _user.month + "-" + _user.year;
                            //DateTime date = Convert.ToDateTime(birthday);
                            //DateTime date = Convert.ToDateTime(birthday);
                            new_info_user.birthday = birthday;
                            //new_info_user.UserID = new_user.UserID;
                            _db.Profile_User.Add(new_info_user);
                            _db.SaveChanges();
                            return RedirectToAction("Index");
                        }




                    }
                    ////_user.Password = GetMD5(_user.Password);
                    //_db.Configuration.ValidateOnSaveEnabled = false;                            
                    ////_db.UserInfoes.Add(_user);
                    //_db.SaveChanges();
                    //return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.error = "User already exists";
                    return View();
                }
            }
            return View();



        }
        //Logout
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
          public ActionResult Logout()
          {
               Session.Clear();//delete all session but keep current session
               Session.RemoveAll();
               Session.Abandon();//delete current session
               Response.Cookies["usercredentials"].Expires=DateTime.Now.AddDays(-1);
               return RedirectToAction("Index");
          }
          //create a string MD5
          public static string GetMD5(string str)
          {
               MD5 md5 = new MD5CryptoServiceProvider();
               byte[] fromData = System.Text.Encoding.UTF8.GetBytes(str);
               byte[] targetData = md5.ComputeHash(fromData);
               string byte2String = null;

               for (int i = 0; i < targetData.Length; i++)
               {
                    byte2String += targetData[i].ToString("x2");

               }
               return byte2String;
          }
     }
}