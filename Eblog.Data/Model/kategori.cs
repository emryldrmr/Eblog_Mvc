namespace Eblog.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("blog.kategori")]
    public partial class kategori
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kategori()
        {
            makale = new HashSet<makale>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string KategoriAdi { get; set; }

        [StringLength(20)]
        public string Tarih { get; set; }

        [Column(TypeName = "bit")]
        public bool Onay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<makale> makale { get; set; }
    }
}
