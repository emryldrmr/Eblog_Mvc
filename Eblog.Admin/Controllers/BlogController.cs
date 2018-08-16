using Eblog.Core.Infrastructure;
using Eblog.Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Eblog.Admin.Controllers
{
    public class BlogController : Controller
    {
        #region Veritabanı
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IYetkiRepository _yetkiRepository;
        private readonly IMakaleRepository _makaleRepository;
        private readonly IBizeUlasinRepository _bizeulasinRepository;
        private readonly IYorumRepository _yorumRepository;

        public BlogController(IKategoriRepository kategoriRepository, IKullaniciRepository kullaniciRepository, IYetkiRepository yetkiRepository, IMakaleRepository makaleRepository, IBizeUlasinRepository bizeulasinRepository, IYorumRepository yorumRepository)
        {
            _kategoriRepository = kategoriRepository;
            _kullaniciRepository = kullaniciRepository;
            _yetkiRepository = yetkiRepository;
            _makaleRepository = makaleRepository;
            _bizeulasinRepository = bizeulasinRepository;
            _yorumRepository = yorumRepository;
        }
        #endregion

        public ActionResult Index(int Sayfa = 1)
        {
            var MakaleListesi = _makaleRepository.GetMany(x => x.Onay == true);
            return View(MakaleListesi.OrderByDescending(x => x.Tarih).ToPagedList(Sayfa, 10));
        }

        #region ByKategori
        public ActionResult ByKategori(int? id, int Sayfa = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IPagedList<makale> makales = _makaleRepository.GetMany(x => x.KategoriID == id && x.Onay == true).OrderByDescending(x => x.Tarih).ToPagedList(Sayfa, 10);
            return View("Index", makales);

            //var MakaleListesi = _makaleRepository.GetMany(x => x.KategoriID == id && x.Onay == true);
            //return View(MakaleListesi.OrderByDescending(x => x.ID).ToPagedList(Sayfa, 5));
        }
        #endregion
    }
}