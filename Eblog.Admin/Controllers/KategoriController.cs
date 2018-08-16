using Eblog.Admin.Class;
using Eblog.Admin.CustomFilter;
using Eblog.Core.Infrastructure;
using Eblog.Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eblog.Admin.Controllers
{
    public class KategoriController : Controller
    {

        #region Veritabanı
        private readonly IKategoriRepository _kategoriRepository;

        public KategoriController(IKategoriRepository kategoriRepository)
        {
            _kategoriRepository = kategoriRepository;
        }
        #endregion

        #region Kategori Listele
        [HttpGet]
        [LoginEditor]
        public ActionResult Index(int Sayfa = 1)
        {
            return View(_kategoriRepository.GetAll().OrderByDescending(x => x.ID).ToPagedList(Sayfa, 10));
        }
        #endregion

        #region Kategori Ekle
        [HttpGet]
        [LoginEditor]
        public ActionResult Ekle()
        {
            SetKategoriListele();
            return View();
        }

        [HttpPost]
        [LoginEditor]
        public JsonResult Ekle(kategori kategori_)
        {
            kategori_.Tarih = DateTime.Now.ToLocalTime().ToString();
            try
            {
                _kategoriRepository.Insert(kategori_);
                _kategoriRepository.Save();
                return Json(new ResultJson { Success = true, Message = "Kategori Ekleme İşleminiz Başarılı" });
            }
            catch (Exception ex)
            {
                //Loglama yaptırabiliriz
                return Json(new ResultJson { Success = false, Message = "Kategori Eklerken Hata Oluştu" });

            }
        }
        #endregion
        
        #region Kategori Düzenle
        [HttpGet]
        [LoginEditor]
        public ActionResult Duzenle(int id)
        {
            kategori dbKategori = _kategoriRepository.GetById(id);
            if (dbKategori == null)
            {
                throw new Exception("Kategori Bulunamadı");
            }
            SetKategoriListele();

            return View(dbKategori);
        }

        [HttpPost]
        [LoginEditor]
        public JsonResult Duzenle(kategori kategori_)
        {
            kategori dbKategori = _kategoriRepository.GetById(kategori_.ID);
            dbKategori.KategoriAdi = kategori_.KategoriAdi;
            dbKategori.Onay = kategori_.Onay;

            _kategoriRepository.Save();

            return Json(new ResultJson { Success = true, Message = "Düzenleme İşlemi Başarılı" });

            //return Json(new ResultJson { Success = false, Message = "Düzenleme İşlemi Sırasında Bir Hata Oluştu." });

        }
        #endregion

        #region Kategori Sil
        [LoginEditor]
        public JsonResult Sil(kategori kategori_)
        {
            kategori dbKategori = _kategoriRepository.GetById(kategori_.ID);
            if (dbKategori == null)
            {
                return Json(new ResultJson { Success = false, Message = "Kategori Bulunamadı." });
            }

            try
            {
                _kategoriRepository.Delete(kategori_.ID);
                _kategoriRepository.Save();

                return Json(new ResultJson { Success = true, Message = "Kategori Silme İşlemi Başarılı." });
            }
            catch (Exception)
            {
                return Json(new ResultJson { Success = false, Message = "Kategori Silme İşlemi Sırasında Bir Hata Oluştu !" });
            }
        }
        #endregion

        #region Aktif / Pasif Yapar
        [LoginEditor]
        public ActionResult Onay(int id)
        {
            kategori gelenKategori = _kategoriRepository.GetById(id);
            if (gelenKategori.Onay == true)
            {
                gelenKategori.Onay = false;
                _kategoriRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı";
                return RedirectToAction("Index", "Kategori");
            }
            else if (gelenKategori.Onay == false)
            {
                gelenKategori.Onay = true;
                _kategoriRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı";
                return RedirectToAction("Index", "Kategori");
            }
            return View();
        }
        #endregion


        #region Set Kategori
        public void SetKategoriListele(object kategori = null)
        {
            var KategoriList = _kategoriRepository.GetAll().ToList();
            ViewBag.Kategori = KategoriList;

        }
        #endregion

    }
}