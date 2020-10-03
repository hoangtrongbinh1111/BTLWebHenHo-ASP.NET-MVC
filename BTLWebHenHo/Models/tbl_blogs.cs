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

        [StringLength(100)]
        public string tittle { get; set; }

        public string description { get; set; }

        public string content { get; set; }

        [Column(TypeName = "date")]
        public DateTime? create_date { get; set; }
    }
}
