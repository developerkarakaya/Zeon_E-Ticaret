using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZeonTicaret.WebUI.App_Classes;
using ZeonTicaret.WebUI.Models;

namespace ZeonTicaret.WebUI.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Urunler()
        {
            return View(Context.Baglanti.Urun.ToList());
        }

        public ActionResult UrunEkle()
        {
            ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
            ViewBag.Markalar = Context.Baglanti.Marka.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UrunEkle(Urun urn)
        {
            Context.Baglanti.Urun.Add(urn);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");

        }

        public ActionResult UrunOzellikleri()
        {
            return View(Context.Baglanti.OzellikUrun.ToList());
        }


        public ActionResult Markalar()
        {
            return View(Context.Baglanti.Marka.ToList());
        }

        public ActionResult MarkaEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MarkaEkle(Marka mrk, HttpPostedFileBase fileUpload)
        {
            int resimId = -1;
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());
                int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());
                string name = "/Content/MarkaResim/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);

                Bitmap bm = new Bitmap(img, width, height);
                bm.Save(Server.MapPath(name));
                Resim rsm = new Resim();
                rsm.OrtaYol = name;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                if (rsm.id != null)
                {
                    resimId = rsm.id;
                }
            }

            if (resimId != -1)
            {
                mrk.ResimID = resimId;
            }
            Context.Baglanti.Marka.Add(mrk);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }

        public ActionResult Kategoriler()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }

        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategori ktg)
        {
            Context.Baglanti.Kategori.Add(ktg);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");

        }

        public ActionResult OzellikTipleri()
        {
            return View(Context.Baglanti.OzellikTip.ToList());
        }

        public ActionResult OzellikTipEkle()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }

        [HttpPost]
        public ActionResult OzellikTipEkle(OzellikTip ot)
        {
            Context.Baglanti.OzellikTip.Add(ot);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikTipleri");

        }

        public ActionResult OzellikDegerleri()
        {
            return View(Context.Baglanti.OzellikDeger.ToList());
        }

        public ActionResult OzellikDegerEkle()
        {
            return View(Context.Baglanti.OzellikTip.ToList());
        }

        [HttpPost]
        public ActionResult OzellikDegerEkle(OzellikDeger od)
        {
            Context.Baglanti.OzellikDeger.Add(od);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikDegerleri");
        }

        public ActionResult OzellikUrunleri()
        {
            return View(Context.Baglanti.OzellikUrun.ToList());
        }

        public ActionResult OzellikUrunSil(int UrunId, int tipId, int degerId)
        {
            OzellikUrun uo = Context.Baglanti.OzellikUrun.FirstOrDefault(x =>
                x.UrunId == UrunId && x.OzellikTipId == tipId && x.OzellikDegerId == degerId);
            Context.Baglanti.OzellikUrun.Remove(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikUrunleri");
        }

        public ActionResult UrunOzellikEkle()
        {

            return View(Context.Baglanti.Urun.ToList());

        }

        public PartialViewResult UrunOzellikTipWidget(int? katId)
        {
            if (katId != null)
            {
                var data = Context.Baglanti.OzellikTip.Where(x => x.KategoriID == katId).ToList();
                return PartialView(data);
            }
            else
            {
                var data = Context.Baglanti.OzellikTip.ToList();
                return PartialView(data);
            }
        }

        public PartialViewResult UrunOzellikDegerWidget(int? tipId)
        {
            if (tipId != null)
            {
                var data = Context.Baglanti.OzellikDeger.Where(x => x.OzellikTipId == tipId).ToList();
                return PartialView(data);
            }
            else
            {
                var data = Context.Baglanti.OzellikDeger.ToList();
                return PartialView(data);
            }
        }

        [HttpPost]
        public ActionResult UrunOzellikEkle(OzellikUrun uo)
        {
            Context.Baglanti.OzellikUrun.Add(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        }


        public ActionResult UrunResimEkle(int id)
        {

            TempData["UrunAdi"] = Context.Baglanti.Urun.FirstOrDefault(x => x.id == id).Adi;
            ViewBag.Id = id;
            var data = Context.Baglanti.Resim.Where(x=>x.UrunId==id).ToList();
            return View(data);
        }


        [HttpPost]
        public ActionResult UrunResimEkle(int uId, HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                Bitmap ortaResim = new Bitmap(img, Settings.UrunOrtaBoyut);
                Bitmap buyukResim = new Bitmap(img, Settings.UrunBuyukBoyut);

                string ortaYol = "/Content/UrunResim/Orta/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);
                string buyukYol = "/Content/UrunResim/Buyuk/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);

                ortaResim.Save(Server.MapPath(ortaYol));
                buyukResim.Save(Server.MapPath(buyukYol));

                Resim rsm = new Resim();
                rsm.BuyukYol = buyukYol;
                rsm.OrtaYol = ortaYol;
                rsm.UrunId = uId;
                if (Context.Baglanti.Resim.FirstOrDefault(x => x.UrunId == uId && x.Varsayilan == false) != null)
                {
                    rsm.Varsayilan = true;
                }
                else
                {
                    rsm.Varsayilan = true;
                }

                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                return View(uId);
            }

            return View(uId);
        }

        public ActionResult SliderResimleri()
        {
            var data = Context.Baglanti.Resim.Where(x => x.BuyukYol.Contains("Slider")).ToList();
            return View(data);
        }


        [HttpPost]
        public ActionResult SliderResimEkle(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                Bitmap bmp = new Bitmap(img, Settings.SliderResimBoyut);
                string yol = "/Content/SliderResim/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);
                bmp.Save(Server.MapPath(yol));
                Resim rsm = new Resim();
                rsm.BuyukYol = yol;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
            }
            return RedirectToAction("SliderResimleri");
        }


        public ActionResult UrunSil(string id)
        {
            int UrunId = Convert.ToInt32(id);
            Urun silinecekUrun =Context.Baglanti.Urun.FirstOrDefault(x => x.id == UrunId);
            Context.Baglanti.Urun.Remove(silinecekUrun);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }

        public ActionResult UrunResimSil(int id)
        {
            Resim SRsm = Context.Baglanti.Resim.FirstOrDefault(x => x.id == id);
            Context.Baglanti.Resim.Remove(SRsm);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }


        public ActionResult KategoriSil(string id)
        {
            int KategoriId = Convert.ToInt32(id);
            Kategori silinecekKategori = Context.Baglanti.Kategori.FirstOrDefault(x => x.id == KategoriId);
            Context.Baglanti.Kategori.Remove(silinecekKategori);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        public ActionResult MarkaSil(string id)
        {
            int MarkaId = Convert.ToInt32(id);
            Marka silenecekMarka = Context.Baglanti.Marka.FirstOrDefault(x => x.id == MarkaId);
            Context.Baglanti.Marka.Remove(silenecekMarka);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }

        public ActionResult SliderResimSil(int id)
        {
            Resim DeleteResim = Context.Baglanti.Resim.FirstOrDefault(x => x.id == id);
            Context.Baglanti.Resim.Remove(DeleteResim);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("SliderResimleri");
        }
        public ActionResult MarkaResimSil(int id)
        {
            Resim DeleteResim = Context.Baglanti.Resim.FirstOrDefault(x => x.id == id);
            Context.Baglanti.Resim.Remove(DeleteResim);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }
    }
}





/*
try
   {
   icerik.Okunma = icerik.Okunma ?? 0;
   icerik.Rating = icerik.Rating ?? 0;
   icerik.ToplamOylayan = icerik.ToplamOylayan ?? 0;
   icerik.AnaHaber = string.IsNullOrEmpty(icerik.AnaHaberGorselTur) ? false : true;
   
   db.Icerik.Add(icerik);
   db.SaveChanges();
   
   }
   catch (DbEntityValidationException e)
   {
   
   foreach (var eve in e.EntityValidationErrors)
   {
   Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
   foreach (var ve in eve.ValidationErrors)
   {
   Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
   }
   Response.End();
   }
 */
