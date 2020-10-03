namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_chat
    {
        [Key]
        public int id_chat { get; set; }

        public int? id_main_user { get; set; }

        public int? id_other_user { get; set; }

        public string content { get; set; }
    }
}
