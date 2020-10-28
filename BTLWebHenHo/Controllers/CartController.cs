using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebHenHo.Models;

namespace BTLWebHenHo.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        WebHenHoDbContext db = new WebHenHoDbContext();
        public ActionResult payment()
        {
            return View();
        }
          public ActionResult buy_coin()
          {
           // var list_product = db.tbl_products.ToList();
            var catalog = db.tbl_catalogs.ToList();
            // ViewBag.list_product = list_product;
          //  var model = new tbl_products();
            ViewData.Model = new tbl_products();
            ViewBag.catalog = catalog;
            // var model = new tbl_products().ListAll();
            var model = db.tbl_products.ToList();

               return View(model);
          }
          
          public ActionResult shop()
          {
            
            if (Request.Url.Segments.Count()==3)
            {
              return   RedirectToAction("buy_coin");
            }
            
                var id = Request.Url.Segments[3];
                var info_product = db.tbl_products.Where(x => x.id_product.ToString() == id).FirstOrDefault();
                ViewBag.info_product = info_product;

                
            
            return View();
        }
          public ActionResult CheckOut()
          {
               return View();
          }
     }
}