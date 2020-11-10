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
            int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
            var list_order = db.tbl_order.Where(x => x.id_transaction == id_transaction).ToList();
            int num_product = 0;
            foreach (var item in list_order)
            {
                num_product += Convert.ToInt32(item.qty);
            }
            ViewBag.num_product = num_product;
            return View(model);
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
                create_new_transaction(0);
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
                float all_amount = Convert.ToSingle(db.tbl_transaction.Where(x => x.id_transaction == id_transaction_1).Select(x => x.all_amount).FirstOrDefault());
                ViewBag.all_amount = all_amount;
                ViewBag.list_order = model;


            }

            // truyen cac tinh
            var provincial = db.Provinces.ToList().OrderBy(x => x.Name);
            ViewBag.provincial = provincial;
            //lấy dữ liệu các tỉnh các kiểu trong tbl_transaction;
            ViewBag.trans = null;
            var transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").FirstOrDefault();
            if (transaction != null)
            {
                tbl_transaction trans = new tbl_transaction();
                trans.phone = transaction.phone;
                trans.address = transaction.address;
                trans.ship = transaction.ship;
                trans.name_customer = transaction.name_customer;
                trans.translate = transaction.translate;
                ViewBag.trans = trans;
            }


            return View();
        }

        public ActionResult shop()
        {

            if (Request.Url.Segments.Count() == 3)
            {
                return RedirectToAction("buy_coin");
            }

            var id = Request.Url.Segments[3];
            var info_product = db.tbl_products.Where(x => x.id_product.ToString() == id).FirstOrDefault();
            ViewBag.info_product = info_product;



            return View();

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult add_mul_product(int num_order, string command)
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
                        float price =Convert.ToSingle( info_product.price * num_order);
                        create_new_transaction(price);
                        //tbl_transaction transaction = new tbl_transaction();
                        //transaction.UserID = id_user;
                        //transaction.status = "false";
                        //transaction.all_amount = info_product.price * num_order;
                        //db.tbl_transaction.Add(transaction);
                        //db.SaveChanges();

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
            //truyen mã sản phẩm qua url ví dụ add_product/3 là thêm 1 sp có mã số 3 vào giỏ hàng
            if (Request.Url.Segments.Count() == 3)
            {
                return RedirectToAction("buy_coin");
            }
            else
            {
                var id = Request.Url.Segments[3];
                var info_product = db.tbl_products.Where(x => x.id_product.ToString() == id).FirstOrDefault();
                // ViewBag.info_product = info_product;

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
                    create_new_transaction(Convert.ToSingle(info_product.price));
                   

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

                    ).Where(p => p.status == 0 && p.id_transaction == id_transaction_1).ToList();
                foreach (var item in list)
                {
                    var x = new list_order(item.id_product, item.name, Convert.ToInt32(item.qty), Convert.ToSingle(item.amount), Convert.ToInt32(item.id_transaction), Convert.ToInt32(item.status), Convert.ToSingle(item.price));

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
                tbl_order order = db.tbl_order.Where(x => x.id_product.ToString() == id && x.status == 0 && x.id_transaction == id_transaction).FirstOrDefault();

                // thay đổi bảng tbl transaction
                tbl_transaction transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
                transaction.all_amount = transaction.all_amount - order.amount;

                db.tbl_order.Remove(order);
                db.SaveChanges();

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
            foreach (var item in list_order)
            {
                db.tbl_order.Remove(item);

            }
            tbl_transaction transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false" && x.id_transaction == id_transaction).FirstOrDefault();
            transaction.all_amount = 0;
            db.SaveChanges();
            return RedirectToAction("CheckOut");
        }
        [HttpPost]
        public JsonResult change_qty_order(int id, int qty)
        {
            int id_user = get_ID_User();
            var info_product = db.tbl_products.Where(x => x.id_product == id).FirstOrDefault();
            int id_transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
            tbl_order order = db.tbl_order.Where(x => x.id_product == id && x.status == 0 && x.id_transaction == id_transaction).FirstOrDefault();
            order.qty = qty;
            order.amount = qty * info_product.price;

            db.SaveChanges();

            update_transaction(id_transaction);
            tbl_transaction transaction = db.tbl_transaction.Where(x => x.id_transaction == id_transaction).FirstOrDefault();
            var all_amount = transaction.all_amount;
            var all_money = transaction.all_money;
            // tbl_order order = new tbl_order();


            return Json(new {
                //qty=qty,

                all_amount = Convert.ToInt32(all_amount),
                amount = Convert.ToInt32(order.amount),
                all_money=Convert.ToInt32(all_money),
            });
        }
        public class distric
            {
            private int id { get; set; }
        private string dis { get; set; }
        public distric(int id,string dis)
        {
            this.id = id;
            this.dis = dis;
        }
            }
        // change_provincial
        [HttpPost]
        public JsonResult change_provi( int val_provi)
        {
            string x = "<option value=\"0\" checked=\"checked\">Chọn quận, huyện</option>";
            var distric = db.Districts.Where(a => a.ProvinceId == val_provi).OrderBy(a => a.Name).ToList();
            foreach(var item in distric)
            {
                string y = "<br><option value=\""+item.Id+"\">"+item.Name+"</option>";
                x = x + y;
            }
         
            return Json(new
            {
                distric = x,
            });

        }

        // change_distric
        [HttpPost]
        public JsonResult change_distric(int val_distric)
        {
            string x = "<option value=\"0\" checked=\"checked\">Chọn xã, phường</option>";
            var ward = db.Wards.Where(a => a.DistrictID == val_distric).OrderBy(a=>a.Name).ToList();
            foreach (var item in ward)
            {
                string y = "<br><option value=\"" + item.Id + "\">" + item.Name + "</option>";
                x = x + y;
            }
            return Json(new
            {
                ward = x,
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
            transaction_1.all_money = transaction_1.all_amount + transaction_1.ship;
            db.SaveChanges();

        }

        // thay đổi địa chỉ
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public JsonResult get_location(string fullname,string telephone,string provincial,string distric,string ward, string address)
        {
            string[] error=new string[6];
            for (int i = 0; i<6;i++)
            {
                error[i] = "";
            }
            
            if (fullname == "")
            {
                 error[0] = "Bạn chưa điền họ tên";
            }
            if(telephone == "")
            {
                error[1] = "Bạn chưa điền số điện thoại";
            }
            if (provincial == "Chọn tỉnh")
            {
                error[2] = "Bạn chưa chọn tỉnh, thành phố";
            }
            if (distric == "Chọn quận, huyện")
            {
                error[3] = "Bạn chưa chọn quận huyện";
            }
            if (ward == "Chọn xã, phường")
            {
                error[4] = "Bạn chưa chọn xã, phường";
            }
            if (address == "")
            {
                error[5] = "Bạn chưa điền địa chỉ";
            }

            if(error[0]==""&& error[1] == "" && error[2] == "" && error[3] == "" && error[4] == "" && error[5] == "" )
            {
                // lưu vào cơ sở dữ liệu
                float ship = 0;
                if(provincial=="Hà Nội"||provincial== "Hồ Chí Minh")
                {
                    ship = 10000;
                }
                 else if( provincial=="Hưng Yên")
                         {
                    ship = 5000;
                }
                else
                {
                    ship = 15000;
                }
                int id_provin = db.Provinces.Where(x => x.Name == provincial).Select(x => x.Id).FirstOrDefault();
                int id_dis = db.Districts.Where(x => x.Name == distric && x.ProvinceId == id_provin).Select(x => x.Id).FirstOrDefault();
                string type_dis = db.Districts.Where(x => x.Id == id_dis).Select(x => x.Type).FirstOrDefault();
                string type_ward = db.Wards.Where(x => x.Name == ward && x.DistrictID == id_dis).Select(x => x.Type).FirstOrDefault();
                string location = "";
                location = address+", "+type_ward+" "+ ward +", " +type_dis+" "+ distric+ ", "+ provincial ;

                int id_user = get_ID_User();
                int id_transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
                tbl_transaction transaction = db.tbl_transaction.Where(x => x.id_transaction == id_transaction_1).FirstOrDefault();
                if (transaction.translate == "fast")
                {
                    ship += 2000;
                }
                if (transaction.translate == "slow")
                {
                    ship -= 2000;
                }
                string all= update_address(fullname, telephone, location, ship);
                return Json(new
                {
                    error = error,
                    location = location,
                    fullname = fullname,
                    telephone = telephone,
                    ship = ship,
                    all =all,
                }

                    );
            }
            else
            {
                return Json(new
                {
                    error = error,
                   
                });
            }
            



            //return Json(new
            //{
            //    ward = location,
            //    error = "",
            //});
        }

        // Lưu địa chỉ vào cơ sở dữ liệu
        public string update_address(string fullname,string telephone,string location,float ship)
        {
            int id_user = get_ID_User();
            int id_transaction_1 = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").Select(x => x.id_transaction).FirstOrDefault();
            tbl_transaction transaction = db.tbl_transaction.Where(x => x.id_transaction == id_transaction_1).FirstOrDefault();
            transaction.phone = telephone;
            transaction.name_customer = fullname;
            transaction.address = location;          
            transaction.ship = ship;
            transaction.all_money = transaction.all_amount + ship;
            db.SaveChanges();
            string all = db.tbl_transaction.Where(x => x.id_transaction==id_transaction_1).Select(x => x.all_money).FirstOrDefault().ToString();
            return all;
        }

        // Tạo mới transaction mới
        public void create_new_transaction(float price)
        {
            int id_user = get_ID_User();
            var transaction = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "true").ToList();
            int count = transaction.Count();    
            if (count>0)
            {
                count--;
                tbl_transaction trans = new tbl_transaction();
                trans.phone = transaction[count].phone;
                trans.address = transaction[count].address;
                trans.ship = transaction[count].ship;
                trans.name_customer = transaction[count].name_customer;
                trans.UserID = id_user;
                trans.status = "false";
                trans.all_amount = price;
                trans.all_money = trans.ship + trans.all_amount;
                trans.translate = "normal";
                db.tbl_transaction.Add(trans);
            }
            else
            {
                tbl_transaction transaction_1 = new tbl_transaction();
                transaction_1.UserID = id_user;
                transaction_1.status = "false";
                transaction_1.all_amount = 0;
                transaction_1.ship = 0;
                transaction_1.all_money = 0;
                transaction_1.translate = "normal";
                db.tbl_transaction.Add(transaction_1);
            }
            db.SaveChanges();
        }

        // xử lý thay đổi phương thức giao hàng
        [HttpPost]
        public JsonResult translate_nomarl(string translate)
        {
            int id_user = get_ID_User();
            tbl_transaction trans = db.tbl_transaction.Where(x => x.UserID == id_user && x.status == "false").FirstOrDefault();
            // xem giao hàng loại nào
            if (translate == "normal")
            {
                trans.translate = "normal";
                if (trans.ship == 10000 || trans.ship == 8000 || trans.ship == 12000)
                {
                    trans.ship = 10000;
                }
                else if (trans.ship == 5000 || trans.ship == 3000 || trans.ship == 7000)
                {
                    trans.ship = 5000;
                }
                else
                {
                    trans.ship = 15000;
                }
            }
            else if(translate=="fast")
            {
                trans.translate = "fast";
                if (trans.ship == 10000 || trans.ship == 8000 || trans.ship == 12000)
                {
                    trans.ship = 12000;
                }
                else if (trans.ship == 5000 || trans.ship == 3000 || trans.ship == 7000)
                {
                    trans.ship = 7000;
                }
                else
                {
                    trans.ship = 17000;
                }
            }
            else if(translate=="slow")
            {
                trans.translate = "slow";
                if (trans.ship == 10000 || trans.ship == 8000 || trans.ship == 12000)
                {
                    trans.ship = 8000;
                }
                else if (trans.ship == 5000 || trans.ship == 3000 || trans.ship == 7000)
                {
                    trans.ship = 3000;
                }
                else
                {
                    trans.ship = 13000;
                }
            }

            
            trans.all_money = trans.ship + trans.all_amount;
            db.SaveChanges();
            return Json(new
            {
                ship = trans.ship,
                all=trans.all_money,

            });
        }

    }

}