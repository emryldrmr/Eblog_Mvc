using Eblog.Admin.Class;
using Eblog.Admin.CustomFilter;
using Eblog.Admin.Helpers;
using Eblog.Core.Infrastructure;
using Eblog.Data.Model;
using Eblog.Data.ValueObjects;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Eblog.Admin.Controllers
{
    public class MakaleController : Controller
    {

        #region Veritabanı
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IMakaleRepository _makaleRepository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IEtiketRepository _etiketRepository;

        public MakaleController(IKategoriRepository kategoriRepository, IMakaleRepository makaleRepository, IKullaniciRepository kullaniciRepository, IEtiketRepository etiketRepository)
        {
            _kategoriRepository = kategoriRepository;
            _makaleRepository = makaleRepository;
            _kullaniciRepository = kullaniciRepository;
            _etiketRepository = etiketRepository;
        }
        #endregion

        #region Makale Listesi
        [LoginEditor]
        public ActionResult Index(int Sayfa = 1)
        {
            return View(_makaleRepository.GetAll().OrderByDescending(x => x.ID).ToPagedList(Sayfa, 10));
        }
        #endregion

        #region Makale Ekle
        [HttpGet]
        [LoginUye]
        public ActionResult Ekle()
        {
            SetKategoriListele();
            return View();
        }

        [HttpPost]
        [LoginUye]
        [ValidateInput(false)]
        public JsonResult Ekle(makale makale_, int KategoriID, HttpPostedFileBase vitrinResmi, IEnumerable<HttpPostedFileBase> DetayResim, string Etiket)
        {
            var sessionControl = HttpContext.Session["ID"];
            if (makale_ != null)
            {
                var kullanici = _kullaniciRepository.GetById(Int32.Parse(sessionControl.ToString()));
                makale_.KullaniciID = kullanici.ID;
                makale_.KategoriID = KategoriID;
                makale_.Onay = false;
                makale_.Tarih = DateTime.Now.ToLocalTime().ToString();

                if (vitrinResmi != null)
                {
                    if(vitrinResmi.ContentLength > 2048000)
                    {
                        return Json(new ResultJson { Success = false, Message = "Dosya boyutu 2 MB'yi geçmemelidir." });
                    }
                    else if (vitrinResmi.ContentLength > 0 && vitrinResmi.ContentLength <= 2048000)
                    {
                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYol = "/External/Makale/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYol));
                        makale_.Foto = tamYol;
                    }
                }
                _makaleRepository.Insert(makale_);

                
            }
            try
            {
                _makaleRepository.Save();
                _etiketRepository.EtiketEkle(makale_.ID, Etiket);
                return Json(new ResultJson { Success = true, Message = "Makale Ekleme İşleminiz Başarılı. Editör Onayından Sonra Makaleniz Yayınlanacaktır. Teşekkür Ederiz." });
            }
            catch (Exception ex)
            {
                //Loglama yaptırabiliriz
                return Json(new ResultJson { Success = false, Message = "Makale Eklerken Hata Oluştu !" });

            }
        }
        #endregion

        #region Makale Düzenle
        [HttpGet]
        [LoginUye]
        public ActionResult Duzenle(int id)
        {
            makale gelenMakale = _makaleRepository.GetById(id);
            var sessionControl = HttpContext.Session["ID"];
            
            //#region Etiket
            //var gelenEtiket = gelenMakale.Etiket.Select(x => x.EtiketAdi).ToArray();
            //MakaleEtiketModel model = new MakaleEtiketModel
            //{
            //    Makale = gelenMakale,
            //    Etiketler = _etiketRepository.GetAll(),
            //    GelenEtikler = gelenEtiket
            //};
            //StringBuilder birlestir = new StringBuilder();
            //foreach (var etiket in model.GelenEtikler)
            //{
            //    birlestir.Append(etiket.ToString());
            //    birlestir.Append(",");
            //}
            //model.EtiketAd = birlestir.ToString();
            //#endregion    
            if(Convert.ToInt32(sessionControl) == 1 || Convert.ToInt32(sessionControl) == 2)
            {
                if (gelenMakale == null)
                {
                    return Json(new ResultJson { Success = false, Message = "Makale Bulunamadı." });
                }
                else
                {
                    SetKategoriListele();
                    return View(gelenMakale);
                }
            }
            else
            {
                if (sessionControl == null || Convert.ToInt32(sessionControl) != gelenMakale.KullaniciID)
                {
                    return Json(new ResultJson { Success = false, Message = "Bu Makaleyi Düzenlemeye Yetkiniz Yok !" }, JsonRequestBehavior.AllowGet);
                }
            }

            if (gelenMakale == null)
            {
                return Json(new ResultJson { Success = false, Message = "Makale Bulunamadı." });
            }
            else
            {
                SetKategoriListele();
                return View(gelenMakale);
            }

        }

        [HttpPost]
        [LoginUye]
        [ValidateInput(false)]
        public JsonResult Duzenle(makale makale_, int KategoriID, HttpPostedFileBase vitrinResmi, string EtiketAd)
        {
            makale gelenMakale = _makaleRepository.GetById(makale_.ID);

            gelenMakale.Baslik = makale_.Baslik;
            gelenMakale.Icerik = makale_.Icerik;
            gelenMakale.Onay = makale_.Onay;
            gelenMakale.KategoriID = Convert.ToInt32(KategoriID);

            if (vitrinResmi != null && vitrinResmi.ContentLength > 0)
            {
                if (vitrinResmi.ContentLength > 2048000)
                {
                    return Json(new ResultJson { Success = false, Message = "Dosya boyutu 2 MB'yi geçmemelidir." });
                }
                else if (gelenMakale.Foto != null && vitrinResmi.ContentLength <= 2048000)
                {
                    string url = gelenMakale.Foto;
                    string resimPath = Server.MapPath(url);
                    FileInfo files = new FileInfo(resimPath);
                    if (files.Exists)
                    {
                        files.Delete();
                    }
                }
                ResimYukle.makaleResim(vitrinResmi, makale_);
                gelenMakale.Foto = makale_.Foto;
            }

            _etiketRepository.EtiketEkle(makale_.ID, EtiketAd);

            try
            {
                _makaleRepository.Save();
                return Json(new ResultJson { Success = true, Message = "Başarılı Bir Şekilde Güncellendi" });
            }
            catch (Exception ex)
            {
                return Json(new ResultJson { Success = false, Message = "Güncelleme İşlemi Başarısız" });
            }
            
        }
        #endregion

        #region Makale Detay

        public ActionResult Detay(int id)
        {
            makale gelenMakale = _makaleRepository.GetById(id);

            if (gelenMakale == null)
            {
                //throw new Exception("Kullanıcı Bulunamadı !");
                return Json(new ResultJson { Success = false, Message = "Kullanıcı Bulunamadı." });
            }
            else
            {
                SetKategoriListele();
                return View(gelenMakale);
            }
        }
        #endregion

        #region Makale Sil
        [LoginEditor]
        public JsonResult Sil(makale makale_)
        {
            makale dbMakale = _makaleRepository.GetById(makale_.ID);
            if (dbMakale == null)
            {
                return Json(new ResultJson { Success = false, Message = "Makale Bulunamadı !" });
            }

            try
            {
                if (dbMakale != null)
                {
                    string Resim = dbMakale.Foto;
                    string resimPath = Server.MapPath(Resim);
                    FileInfo files = new FileInfo(resimPath);
                    if (files.Exists)
                    {
                        files.Delete();
                    }
                }
                _makaleRepository.Delete(makale_.ID);
                _makaleRepository.Save();
                return Json(new ResultJson { Success = true, Message = "Makale Silme işlemi Başarılı" });
            }
            catch (Exception)
            {
                return Json(new ResultJson { Success = false, Message = "Makale Silme İşlemi Sırasında Bir Hata Oluştu !" });
            }
        }
        #endregion

        #region Aktif / Pasif Yapar
        [LoginEditor]
        public ActionResult Onay(int id)
        {
            makale gelenMakale = _makaleRepository.GetById(id);
            if (gelenMakale.Onay == true)
            {
                gelenMakale.Onay = false;
                _makaleRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı";
                return RedirectToAction("Index", "Makale");
            }
            else if (gelenMakale.Onay == false)
            {
                gelenMakale.Onay = true;
                _makaleRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı";
                return RedirectToAction("Index", "Makale");
            }
            return View();
        }
        #endregion

        #region Set Makale
        public void SetMakaleListele(object makale = null)
        {
            var MakaleList = _makaleRepository.GetAll().ToList();
            ViewBag.Makale = MakaleList;

        }
        #endregion

        #region Set Kategori
        public void SetKategoriListele(object kategori = null)
        {
            var KategoriList = _kategoriRepository.GetMany(x => x.Onay == true).ToList();
            ViewBag.Kategori = KategoriList;

        }
        #endregion
    }
}