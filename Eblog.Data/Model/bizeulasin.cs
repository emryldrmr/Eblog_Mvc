namespace Eblog.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("blog.bizeulasin")]
    public partial class bizeulasin
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string Adisoyadi { get; set; }

        [StringLength(70)]
        public string Email { get; set; }

        [StringLength(500)]
        public string Mesaj { get; set; }

        [StringLength(20)]
        public string Tarih { get; set; }

        [StringLength(50)]
        public string Ip { get; set; }
    }
}
