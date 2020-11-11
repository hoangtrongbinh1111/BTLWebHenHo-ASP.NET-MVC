using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebHenHo.Models;
using System.Security.Cryptography;
using BTLWebHenHo.EF.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;

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
          //use API
          public async Task<ActionResult> Quizz()
          {
               System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
               HttpClient client = new HttpClient();
               client.BaseAddress = new Uri("http://127.0.0.1/");
               client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               HttpResponseMessage response = await client.GetAsync("/api/Quizz_API").ConfigureAwait(false);
               List<tbl_quizz> model = new List<tbl_quizz>();
               if (response.IsSuccessStatusCode)
               { model = await response.Content.ReadAsAsync<List<tbl_quizz>>(); }
               return View(model);
          }
          public ActionResult Blog()
          {
               if (Request.Url.Segments.Count() == 3)
               {
                    var blog = db.tbl_blogs.ToList();
                    ViewBag.list_blog = blog.Take(4).ToList();
                    ViewBag.all_blog = blog;
                    ViewBag.sort_cate = null;
               }
               else
               {
                    var id_blog = Request.Url.Segments[3];
                    var blog = db.tbl_blogs.Where(x=>x.id_category.ToString() == id_blog).ToList();
                    ViewBag.list_blog = blog.Take(4).ToList();
                    ViewBag.all_blog = blog;
                    ViewBag.sort_cate = id_blog;
               }
               var hottest = db.tbl_blogs.OrderByDescending(z => z.qty_view).Take(1).FirstOrDefault();
               ViewBag.hottest = hottest;              
               var des_View = db.tbl_blogs.OrderByDescending(z => z.qty_view).ToList();
               ViewBag.popular_blog = des_View.Take(4).ToList();
               var category = db.tbl_category.ToList();
               ViewBag.category = category;
               return View();
          }
          public ActionResult Blog_Detail(int id)
          {

               if (Request.Url.Segments.Count() == 3) return RedirectToAction("Blog");
               var id_blog = Request.Url.Segments[3];
               var blog = db.tbl_blogs.Where(x => x.id_blog.ToString() == id_blog).FirstOrDefault();
               ViewBag.blog = blog;
               var des_View = db.tbl_blogs.OrderByDescending(z => z.qty_view).ToList();
               ViewBag.popular_blog = des_View.Take(4).ToList();
               var name = db.tbl_category.Where(x => x.id_category == blog.id_category).Select(x => x.Name).FirstOrDefault();
               ViewBag.category = name;
               var category = db.tbl_category.ToList();
               ViewBag.cate = category;
               var dt = (from x in db.tbl_blogs group x by x.id_category).ToList();
               List<int> qty_blog = new List<int>();
               for (int i = 0; i < dt.Count(); i++)
               {
                    qty_blog.Add(dt[i].Count());
               }
               ViewBag.qty_blog = qty_blog;
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
          public JsonResult older_Post(int count, string is_sort_cate)
          {
               List<tbl_blogs> blog = new List<tbl_blogs>();
               if (is_sort_cate !="")
               {
                     blog = db.tbl_blogs.Where(x=>x.id_category.ToString()==is_sort_cate).Take(count).ToList();
               }
               else
               {
                     blog = db.tbl_blogs.Take(count).ToList();
               }
               
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
          public ActionResult add_older_post(int count, string is_sort_cate)
          {
               var category = db.tbl_category.ToList();
               ViewBag.category = category;
               List<tbl_blogs> blog = new List<tbl_blogs>();
               if (is_sort_cate != "")
               {
                    blog = db.tbl_blogs.Where(x => x.id_category.ToString() == is_sort_cate).Take(count).ToList();
               }
               else
               {
                    blog = db.tbl_blogs.Take(count).ToList();
               }

               var cate = db.tbl_category.ToList();
               List<tbl_blogs> bl = new List<tbl_blogs>();
               foreach (var a in blog)
               {
                    tbl_blogs tl = new tbl_blogs();
                    tl.id_blog = a.id_blog;
                    tl.link_img = a.link_img;
                    tl.tittle = a.tittle;
                    tl.description = a.description;
                    tl.id_category = a.id_category;
                    foreach (var c in cate)
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
               return View(bl);
          }
          public JsonResult update_view(int id)
          {
               F_tbl_blogs ftb = new F_tbl_blogs();
               var row_upd = ftb.GetSingleByCondition(x => x.id_blog == id);
               row_upd.qty_view += 1;
               ftb.Update(row_upd);
               return Json(new { status=true});
          }
         
     }
}