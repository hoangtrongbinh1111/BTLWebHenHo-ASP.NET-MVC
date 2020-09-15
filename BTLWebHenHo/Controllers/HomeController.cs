using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebHenHo.Models;
using System.Security.Cryptography;
namespace BTLWebHenHo.Controllers
{
     public class HomeController : Controller
     {
          private WebHenHoDbContext _db = new WebHenHoDbContext();//khởi tạo 1 đối tượng của database
          public ActionResult Index()
          {               
               return View();
          }
          public ActionResult MTAhub_Story()
          {
               return View();
          }
          public ActionResult Quizz()
          {
               return View();
          }
          public ActionResult Blog()
          {
               return View();
          }
          
          public ActionResult Login()
          {
               return View();
          }



          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Login(string username, string password)
          {
               if (ModelState.IsValid)
               {
                    var f_password = GetMD5(password);
                    var data = _db.UserInfoes.Where(s=> s.username.Equals(username) && s.passw.Equals(password)).ToList();  //So sánh đối chiếu tài khoản                 
                    if (data.Count() > 0)
                    {
                         //add session                        
                         Session["idUser"] = data.FirstOrDefault().UserID;//lấy IDUser vào Session
                         return RedirectToAction("Index","Profile");
                    }
                    else
                    {
                         ViewBag.error = "Login failed";
                         return RedirectToAction("Login");
                    }
               }
               return View();
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