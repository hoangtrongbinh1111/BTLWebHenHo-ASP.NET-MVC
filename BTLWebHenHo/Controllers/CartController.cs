using BTLWebHenHo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BTLWebHenHo.Models;
using BTLWebHenHo.EF.Model;

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
        public ActionResult add_product()
        {
            int id_user = get_ID_User();
            
            if (Request.Url.Segments.Count() == 3)
            {
                return RedirectToAction("buy_coin");
            }
            else
            {
               var  id = Request.Url.Segments[3];
                var info_product = db.tbl_products.Where(x => x.id_product.ToString() == id).FirstOrDefault();
                // ViewBag.info_product = info_product;

                // kiểm tra xem đã có đơn hàng trước chưa
                var check_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").ToList();
                if(check_transaction.Count()>0)   // nếu đã có đơn hàng
                {
                    int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                    // kiểm tra xem có sản phẩm chưa
                    var check_product = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction==id_transaction).ToList();
                    if (check_product.Count() > 0)  // nếu đã có sản phẩm ý rồi
                    {
                        //var qty = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).Select(x => x.qty).FirstOrDefault();
                        
                        tbl_order order = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).FirstOrDefault();
                        int new_qty = Convert.ToInt32(order.qty) + 1;
                        order.qty = new_qty;
                        var price = db.tbl_products.Where(x => x.id_product.ToString() == id).Select(x => x.price).FirstOrDefault();
                        int amout = Convert.ToInt32(price) * new_qty;
                        float mou = Convert.ToSingle(amout);
                        order.amount = mou;

                        // thay đổi bảng transaction
                        tbl_transaction transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                        transaction.all_amount = transaction.all_amount + price;


                        db.SaveChanges();
                    }
                    else    // chưa có sản phẩm ý
                    {
                        tbl_order order = new tbl_order();
                        order.id_product = info_product.id_product;
                        order.qty = 1;
                        order.status = 0;
                        order.amount = info_product.price;
                        order.id_transaction = id_transaction;
                        db.tbl_order.Add(order);
                        // thay đổi bảng transaction
                        tbl_transaction transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                        transaction.all_amount = transaction.all_amount + order.amount;

                        db.SaveChanges();

                    }
                }

                else   // chưa có đơn hàng
                {
                    tbl_transaction transaction = new tbl_transaction();
                    transaction.UserID = id_user;
                    transaction.status = "false";
                    transaction.all_amount = info_product.price;

                    db.SaveChanges();

                }
                int id_transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                // var list_order = db.tbl_order.Where(x => x.status == 0 && x.id_transaction ==id_transaction_1 ).ToList();
                //tbl_products pro = new tbl_products();
                //var list = db.tbl_order.GroupBy(tbl_products ,x => x.tbl_products.ToString() == id);
                //ViewBag.list_order = list_order.GroupBy(tbl_products, tbl_order.id_product == tbl_products.id_product);
                //var list_order = from a in db.tbl_products join b in db.tbl_order on a.id_product equals b.id_product where b.status == 0 && b.id_transaction == id_transaction_1 select new {
                //    id_product = a.id_product,
                //    name = a.name,
                //    price = a.price,
                //    id_transaction = b.id_transaction,
                //    qty = b.qty,
                //    amount = b.amount,
                //} ;
                
                var model = new List<list_order>();
                var list = db.tbl_products.Join(db.tbl_order, c => c.id_product, p => p.id_product,
                    (c, p) => new
                    {
                        name = c.name,
                        id_product = c.id_product,
                        qty = p.qty,
                        amount = p.amount,
                        id_transaction = p.id_transaction,
                        status = p.status,
                        price = c.price,
                      
                    }

                    ).Where(p => p.status==0 && p.id_transaction== id_transaction_1).ToList();
                foreach (var item in list)
                {
                    var x = new list_order(item.id_product, item.name, Convert.ToInt32( item.qty),Convert.ToSingle( item.amount),Convert.ToInt32( item.id_transaction),Convert.ToInt32( item.status),Convert.ToSingle( item.price));
                    // x= list_order(item.id_product, item.name, item.qty, item.amount, item.id_transaction, item.status, item.price);
                    model.Add(x);
                    
                };
                ViewBag.list_order = model;
                float all_amount = Convert.ToSingle(db.tbl_transaction.Where(x => x.id_transaction == id_transaction_1).Select(x => x.all_amount).FirstOrDefault());
                ViewBag.all_amount = all_amount;
                //  model = list_order.ToList();
                //  return RedirectToAction("CheckOut");


            }




            
            return RedirectToAction("CheckOut");
        }

        private ActionResult RedirectToAction(List<object> model, string v)
        {
            throw new NotImplementedException();
        }

        // lấy id của người dùng
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
        public ActionResult CheckOut()
          {
            int id_user = get_ID_User();
            // kiểm tra xem đã có đơn hàng trước chưa
            var check_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").ToList();
            if (check_transaction.Count() > 0)   // nếu đã có đơn hàng
            {
                int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();

                //var list_order = db.tbl_order.Where(x => x.status == 0 && x.id_transaction == id_transaction).ToList();
                var model = new List<list_order>();
                var list = db.tbl_products.Join(db.tbl_order, c => c.id_product, p => p.id_product,
                    (c, p) => new
                    {
                        name = c.name,
                        id_product = c.id_product,
                        price = c.price,
                        qty = p.qty,
                        amount = p.amount,
                        status = p.status,
                        id_transaction = p.id_transaction
                    }

                    ).Where(p => p.status == 0 && p.id_transaction == id_transaction).ToList();
                foreach (var item in list)
                {
                    var x = new list_order(item.id_product, item.name, Convert.ToInt32(item.qty), Convert.ToSingle(item.amount), Convert.ToInt32(item.id_transaction), Convert.ToInt32(item.status), Convert.ToSingle(item.price));
                    // x= list_order(item.id_product, item.name, item.qty, item.amount, item.id_transaction, item.status, item.price);
                    model.Add(x);

                };
                ViewBag.list_order = model;
                float all_amount = Convert.ToSingle(db.tbl_transaction.Where(x => x.id_transaction == id_transaction).Select(x => x.all_amount).FirstOrDefault());
                ViewBag.all_amount = all_amount;
            }
            else  // chưa có đơn hàng
            {
                tbl_transaction transaction = new tbl_transaction();
                transaction.UserID = id_user;
                transaction.status = "false";
                transaction.all_amount = 0;

                db.SaveChanges();
                int id_transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                // var list_order = db.tbl_order.Where(x => x.status == 0 && x.id_transaction ==id_transaction_1 ).ToList();
                //tbl_products pro = new tbl_products();
                //var list = db.tbl_order.GroupBy(tbl_products ,x => x.tbl_products.ToString() == id);
                //ViewBag.list_order = list_order.GroupBy(tbl_products, tbl_order.id_product == tbl_products.id_product);
                //var list_order = from a in db.tbl_products
                //                 join b in db.tbl_order on a.id_product equals b.id_product
                //                 where b.status == 0 && b.id_transaction == id_transaction_1
                //                 select new
                //                 {
                //                     id_product = a.id_product,
                //                     name = a.name,
                //                     price = a.price,
                //                     id_transaction = b.id_transaction,
                //                     qty = b.qty,
                //                     amount = b.amount,
                //                 };
                var model = new List<list_order>();
                var list = db.tbl_products.Join(db.tbl_order, c => c.id_product, p => p.id_product,
                   (c, p) => new
                   {
                       name = c.name,
                       id_product = c.id_product,
                       price = c.price,
                       qty = p.qty,
                       amount = p.amount,
                      status = p.status,
                       id_transaction = p.id_transaction
                   }

                   ).Where(p => p.status == 0 && p.id_transaction == id_transaction_1).ToList();
                foreach (var item in list)
                {
                    var x = new list_order(item.id_product, item.name, Convert.ToInt32(item.qty), Convert.ToSingle(item.amount), Convert.ToInt32(item.id_transaction), Convert.ToInt32(item.status), Convert.ToSingle(item.price));
                    // x= list_order(item.id_product, item.name, item.qty, item.amount, item.id_transaction, item.status, item.price);
                    model.Add(x);

                };
                float all_amount =Convert.ToSingle( db.tbl_transaction.Where(x => x.id_transaction == id_transaction_1).Select(x=> x.all_amount).FirstOrDefault());
                ViewBag.all_amount = all_amount;
                ViewBag.list_order = model;
                
               
            }
           


            return View();
          }
         
     }
}