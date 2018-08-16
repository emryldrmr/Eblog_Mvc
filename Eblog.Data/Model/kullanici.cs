namespace Eblog.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("blog.kullanici")]
    public partial class kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kullanici()
        {
            makale = new HashSet<makale>();
            yorum = new HashSet<yorum>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string KullaniciAdi { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(16)]
        public string Sifre { get; set; }

        [StringLength(100)]
        public string AdSoyad { get; set; }

        [StringLength(250)]
        public string Foto { get; set; }

        public int YetkiID { get; set; }

        [StringLength(20)]
        public string Tarih { get; set; }

        [Column(TypeName = "bit")]
        public bool Onay { get; set; }

        public virtual yetki yetki { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<makale> makale { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<yorum> yorum { get; set; }
    }
}
