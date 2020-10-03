namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_order
    {
        [Key]
        public int id_order { get; set; }

        public int? qty { get; set; }

        public double? amount { get; set; }

        public int? id_transaction { get; set; }

        public int? id_product { get; set; }

        public virtual tbl_products tbl_products { get; set; }

        public virtual tbl_transaction tbl_transaction { get; set; }
    }
}
