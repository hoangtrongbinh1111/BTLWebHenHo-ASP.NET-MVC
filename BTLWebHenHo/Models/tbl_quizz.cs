namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_quizz
    {
        [Key]
        public int id_quizz { get; set; }

        public string img_tittle { get; set; }

        [StringLength(200)]
        public string quizz_tittle { get; set; }

        public int? qty_player { get; set; }
    }
}
