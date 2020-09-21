namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Profile_User
    {
        [Key]
        public int stt { get; set; }

        [StringLength(50)]
        public string fullname { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngsinh { get; set; }

        [StringLength(50)]
        public string diachi { get; set; }

        [StringLength(3)]
        public string gioitinh { get; set; }

        public int? UserID { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
