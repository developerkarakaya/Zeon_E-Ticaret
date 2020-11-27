using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeonTicaret.WebUI.App_Classes;
using ZeonTicaret.WebUI.Models;

namespace ZeonTicaret.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private object context;

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult Sepet()
        {
            return PartialView();
        }

        public PartialViewResult Slider()
        {
            var data = Context.Baglanti.Resim.Where(x => x.BuyukYol.Contains("Slider")).ToList();
            return PartialView(data);
        }
        public PartialViewResult Kategori()
        {

            var data = Context.Baglanti.Kategori.Take(3).ToList();

            return PartialView(data);
        }



        [ChildActionOnly]
        public PartialViewResult YeniUrunler()
        {
            var data = Context.Baglanti.Urun.ToList();
            return PartialView(data);
        }

        public PartialViewResult Servisler()
        {
            return PartialView();
        }

        public PartialViewResult Markalar()
        {
            var data = Context.Baglanti.Marka.ToList();

           return PartialView(data);
        }

        public ActionResult Urunler()
        {
            if (Request.QueryString["KategoriId"] != null)
            {

                ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
                ViewBag.Markalar = Context.Baglanti.Marka.ToList();
                int KategoriId = Convert.ToInt32(Request.QueryString["KategoriId"]);
                ViewBag.Urunler = Context.Baglanti.Urun.Where(x => x.KategoriID == KategoriId).ToList();
                TempData["KategoriAdi"] = Context.Baglanti.Kategori.Find(KategoriId).Adi;
                return View();
            }
            else if (Request.QueryString["MarkaId"] != null)
            {

                ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
                ViewBag.Markalar = Context.Baglanti.Marka.ToList();
                int MarkaId = Convert.ToInt32(Request.QueryString["MarkaId"]);
                ViewBag.Urunler = Context.Baglanti.Urun.Where(x => x.MarkaID == MarkaId).ToList();
                TempData["MarkaAdi"] = Context.Baglanti.Marka.Find(MarkaId).Adi;
                return View();
            }
            else
            {
                ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
                ViewBag.Markalar = Context.Baglanti.Marka.ToList();
                ViewBag.Urunler = Context.Baglanti.Urun.ToList();
                return View();
            }
        }

        public ActionResult iletisim()
        {
            return View();
        }

        public ActionResult Hakkimizda()
        {
            return View();
        }

        public void SepeteEkle(int id)
        {
            SepetItem si = new SepetItem();
            Urun u = Context.Baglanti.Urun.FirstOrDefault(x => x.id == id);

            si.Urun = u;
            si.Adet = 1;
            si.Indirim = 0;
            Sepet s = new Sepet();
            s.SepeteEkle(si);
            MiniSepetWidget();
        }

        public PartialViewResult MiniSepetWidget()
        {
            if (HttpContext.Session["AktifSepet"] != null)
            {
                return PartialView((Sepet)HttpContext.Session["AktifSepet"]);
            }
            else
            {
                return PartialView();
            }

        }

        public void SepetUrunAdetDusur(int id)
        {
            if(HttpContext.Session["AktifSepet"]!=null)
            {
                Sepet s = (Sepet)HttpContext.Session["AktifSepet"];

                if (s.Urunler.FirstOrDefault(x => x.Urun.id == id).Adet > 1) 
                {
                    s.Urunler.FirstOrDefault(x => x.Urun.id == id).Adet--;
                }
                else{
                    SepetItem si = s.Urunler.FirstOrDefault(x => x.Urun.id == id);
                    s.Urunler.Remove(si);
                }
            }
        }

        public ActionResult UrunDetay()
        {
            string id = Request.QueryString["id"];
            int kId = Convert.ToInt32(Request.QueryString["kId"]);
            ViewBag.Markalar = Context.Baglanti.Marka.ToList();
            ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
            ViewBag.Urunler = Context.Baglanti.Urun.Where(x=>x.KategoriID==kId).Take(4).ToList();
            ViewBag.OzellikDeger = Context.Baglanti.OzellikDeger.ToList();
            Urun u = Context.Baglanti.Urun.FirstOrDefault(x => x.Adi == id);
            return View(u);
        }
        public PartialViewResult KategoriListeleme()
        {
            var data = Context.Baglanti.Kategori.ToList();
           return PartialView(data);
        }

        public ActionResult GirisYap()
        {
            return View();
        }

    }
}