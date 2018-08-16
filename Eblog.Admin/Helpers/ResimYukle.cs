using Eblog.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eblog.Admin.Helpers
{
    public class ResimYukle
    {
        public static string kullaniciResim(HttpPostedFileBase Resim, kullanici kullanici)
        {
            string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
            string[] uzanti = Resim.ContentType.Split('/');
            string TamYolYeri = "/External/ProfilImage/" + DosyaAdi + "." + uzanti[1];

            Resim.SaveAs(System.Web.HttpContext.Current.Server.MapPath(TamYolYeri));
            kullanici.Foto = TamYolYeri;
            return kullanici.Foto;
        }

        public static string makaleResim(HttpPostedFileBase vitrinResmi, makale makale)
        {
            string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
            string[] uzanti = vitrinResmi.ContentType.Split('/');
            string TamYolYeri = "/External/Makale/" + DosyaAdi + "." + uzanti[1];

            vitrinResmi.SaveAs(System.Web.HttpContext.Current.Server.MapPath(TamYolYeri));
            makale.Foto = TamYolYeri;
            return makale.Foto;
        }
    }
}