namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_image_links
    {
        [Key]
        public int id_image { get; set; }

        public string image_link { get; set; }

        public int? id_product { get; set; }

        public virtual tbl_products tbl_products { get; set; }
    }
}
