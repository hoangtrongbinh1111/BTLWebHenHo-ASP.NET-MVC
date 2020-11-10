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
using BTLWebHenHo.EF.Model;
using BTLWebHenHo.EF.Services;

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
            var id_user = get_ID_User();
               ViewBag.id_user = id_user;
            int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
            var list_order = db.tbl_order.Where(x => x.id_transaction == id_transaction).ToList();
            int num_product = 0;
            foreach(var item in list_order)
            {
                num_product += Convert.ToInt32(item.qty);
            }
            ViewBag.num_product = num_product;
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
               int num_product = 0;
               var id_user = get_ID_User();
               int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
               var list_order = db.tbl_order.Where(x => x.id_transaction == id_transaction).ToList();
               foreach (var item in list_order)
               {
                    num_product += Convert.ToInt32(item.qty);
               }
              
               ViewBag.num_product = num_product;


               return View();

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult add_mul_product(int num_order , string command)
        {
            int id_user = get_ID_User();    // lấy id_user

            if (Request.Url.Segments.Count() == 3)   // nếu ko có id product
            {


                return RedirectToAction("buy_coin");
            }
            else   // có id product
            { 
                if (command == "buy_now")   //Mua ngay
                {
                    var id = Request.Url.Segments[3];
                    var info_product = db.tbl_products.Where(x => x.id_product.ToString() == id).FirstOrDefault();
              
                    // kiểm tra xem đã có đơn hàng trước chưa
                    var check_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").ToList();
                    if (check_transaction.Count() > 0)   // nếu đã có đơn hàng
                    {
                        int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                        // kiểm tra xem có sản phẩm chưa
                        var check_product = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).ToList();
                        if (check_product.Count() > 0)  // nếu đã có sản phẩm ý rồi
                        {
                            //var qty = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).Select(x => x.qty).FirstOrDefault();

                            tbl_order order = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).FirstOrDefault();
                            int new_qty = Convert.ToInt32(order.qty) + num_order;
                            order.qty = new_qty;
                            var price = db.tbl_products.Where(x => x.id_product.ToString() == id).Select(x => x.price).FirstOrDefault();
                            int amout = Convert.ToInt32(price) * new_qty;
                            float mou = Convert.ToSingle(amout);
                            order.amount = mou;

                            // thay đổi bảng transaction
                            tbl_transaction transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                            transaction.all_amount = transaction.all_amount + price*num_order;


                            db.SaveChanges();
                        }
                        else    // chưa có sản phẩm ý
                        {
                            tbl_order order = new tbl_order();
                            order.id_product = info_product.id_product;
                            order.qty = num_order;
                            order.status = 0;
                            order.amount = info_product.price*num_order;
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
                        transaction.all_amount = info_product.price*num_order;
                        db.tbl_transaction.Add(transaction);
                        db.SaveChanges();

                        int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                        tbl_order order = new tbl_order();
                        order.id_product = info_product.id_product;
                        order.qty = num_order;
                        order.status = 0;
                        order.amount = info_product.price * num_order;
                        order.id_transaction = id_transaction;
                        db.tbl_order.Add(order);
                        // thay đổi bảng transaction
                        tbl_transaction transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                        transaction_1.all_amount = transaction_1.all_amount + order.amount;

                        db.SaveChanges();

                    }
                    int id_transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
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

                        ).Where(p => p.status == 0 && p.id_transaction == id_transaction_1).ToList();
                    foreach (var item in list)
                    {
                        var x = new list_order(item.id_product, item.name, Convert.ToInt32(item.qty), Convert.ToSingle(item.amount), Convert.ToInt32(item.id_transaction), Convert.ToInt32(item.status), Convert.ToSingle(item.price));
                        // x= list_order(item.id_product, item.name, item.qty, item.amount, item.id_transaction, item.status, item.price);
                        model.Add(x);

                    };
                    ViewBag.list_order = model;
                    float all_amount = Convert.ToSingle(db.tbl_transaction.Where(x => x.id_transaction == id_transaction_1).Select(x => x.all_amount).FirstOrDefault());
                    ViewBag.all_amount = all_amount;
                    return RedirectToAction("CheckOut");
                }
                if (command == "add_product")   // thêm vào giỏ hàng
                {
                    var id = Request.Url.Segments[3];
                    var info_product = db.tbl_products.Where(x => x.id_product.ToString() == id).FirstOrDefault();

                    // kiểm tra xem đã có đơn hàng trước chưa
                    var check_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").ToList();
                    if (check_transaction.Count() > 0)   // nếu đã có đơn hàng
                    {
                        int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                        // kiểm tra xem có sản phẩm chưa
                        var check_product = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).ToList();
                        if (check_product.Count() > 0)  // nếu đã có sản phẩm ý rồi
                        {
                            //var qty = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).Select(x => x.qty).FirstOrDefault();

                            tbl_order order = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).FirstOrDefault();
                            int new_qty = Convert.ToInt32(order.qty) + num_order;
                            order.qty = new_qty;
                            var price = db.tbl_products.Where(x => x.id_product.ToString() == id).Select(x => x.price).FirstOrDefault();
                            int amout = Convert.ToInt32(price) * new_qty;
                            float mou = Convert.ToSingle(amout);
                            order.amount = mou;

                            // thay đổi bảng transaction
                            tbl_transaction transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                            transaction.all_amount = transaction.all_amount + price * num_order;


                            db.SaveChanges();
                        }
                        else    // chưa có sản phẩm ý
                        {
                            tbl_order order = new tbl_order();
                            order.id_product = info_product.id_product;
                            order.qty = num_order;
                            order.status = 0;
                            order.amount = info_product.price * num_order;
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
                        transaction.all_amount = info_product.price * num_order;
                        db.tbl_transaction.Add(transaction);
                        db.SaveChanges();

                        int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                        tbl_order order = new tbl_order();
                        order.id_product = info_product.id_product;
                        order.qty = num_order;
                        order.status = 0;
                        order.amount = info_product.price * num_order;
                        order.id_transaction = id_transaction;
                        db.tbl_order.Add(order);
                        // thay đổi bảng transaction
                        tbl_transaction transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                        transaction_1.all_amount = transaction_1.all_amount + order.amount;
                        db.SaveChanges();

                    }

                    return RedirectToAction("buy_coin");
                }
            }
           
            
            return RedirectToAction("buy_coin");
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
                    db.tbl_transaction.Add(transaction);
                    db.SaveChanges();

                    int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                    tbl_order order = new tbl_order();
                    order.id_product = info_product.id_product;
                    order.qty = 1;
                    order.status = 0;
                    order.amount = info_product.price;
                    order.id_transaction = id_transaction;
                    db.tbl_order.Add(order);
                    db.SaveChanges();
                    tbl_transaction transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                    transaction_1.all_amount = transaction_1.all_amount + order.amount;
                    db.SaveChanges();

                }
                int id_transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
               
                
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
            //lay thong tin ca nhan
            var info_user = db.UserInfoes.Where(x => x.UserID == id_user).FirstOrDefault();
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

        public ActionResult Delete()
        {
            int id_user = get_ID_User();

            if (Request.Url.Segments.Count() == 3)
            {
                return RedirectToAction("CheckOut");
            }
            else
            {
                int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                var id = Request.Url.Segments[3];
                var info_product = db.tbl_products.Where(x => x.id_product.ToString() == id).FirstOrDefault();
                tbl_order order = db.tbl_order.Where(x => x.id_product.ToString() == id &&x.status==0 && x.id_transaction==id_transaction).FirstOrDefault();

                // thay đổi bảng tbl transaction
                tbl_transaction transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                transaction.all_amount = transaction.all_amount - order.amount;

                db.tbl_order.Remove(order);
                db.SaveChanges();

                var model = new List<list_order>();
                var list = db.tbl_products.Join(db.tbl_order, c => c.id_product, p => p.id_product,
                    (c, p) => new
                    {
                        name = c.name.ToString(),
                        id_product =Convert.ToInt32( c.id_product),
                        qty =Convert.ToInt32( p.qty),
                        amount =Convert.ToSingle( p.amount),
                        id_transaction =Convert.ToInt32( p.id_transaction),
                        status =Convert.ToInt32( p.status),
                        price =Convert.ToSingle( c.price),

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
            return RedirectToAction("CheckOut");
        }

        public ActionResult DeleteAll()
        {
            int id_user = get_ID_User();
            int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();

            var list_order = db.tbl_order.Where(x => x.id_transaction == id_transaction).ToList();
            foreach(var item in list_order)
            {
                db.tbl_order.Remove(item);

            }
            tbl_transaction transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
            transaction.all_amount = 0;
            db.SaveChanges();
            return RedirectToAction("CheckOut");
        }
        [HttpPost]
        public JsonResult  change_qty_order( int id,int qty)
        {
            int id_user = get_ID_User();
            var info_product = db.tbl_products.Where(x => x.id_product == id).FirstOrDefault();
            int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
            tbl_order order = db.tbl_order.Where(x => x.id_product == id && x.status == 0 && x.id_transaction == id_transaction).FirstOrDefault();
            order.qty = qty;
            order.amount = qty * info_product.price;
            db.SaveChanges();
            
            update_transaction(id_transaction);
            tbl_transaction transaction = db.tbl_transaction.Where(x =>  x.id_transaction == id_transaction).FirstOrDefault();
            var all_amount = transaction.all_amount;
            // tbl_order order = new tbl_order();
           

            return Json(new {
                //qty=qty,
               
                all_amount=Convert.ToInt32( all_amount),
                amount = Convert.ToInt32(order.amount),
            });
        }
        //cập nhật transaction
        public void update_transaction(int transaction)
        {

            WebHenHoDbContext db = new WebHenHoDbContext();
            var list_order = db.tbl_order.Where(x => x.id_transaction == transaction).ToList();
            float all_amount = 0;
            foreach (var item in list_order)
            {
                all_amount += Convert.ToSingle(item.amount);

            }
            // thay đổi bảng tbl transaction
            tbl_transaction transaction_1 = db.tbl_transaction.Where(x => x.id_transaction == transaction).FirstOrDefault();
            transaction_1.all_amount = all_amount;
            db.SaveChanges();

        }

        [HttpPost]
        public ActionResult get_location(string fullname,string telephone,string address,string command)
        {
            string location = "";
            if(command=="close")
            {
                ViewBag.get_location = location;
                return RedirectToAction("CheckOut");
            }
            if(command=="complete")
            {
                GeoLocationController local = new GeoLocationController();
                
                 location = local.get_location(address);
                ViewBag.get_location = location;
                return RedirectToAction("CheckOut");
            }
            return RedirectToAction("CheckOut");
        }

    }

}