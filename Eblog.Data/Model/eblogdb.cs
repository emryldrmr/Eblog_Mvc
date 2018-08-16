namespace Eblog.Data.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class eblogdb : DbContext
    {
        public eblogdb()
            : base("name=eblogdb")
        {
        }

        public virtual DbSet<bizeulasin> bizeulasin { get; set; }
        public virtual DbSet<etiket> etiket { get; set; }
        public virtual DbSet<kategori> kategori { get; set; }
        public virtual DbSet<kullanici> kullanici { get; set; }
        public virtual DbSet<makale> makale { get; set; }
        public virtual DbSet<yetki> yetki { get; set; }
        public virtual DbSet<yorum> yorum { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<bizeulasin>()
                .Property(e => e.Adisoyadi)
                .IsUnicode(false);

            modelBuilder.Entity<bizeulasin>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<bizeulasin>()
                .Property(e => e.Mesaj)
                .IsUnicode(false);

            modelBuilder.Entity<bizeulasin>()
                .Property(e => e.Tarih)
                .IsUnicode(false);

            modelBuilder.Entity<bizeulasin>()
                .Property(e => e.Ip)
                .IsUnicode(false);

            modelBuilder.Entity<etiket>()
                .Property(e => e.EtiketAdi)
                .IsUnicode(false);

            modelBuilder.Entity<etiket>()
                .HasMany(e => e.makale)
                .WithMany(e => e.etiket)
                .Map(m => m.ToTable("makaleetiket", "blog").MapLeftKey("EtiketID").MapRightKey("MakaleID"));

            modelBuilder.Entity<kategori>()
                .Property(e => e.KategoriAdi)
                .IsUnicode(false);

            modelBuilder.Entity<kategori>()
                .Property(e => e.Tarih)
                .IsUnicode(false);

            modelBuilder.Entity<kategori>()
                .HasMany(e => e.makale)
                .WithRequired(e => e.kategori)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<kullanici>()
                .Property(e => e.KullaniciAdi)
                .IsUnicode(false);

            modelBuilder.Entity<kullanici>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<kullanici>()
                .Property(e => e.Sifre)
                .IsUnicode(false);

            modelBuilder.Entity<kullanici>()
                .Property(e => e.AdSoyad)
                .IsUnicode(false);

            modelBuilder.Entity<kullanici>()
                .Property(e => e.Foto)
                .IsUnicode(false);

            modelBuilder.Entity<kullanici>()
                .Property(e => e.Tarih)
                .IsUnicode(false);

            modelBuilder.Entity<kullanici>()
                .HasMany(e => e.makale)
                .WithRequired(e => e.kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<kullanici>()
                .HasMany(e => e.yorum)
                .WithRequired(e => e.kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<makale>()
                .Property(e => e.Baslik)
                .IsUnicode(false);

            modelBuilder.Entity<makale>()
                .Property(e => e.Icerik)
                .IsUnicode(false);

            modelBuilder.Entity<makale>()
                .Property(e => e.Foto)
                .IsUnicode(false);

            modelBuilder.Entity<makale>()
                .Property(e => e.Tarih)
                .IsUnicode(false);

            modelBuilder.Entity<makale>()
                .HasMany(e => e.yorum)
                .WithRequired(e => e.makale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<yetki>()
                .Property(e => e.YetkiAdi)
                .IsUnicode(false);

            modelBuilder.Entity<yetki>()
                .Property(e => e.Tarih)
                .IsUnicode(false);

            modelBuilder.Entity<yetki>()
                .HasMany(e => e.kullanici)
                .WithRequired(e => e.yetki)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<yorum>()
                .Property(e => e.Icerik)
                .IsUnicode(false);

            modelBuilder.Entity<yorum>()
                .Property(e => e.Tarih)
                .IsUnicode(false);

            modelBuilder.Entity<yorum>()
                .Property(e => e.Ip)
                .IsUnicode(false);
        }
    }
}
