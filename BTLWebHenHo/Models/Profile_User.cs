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
        public string NickName { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string phone { get; set; }

        [StringLength(50)]
        public string address_user { get; set; }

        [StringLength(50)]
        public string coins { get; set; }

        [StringLength(50)]
        public string avatar { get; set; }

        [StringLength(50)]
        public string birthday { get; set; }

        public int? height { get; set; }

        [StringLength(50)]
        public string body { get; set; }

        [StringLength(50)]
        public string blood { get; set; }

        [StringLength(50)]
        public string national_user { get; set; }

        [StringLength(50)]
        public string language_user { get; set; }

        [StringLength(50)]
        public string education { get; set; }

        [StringLength(50)]
        public string job { get; set; }

        [StringLength(50)]
        public string income { get; set; }

        [StringLength(50)]
        public string martial_status { get; set; }

        [StringLength(50)]
        public string baby_status { get; set; }

        [StringLength(50)]
        public string want_baby_status { get; set; }

        [StringLength(50)]
        public string live_status { get; set; }

        [StringLength(100)]
        public string hobbies { get; set; }

        [StringLength(50)]
        public string character_user { get; set; }

        [StringLength(50)]
        public string public_relationship { get; set; }

        [StringLength(50)]
        public string want_meet { get; set; }

        [StringLength(50)]
        public string want_marry { get; set; }

        [StringLength(50)]
        public string ready_do_homework { get; set; }

        [StringLength(50)]
        public string freeday { get; set; }

        [StringLength(50)]
        public string wine { get; set; }

        [StringLength(50)]
        public string smoke { get; set; }

        [StringLength(50)]
        public string pay_first_meet { get; set; }

        [StringLength(50)]
        public string vip { get; set; }

        [StringLength(50)]
        public string date_vip { get; set; }

        [StringLength(50)]
        public string family { get; set; }

        public string list_thumb { get; set; }

        public int? hide_age { get; set; }

        [StringLength(10)]
        public string gender { get; set; }

        public int? UserID { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
