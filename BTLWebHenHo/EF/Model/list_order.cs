using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebHenHo.EF.Model
{
    public class list_order
    {
        public int id_product { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public float amount { get; set; }
        public int id_transaction { get; set; }

        public int status { get; set; }
        public float price { get; set; }
        public list_order(int id_product,string name ,int qty, float amount,int id_transaction,int status,float price)
        {
            this.id_product = id_product;this.name = name;this.qty = qty;this.amount = amount;this.id_transaction = id_transaction; this.status = status;this.price = price;
        }
    }
}