namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_permission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_permission()
        {
            tbl_user_permission = new HashSet<tbl_user_permission>();
        }

        [Key]
        public int id_permission { get; set; }

        [StringLength(200)]
        public string name { get; set; }

        [StringLength(200)]
        public string comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_user_permission> tbl_user_permission { get; set; }
    }
}
