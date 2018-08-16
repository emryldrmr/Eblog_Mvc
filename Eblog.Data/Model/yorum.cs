namespace Eblog.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("blog.yorum")]
    public partial class yorum
    {
        public int ID { get; set; }

        [StringLength(2000)]
        public string Icerik { get; set; }

        public int KullaniciID { get; set; }

        public int MakaleID { get; set; }

        [StringLength(20)]
        public string Tarih { get; set; }

        [Column(TypeName = "bit")]
        public bool Onay { get; set; }

        [StringLength(50)]
        public string Ip { get; set; }

        public virtual kullanici kullanici { get; set; }

        public virtual makale makale { get; set; }
    }
}
