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

namespace BTLWebHenHo.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult payment()
        {
            return View();
        }
          public ActionResult buy_coin()
          {
               return View();
          }
          
          public ActionResult shop()
          {
               return View();
          }
          public ActionResult CheckOut()
          {
               return View();
          }
         
     }
}