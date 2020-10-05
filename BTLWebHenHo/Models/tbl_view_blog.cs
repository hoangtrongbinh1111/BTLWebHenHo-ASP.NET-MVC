namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_view_blog
    {
        [Key]
        public int id_view { get; set; }

        public int? view_qty { get; set; }

        public int? id_blog { get; set; }

        public virtual tbl_blogs tbl_blogs { get; set; }
    }
}
