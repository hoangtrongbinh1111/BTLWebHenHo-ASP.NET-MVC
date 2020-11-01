using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTLWebHenHo.EF.Infrastructure;
using BTLWebHenHo.Models;
namespace BTLWebHenHo.EF.Services
{
    public class F_Cart 
    {
        public void add_product(int id_product, int id_user,int qty)
        {
            tbl_order order = new tbl_order();


        }

        public void update_transaction(int transaction)
        {
            
            WebHenHoDbContext db = new WebHenHoDbContext();
            var list_order = db.tbl_order.Where(x => x.id_transaction == transaction).ToList();
            float all_amount = 0;
            foreach(var item in list_order)
            {
                all_amount +=Convert.ToSingle( item.amount);

            }
            // thay đổi bảng tbl transaction
            tbl_transaction transaction_1 = db.tbl_transaction.Where(x =>  x.id_transaction == transaction).FirstOrDefault();
            transaction_1.all_amount = all_amount;
            db.SaveChanges();

        }
         
    }
}