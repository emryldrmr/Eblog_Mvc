using Eblog.Admin.Class;
using Eblog.Admin.CustomFilter;
using Eblog.Admin.Helpers;
using Eblog.Core.Infrastructure;
using Eblog.Data.Model;
using Microsoft.Web.Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Eblog.Admin.Controllers
{
    public class HomeController : Controller
    {
        #region Veritabanı
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IYetkiRepository _yetkiRepository;
        private readonly IMakaleRepository _makaleRepository;
        private readonly IBizeUlasinRepository _bizeulasinRepository;
        private readonly IYorumRepository _yorumRepository;
        private readonly IEtiketRepository _etiketRepository;

        public HomeController(IKategoriRepository kategoriRepository, IKullaniciRepository kullaniciRepository, IYetkiRepository yetkiRepository, IMakaleRepository makaleRepository, IBizeUlasinRepository bizeulasinRepository, IYorumRepository yorumRepository, IEtiketRepository etiketRepository)
        {
            _kategoriRepository = kategoriRepository;
            _kullaniciRepository = kullaniciRepository;
            _yetkiRepository = yetkiRepository;
            _makaleRepository = makaleRepository;
            _bizeulasinRepository = bizeulasinRepository;
            _yorumRepository = yorumRepository;
            _etiketRepository = etiketRepository;
        }
        #endregion

        public ActionResult Index(int Sayfa = 1)
        {
            var MakaleListesi = _makaleRepository.GetMany(x => x.Onay==true);
            return View(MakaleListesi.OrderByDescending(x => x.Tarih).ToPagedList(Sayfa, 10));
        }

        

        #region Makalelerim
        [LoginFilter]
        public ActionResult Makalelerim(int? id, int Sayfa = 1)
        {
            var sessionControl = HttpContext.Session["ID"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IPagedList<makale> makales = _makaleRepository.GetMany(x => x.KullaniciID == Convert.ToInt32(sessionControl) && x.Onay == true).OrderByDescending(x => x.Tarih).ToPagedList(Sayfa, 10);
            return View("Index", makales);
        }
        #endregion


        #region Giriş
        [HttpGet]
        public ActionResult Login()
        {
            SetYetkiListele();
            return View();
        }

        [HttpPost]
        public ActionResult Login(kullanici kullanici_)
        {
            var KullaniciVarmi = _kullaniciRepository.GetMany(x => x.Email == kullanici_.Email && x.Sifre == kullanici_.Sifre && x.Onay == true).SingleOrDefault();
            if (KullaniciVarmi != null)
            {
                if (KullaniciVarmi.yetki.YetkiAdi == "Admin" || KullaniciVarmi.yetki.YetkiAdi == "Editor" || KullaniciVarmi.yetki.YetkiAdi == "Uye")
                {
                    Session["Rol"] = KullaniciVarmi.YetkiID;
                    Session["ID"] = KullaniciVarmi.ID;
                    Session["KullaniciEmail"] = KullaniciVarmi.Email;
                    Session["KullaniciAdi"] = KullaniciVarmi.KullaniciAdi;
                    Session["AdSoyad"] = KullaniciVarmi.AdSoyad;
                    Session["Foto"] = KullaniciVarmi.Foto;
                    Session["YetkiAdi"] = KullaniciVarmi.yetki.YetkiAdi;
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Mesaj = "Yetkisiz Kullanıcı";
                return View();
            }
            ViewBag.Mesaj = "Kullanıcı Bulunamadı.";
            return View();
        }
        #endregion

        #region Kullanıcı Kayıt
        [HttpGet]
        public ActionResult Kayit()
        {
            SetYetkiListele();
            return View();
        }

        [HttpPost]
        public JsonResult Kayit(kullanici kullanici_, int? YetkiID)
        {

            if (kullanici_ != null)
            {
                var KullaniciVarmi = _kullaniciRepository.KullaniciBul(kullanici_.Email);
                if (KullaniciVarmi != null)
                {
                    return Json(new ResultJson { Success = false, Message = kullanici_.Email + " Daha önce Kayıt Edilmiş" });
                }

                kullanici_.Tarih = DateTime.Now.ToLocalTime().ToString();
                kullanici_.YetkiID = 3;
                kullanici_.Onay = true;
                _kullaniciRepository.Insert(kullanici_);
                try
                {
                    _kullaniciRepository.Save();
                    return Json(new ResultJson { Success = true, Message = "Başarılı bir şekilde kayıt oldunuz. Hadi ilk makaleyi yazalım." });
                }
                catch (Exception ex)
                {
                    return Json(new ResultJson { Success = false, Message = "Kayıt olurken Hata oluştu !" });
                }
            }
            return Json(new ResultJson { Success = false, Message = "Kayıt olurken Hata oluştu !" });
        }

        #endregion

        #region Profili Göster
        [LoginFilter]
        public ActionResult ProfilGoster(kullanici kullanici_, int id)
        {
            var kul_id = Session["ID"].ToString();
            kullanici gelenKullanici = _kullaniciRepository.GetById(kullanici_.ID);
            
            
            if (gelenKullanici == null || gelenKullanici.ID != Convert.ToInt32(kul_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //return Json(new ResultJson { Success = false, Message = "Yanlış işlem yürütüyorsunuz!" });
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
        [LoginFilter]
        public ActionResult ProfilDuzenle(int id)
        {
            kullanici gelenKullanici = _kullaniciRepository.GetById(id);

            if (gelenKullanici == null)
            {
                //throw new Exception("Kullanıcı Bulunamadı !");
                return Json(new ResultJson { Success = true, Message = "Kullanıcı Bulunamadı !" });
                }
            else
            {
                return View(gelenKullanici);
            }
        }

        [HttpPost]
        [LoginFilter]
        public ActionResult ProfilDuzenle(kullanici kullanici_, HttpPostedFileBase Resim)
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
            gelenKullanici.Sifre = kullanici_.Sifre;

            if (Resim != null && Resim.ContentLength > 0)
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

        #region Yetki Listesi
        public void SetYetkiListele(object rol = null)
        {
            var YetkiList = _yetkiRepository.GetMany(x => x.ID == 3).ToList();
            ViewBag.Yetki = YetkiList;
        }
        #endregion

        #region Çıkış
        public ActionResult Cikis()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Set Kategori
        public PartialViewResult setKategori()
        {
            List<kategori> model = _kategoriRepository.GetMany(x => x.Onay == true).ToList();
            return PartialView(model);
        }
        #endregion

        #region Set Kategori Haber
        public PartialViewResult setKategoriHaber()
        {
            List<makale> model = _makaleRepository.GetMany(x => x.Onay == true && x.kategori.KategoriAdi == "Haber").Take(5).ToList();
            return PartialView(model);
        }
        #endregion

        #region Set Kategori Haber
        public PartialViewResult setKullanici()
        {
            List<kullanici> model = _kullaniciRepository.GetMany(x => x.Onay == true).Take(5).OrderBy(x => x.Tarih).ToList();
            return PartialView(model);
        }
        #endregion

        #region Set Makale Okunma
        public PartialViewResult setMakaleOkunma()
        {
            List<makale> modeloku = _makaleRepository.GetMany(x => x.Onay == true).OrderByDescending(o => o.Okunma).Take(5).ToList();
            return PartialView(modeloku);
        }
        #endregion

        #region Yorum Yap
        [LoginFilter]
        public JsonResult YorumYap(yorum yorum_, string yorum, int MakaleID)
        {
            var sessionControl = HttpContext.Session["ID"];

            if (yorum == null)
            {
                return Json(new ResultJson { Success = true, Message = "Yorum Eklerken Hata Oluştu !" }, JsonRequestBehavior.AllowGet);
            }

            var ipAddress = string.Empty;
            if (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ipAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (HttpContext.Request.ServerVariables["HTTP_CLIENT_IP"] != null && HttpContext.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
                ipAddress = HttpContext.Request.ServerVariables["HTTP_CLIENT_IP"];
            else if (HttpContext.Request.UserHostAddress.Length != 0)
                ipAddress = HttpContext.Request.UserHostName;

            

            _yorumRepository.Insert(new yorum
            {
                KullaniciID = Convert.ToInt32(sessionControl),
                MakaleID = MakaleID,
                Icerik = yorum,
                Tarih = DateTime.Now.ToLocalTime().ToString(),
                Onay = false,
                Ip=ipAddress
            });
            _yorumRepository.Save();
            return Json(new ResultJson { Success = false, Message = "Yorum Başarıyla Eklenmiştir. Lütfen Editör Onayını Bekleyiniz !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Yorum Sil
        public ActionResult YorumSil(int id)
        {
            var sessionControl = HttpContext.Session["ID"];
            var yorum = _yorumRepository.GetMany(y => y.ID == id).SingleOrDefault();
            var makale = _makaleRepository.GetMany(m => m.ID == yorum.MakaleID).SingleOrDefault();
            if (yorum.KullaniciID == Convert.ToInt32(sessionControl))
            {
                _yorumRepository.Delete(id);
                _yorumRepository.Save();
                return RedirectToAction("Detay", "Makale", new { id = makale.ID });
            }
            else
            {
                return HttpNotFound();
            }
        }
        #endregion

        #region Hakkımızda
        public ActionResult Hakkimizda()
        {
            return View();
        }
        #endregion

        #region Bize Ulaşın
        [HttpGet]
        public ActionResult BizeUlasin()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult BizeUlasin(bizeulasin bizeulasin)
        {
            bizeulasin.Tarih = DateTime.Now.ToLocalTime().ToString();

            var ipAddress = string.Empty;
            if (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ipAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (HttpContext.Request.ServerVariables["HTTP_CLIENT_IP"] != null && HttpContext.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
                ipAddress = HttpContext.Request.ServerVariables["HTTP_CLIENT_IP"];
            else if (HttpContext.Request.UserHostAddress.Length != 0)
                ipAddress = HttpContext.Request.UserHostName;

            bizeulasin.Ip = ipAddress;

            try
            {
                _bizeulasinRepository.Insert(bizeulasin);
                _bizeulasinRepository.Save();
                return Json(new ResultJson { Success = true, Message = "Mesajınız alınmış olup en yakın sürede dönüş yapılacaktır. İlginiz için Teşekkür Ederiz." });
            }
            catch (Exception ex)
            {
                //Loglama yaptırabiliriz
                return Json(new ResultJson { Success = false, Message = "Mesaj Gönderirken Hata Oluştu" });

            }
        }
        #endregion

        #region Okunma Sayısı
        public ActionResult Okunma(int MakaleID)
        {
            var makale = _makaleRepository.GetMany(x => x.ID == MakaleID).SingleOrDefault();
            makale.Okunma += 1;
            _makaleRepository.Save();
            return View();
        }
        #endregion

        #region Ara
        public ActionResult Ara(string Ara = null, int Sayfa = 1)
        {
            var aranan = _makaleRepository.GetMany(x => x.Icerik.Contains(Ara));
            return View(aranan.OrderByDescending(m => m.Tarih).ToPagedList(Sayfa, 10));
        }
        #endregion
    }
}