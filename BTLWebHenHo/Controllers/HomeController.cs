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
          WebHenHoDbContext db = new WebHenHoDbContext();         
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
               var hottest = db.tbl_blogs.Where(x => x.hottest == 1).FirstOrDefault();
               ViewBag.hottest = hottest;
               var blog = db.tbl_blogs.ToList();              
               ViewBag.list_blog = blog.Take(4); 
               var des_View = db.tbl_view_blog.OrderByDescending(z => z.view_qty).ToList();
               ViewBag.popular_blog = des_View.Take(5);
               return View();
          }

          [HttpPost]
          public ActionResult feedback_web(tbl_collects Collect)
          {
               tbl_collects tc = new tbl_collects {
                    first_name = Collect.first_name,
                    last_name=Collect.last_name,
                    email=Collect.email,
                    tittle=Collect.tittle,
                    content=Collect.content
               };
               db.tbl_collects.Add(tc);
               db.SaveChanges();
               return RedirectToAction("MTAhub_Story");
          }
          public ActionResult Older_Post()
          {
               
               return RedirectToAction("Blog");
          }          

     }
}