using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebHenHo.Models;
using System.Security.Cryptography;
using System.Web.SessionState;
using System.Web.UI;

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
          
          public ActionResult Register(UserInfo _user)
          {
               if (ModelState.IsValid)
               {
                    var check = _db.UserInfoes.FirstOrDefault(s => s.username == _user.username);
                    if (check == null)
                    {
                         if (_user.passw == "")
                         {
                              ViewBag.error = "Please enter password";
                              return View();
                         }
                         else
                         {
                              //_user.Password = GetMD5(_user.Password);
                              _db.Configuration.ValidateOnSaveEnabled = false;                            
                              _db.UserInfoes.Add(_user);
                              _db.SaveChanges();
                              return RedirectToAction("Index");
                         }
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