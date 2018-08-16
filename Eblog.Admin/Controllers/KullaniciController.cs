using Eblog.Admin.Class;
using Eblog.Admin.CustomFilter;
using Eblog.Admin.Helpers;
using Eblog.Core.Infrastructure;
using Eblog.Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eblog.Admin.Controllers
{
    
    public class KullaniciController : Controller
    {
        #region Kullanıcı Veritabanı
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IYetkiRepository _yetkiRepository;

        public KullaniciController(IKullaniciRepository kullaniciRepository, IYetkiRepository yetkiRepository)
        {
            _kullaniciRepository = kullaniciRepository;
            _yetkiRepository = yetkiRepository;
        }
        #endregion

        #region Kullanici Listesi
        [HttpGet]
        [LoginEditor]
        public ActionResult Index(int Sayfa = 1)
        {
            var KullaniciListe = _kullaniciRepository.GetAll();
            return View(KullaniciListe.OrderByDescending(x => x.ID).ToPagedList(Sayfa, 10));
        }
        #endregion

        #region Kullanıcı Kayıt
        [HttpGet]
        [LoginEditor]
        public ActionResult Kayit()
        {
            SetYetkiListele();
            return View();
        }

        [HttpPost]
        [LoginEditor]
        public JsonResult Kayit(kullanici kullanici_, int? YetkiID, HttpPostedFileBase Resim)
        {
            if (ModelState.IsValid)
            {
                var KullaniciVarmi = _kullaniciRepository.KullaniciBul(kullanici_.Email);
                if (KullaniciVarmi != null)
                {
                    return Json(new ResultJson { Success = false, Message = kullanici_.Email + " Daha önce Kayıt Edilmiş" });
                }
                if (kullanici_.Foto == null)
                {
                    if (Resim.ContentLength > 2048000)
                    {
                        return Json(new ResultJson { Success = false, Message = "Dosya boyutu 2 MB'yi geçmemelidir." });
                        
                    }
                    else if (Resim.ContentLength > 0 && Resim.ContentLength <= 2048000)
                    {
                        string Dosya = Guid.NewGuid().ToString().Replace("-", "");
                        string Uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string ResimYolu = "/External/ProfilImage/" + Dosya + Uzanti;

                        Resim.SaveAs(Server.MapPath(ResimYolu));
                        kullanici_.Foto = ResimYolu;
                    }
                }

                kullanici_.Tarih = DateTime.Now.ToLocalTime().ToString();
                kullanici_.YetkiID = Convert.ToInt32(YetkiID);
                _kullaniciRepository.Insert(kullanici_);
                try
                {
                    _kullaniciRepository.Save();
                    return Json(new ResultJson { Success = true, Message = "Kullanıcı Ekleme İşleminiz Başarılı" });
                }
                catch (Exception ex)
                {
                    return Json(new ResultJson { Success = false, Message = "Kullanıcı Eklerken Hata Oluştu !" });
                }
            }
            return Json(new ResultJson { Success = false, Message = "Kullanıcı Eklerken Hata Oluştu !" });
        }

        #endregion

        #region Kullanıcı Detay
        [LoginEditor]
        public ActionResult Detay(int id)
        {
            kullanici gelenKullanici = _kullaniciRepository.GetById(id);

            if (gelenKullanici == null)
            {
                //throw new Exception("Kullanıcı Bulunamadı !");
                return Json(new ResultJson { Success = false, Message = "Kullanıcı Bulunamadı!" });
            }
            else
            {
                SetYetkiListele();
                return View(gelenKullanici);
            }
        }
        #endregion

        #region Kullanıcı Düzenle
        [HttpGet]
        [LoginEditor]
        public ActionResult Duzenle(int id)
        {
            kullanici gelenKullanici = _kullaniciRepository.GetById(id);

            if (gelenKullanici == null)
            {
                //throw new Exception("Kullanıcı Bulunamadı !");
                return Json(new ResultJson { Success = false, Message = "Kullanıcı Bulunamadı!" });
            }
            else
            {
                SetYetkiListele();
                return View(gelenKullanici);
            }
        }

        [HttpPost]
        [LoginEditor]
        public JsonResult Duzenle(kullanici kullanici_, int? YetkiID, HttpPostedFileBase Resim)
        {
            kullanici gelenKullanici = _kullaniciRepository.GetById(kullanici_.ID);

            var EmailVarmi = _kullaniciRepository.KullaniciBul(kullanici_.Email);
            if (EmailVarmi != null && gelenKullanici.Email != kullanici_.Email)
            {
                return Json(new ResultJson { Success = false, Message = "Bu Mail Adresi Sistemde Kayıtlı" });
            }

            gelenKullanici.KullaniciAdi = kullanici_.KullaniciAdi;
            gelenKullanici.AdSoyad = kullanici_.AdSoyad;
            gelenKullanici.Email = kullanici_.Email;
            gelenKullanici.Onay = kullanici_.Onay;
            gelenKullanici.YetkiID = Convert.ToInt32(YetkiID);
            gelenKullanici.Sifre = kullanici_.Sifre;

            if (Resim != null && Resim.ContentLength > 0 && Resim.ContentLength <= 2048000)
            {
                if (gelenKullanici.Foto != null)
                {
                    string url = gelenKullanici.Foto;
                    string resimPath = Server.MapPath(url);
                    FileInfo files = new FileInfo(resimPath);
                    if (files.Exists)
                    {
                        files.Delete();
                    }
                }
                ResimYukle.kullaniciResim(Resim, kullanici_);
                gelenKullanici.Foto = kullanici_.Foto;
            }
            try
            {
                _kullaniciRepository.Save();
                return Json(new ResultJson { Success = true, Message = "Başarılı Bir Şekilde Güncellendi" });
            }
            catch (Exception ex)
            {
                return Json(new ResultJson { Success = false, Message = "Güncelleme İşlemi Başarısız" });
            }
            
        }
        #endregion

        #region Kullanıcı Sil
        [LoginEditor]
        public JsonResult Sil(kullanici kullanici_)
        {
            kullanici dbKullanici = _kullaniciRepository.GetById(kullanici_.ID);
            if (dbKullanici == null)
            {
                return Json(new ResultJson { Success = false, Message = "Kullanıcı Bulunamadı !" });
            }

            try
            {
                if (dbKullanici != null)
                {
                    string Resim = dbKullanici.Foto;
                    string resimPath = Server.MapPath(Resim);
                    FileInfo files = new FileInfo(resimPath);
                    if (files.Exists)
                    {
                        files.Delete();
                    }
                }
                _kullaniciRepository.Delete(kullanici_.ID);
                _kullaniciRepository.Save();
                return Json(new ResultJson { Success = true, Message = "Kullanıcı Silme işlemi Başarılı" });
            }
            catch (Exception)
            {
                return Json(new ResultJson { Success = false, Message = "Kullanıcı Silme İşlemi Sırasında Bir Hata Oluştu !" });
            }
        }
        #endregion

        #region Aktif / Pasif Yapar
        [LoginEditor]
        public ActionResult Onay(int id)
        {
            kullanici gelenKullanici = _kullaniciRepository.GetById(id);
            if (gelenKullanici.Onay == true)
            {
                gelenKullanici.Onay = false;
                _kullaniciRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı";
                return RedirectToAction("Index", "Kullanici");
            }
            else if (gelenKullanici.Onay == false)
            {
                gelenKullanici.Onay = true;
                _kullaniciRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı";
                return RedirectToAction("Index", "Kullanici");
            }
            return View();
        }
        #endregion

        #region Yetki Listesi
        public void SetYetkiListele(object rol = null)
        {
            var YetkiList = _yetkiRepository.GetAll().ToList();
            ViewBag.Yetki = YetkiList;
        }
        #endregion

    }
}