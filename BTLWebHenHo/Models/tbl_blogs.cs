namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_blogs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_blogs()
        {
            tbl_view_blog = new HashSet<tbl_view_blog>();
        }

        [Key]
        public int id_blog { get; set; }

        public string link_img { get; set; }

        [StringLength(100)]
        public string tittle { get; set; }

        public string description { get; set; }

        public string content { get; set; }

        [StringLength(50)]
        public string create_date { get; set; }

        public int? hottest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_view_blog> tbl_view_blog { get; set; }
    }
}
