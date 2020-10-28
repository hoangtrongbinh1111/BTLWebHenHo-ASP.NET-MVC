namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class new_imgage
    {
        [Key]
        public int id_new_img { get; set; }

        public string new_img { get; set; }
    }
}
