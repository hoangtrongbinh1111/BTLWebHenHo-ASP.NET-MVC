namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_codes
    {
        [Key]
        public int id_code { get; set; }

        [StringLength(20)]
        public string name_code { get; set; }

        [StringLength(50)]
        public string content_code { get; set; }
    }
}
