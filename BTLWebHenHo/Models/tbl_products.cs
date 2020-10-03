namespace BTLWebHenHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_products()
        {
            tbl_image_links = new HashSet<tbl_image_links>();
            tbl_order = new HashSet<tbl_order>();
        }

        [Key]
        public int id_product { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public double? price { get; set; }

        public string image_link { get; set; }

        public int? views_people { get; set; }

        public int? num_of_pro { get; set; }

        public string description { get; set; }

        public string content { get; set; }

        public int? num_of_sold_pro { get; set; }

        [StringLength(20)]
        public string code { get; set; }

        public int? id_catalog { get; set; }

        public virtual tbl_catalogs tbl_catalogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_image_links> tbl_image_links { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_order> tbl_order { get; set; }
    }
}
