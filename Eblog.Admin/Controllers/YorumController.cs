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
    public class YorumController : Controller
    {
        #region Veritabanı
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IMakaleRepository _makaleRepository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IYorumRepository _yorumRepository;

        public YorumController(IKategoriRepository kategoriRepository, IMakaleRepository makaleRepository, IKullaniciRepository kullaniciRepository, IEtiketRepository etiketRepository, IYorumRepository yorumRepository)
        {
            _kategoriRepository = kategoriRepository;
            _makaleRepository = makaleRepository;
            _kullaniciRepository = kullaniciRepository;
            _yorumRepository = yorumRepository;
        }
        #endregion

        #region Yorum Liste
        [LoginEditor]
        public ActionResult Index(int Sayfa = 1)
        {
            return View(_yorumRepository.GetAll().OrderByDescending(x => x.ID).ToPagedList(Sayfa, 10));
        }
        #endregion

        #region Yorum Sil
        [LoginEditor]
        public JsonResult Sil(yorum yorum_)
        {
            yorum dbYorum = _yorumRepository.GetById(yorum_.ID);
            if (dbYorum == null)
            {
                return Json(new ResultJson { Success = false, Message = "Yorum Bulunamadı." });
            }

            try
            {
                _yorumRepository.Delete(yorum_.ID);
                _yorumRepository.Save();

                return Json(new ResultJson { Success = true, Message = "Yorum Silme İşlemi Başarılı." });
            }
            catch (Exception)
            {
                return Json(new ResultJson { Success = false, Message = "Yorum Silme İşlemi Sırasında Bir Hata Oluştu !" });
            }
        }
        #endregion

        #region Aktif / Pasif Yapar
        [LoginEditor]
        public ActionResult Onay(int id)
        {
            yorum gelenYorum = _yorumRepository.GetById(id);
            if (gelenYorum.Onay == true)
            {
                gelenYorum.Onay = false;
                _yorumRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı";
                return RedirectToAction("Index", "Yorum");
            }
            else if (gelenYorum.Onay == false)
            {
                gelenYorum.Onay = true;
                _yorumRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı";
                return RedirectToAction("Index", "Yorum");
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

        #region Set Kullanıcı
        public void SetKullaniciListele(object kullanici = null)
        {
            var KullaniciList = _kullaniciRepository.GetAll().ToList();
            ViewBag.Kullanici = KullaniciList;

        }
        #endregion
    }
}