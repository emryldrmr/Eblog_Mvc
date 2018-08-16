namespace Eblog.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("blog.yetki")]
    public partial class yetki
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public yetki()
        {
            kullanici = new HashSet<kullanici>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string YetkiAdi { get; set; }

        [StringLength(20)]
        public string Tarih { get; set; }

        [Column(TypeName = "bit")]
        public bool Onay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kullanici> kullanici { get; set; }
    }
}
