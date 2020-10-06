namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_blogs
    {
        [Key]
        public int id_blog { get; set; }

        public string link_img { get; set; }

        [StringLength(100)]
        public string tittle { get; set; }

        public string description { get; set; }

        public string content { get; set; }

        public int? id_category { get; set; }

        [StringLength(50)]
        public string create_date { get; set; }

        public int? qty_view { get; set; }

        public virtual tbl_category tbl_category { get; set; }
    }
}
