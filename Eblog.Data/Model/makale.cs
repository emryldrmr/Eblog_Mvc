namespace Eblog.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("blog.makale")]
    public partial class makale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public makale()
        {
            yorum = new HashSet<yorum>();
            etiket = new HashSet<etiket>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Baslik { get; set; }

        [StringLength(8000)]
        public string Icerik { get; set; }

        [StringLength(250)]
        public string Foto { get; set; }

        [StringLength(20)]
        public string Tarih { get; set; }

        public int KategoriID { get; set; }

        public int KullaniciID { get; set; }

        public int? Okunma { get; set; }

        [Column(TypeName = "bit")]
        public bool Onay { get; set; }

        public virtual kategori kategori { get; set; }

        public virtual kullanici kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<yorum> yorum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<etiket> etiket { get; set; }
    }
}
