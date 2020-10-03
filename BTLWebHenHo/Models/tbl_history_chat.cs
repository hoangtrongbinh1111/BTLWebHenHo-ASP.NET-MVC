namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_history_chat
    {
        [Key]
        public int id_chat_history { get; set; }

        public string list_id_User { get; set; }

        public string list_msg { get; set; }
    }
}
