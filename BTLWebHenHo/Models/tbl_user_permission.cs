namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_user_permission
    {
        [Key]
        public int id_user_permission { get; set; }

        public int? id_permission { get; set; }

        public int? id_User_Type { get; set; }

        [StringLength(200)]
        public string comment { get; set; }

        public virtual tbl_permission tbl_permission { get; set; }

        public virtual tbl_User_Type tbl_User_Type { get; set; }
    }
}
