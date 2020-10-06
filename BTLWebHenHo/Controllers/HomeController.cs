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
               var hottest = db.tbl_blogs.OrderByDescending(z => z.qty_view).Take(1).FirstOrDefault();
               ViewBag.hottest = hottest;
               var blog = db.tbl_blogs.ToList();              
               ViewBag.list_blog = blog.Take(4).ToList();
               ViewBag.all_blog = blog;
               var des_View = db.tbl_blogs.OrderByDescending(z => z.qty_view).ToList();
               ViewBag.popular_blog = des_View.Take(4).ToList();
               var category = db.tbl_category.ToList();
               ViewBag.category = category;
               return View();
          }
          public ActionResult Blog_Detail()
          {
               var id_blog = Request.Url.Segments[3];
               var blog = db.tbl_blogs.Where(x => x.id_blog.ToString() == id_blog).FirstOrDefault();
               ViewBag.blog = blog;
               var name = db.tbl_category.Where(x => x.id_category == blog.id_category).Select(x => x.Name).FirstOrDefault();
               ViewBag.category = name;
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
          [HttpPost]
          public JsonResult older_Post(int count)
          {
               var blog = db.tbl_blogs.Take(count).ToList();
               var cate = db.tbl_category.ToList();              
               List<tbl_blogs> bl = new List<tbl_blogs>();
               foreach(var a in blog)
               {
                    tbl_blogs tl = new tbl_blogs();
                    tl.id_blog = a.id_blog;
                    tl.link_img = a.link_img;
                    tl.tittle = a.tittle;
                    tl.description = a.description;
                    foreach(var c in cate)
                    {
                         if (c.id_category == a.id_category)
                         {
                              tl.content = c.Name;
                         }
                    }                  
                    tl.create_date = a.create_date;
                    tl.qty_view = a.qty_view;
                    bl.Add(tl);
               }
              
               
               return Json(bl,JsonRequestBehavior.AllowGet);
          }          

     }
}