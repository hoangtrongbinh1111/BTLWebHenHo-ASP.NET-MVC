namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_collects
    {
        [Key]
        public int id_collect { get; set; }

        public string img_link { get; set; }

        public int? UserID { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
