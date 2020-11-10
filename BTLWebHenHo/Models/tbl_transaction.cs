namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_transaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_transaction()
        {
            tbl_order = new HashSet<tbl_order>();
        }

        [Key]
        public int id_transaction { get; set; }

        public double? all_amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_date { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        public int? UserID { get; set; }

        [StringLength(15)]
        public string phone { get; set; }

        [StringLength(100)]
        public string address { get; set; }

        [StringLength(50)]
        public string name_customer { get; set; }

        public double? ship { get; set; }

        public double? all_money { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_order> tbl_order { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
