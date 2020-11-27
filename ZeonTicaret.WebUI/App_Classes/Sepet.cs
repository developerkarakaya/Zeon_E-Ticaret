using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeonTicaret.WebUI.Models;

namespace ZeonTicaret.WebUI.App_Classes
{
    public class Sepet
    {

        public static Sepet AktifSepet
        {
            get{
                HttpContext ctx = HttpContext.Current;
                if (ctx.Session["AktifSepet"] == null)
                {
                    ctx.Session["AktifSepet"] = new Sepet();
                }
                return (Sepet)ctx.Session["AktifSepet"];
            }
        }

        private static List<SepetItem> urunler = new List<SepetItem>();

        public List<SepetItem> Urunler
        {
            get { return urunler; }
            set { urunler=value; }
        } 



        public void SepeteEkle(SepetItem si)
        {
            if (HttpContext.Current.Session["AktifSepet"] != null)
            {
                Sepet s = (Sepet)HttpContext.Current.Session["AktifSepet"];

                if (s.Urunler.Any(x => x.Urun.id == si.Urun.id))
                {
                   s.Urunler.FirstOrDefault(x => x.Urun.id == si.Urun.id).Adet++;
                }
                else
                {
                    s.Urunler.Add(si);
                }
            }
            else
            {
                Sepet s = new Sepet();
                s.Urunler.Add(si);
                HttpContext.Current.Session["AktifSepet"] = s;
            }

        }
         

        public static decimal ToplamTutar
        {
            get
            {
                return urunler.Sum(x => x.Tutar);
            }
        }



    }

    public class SepetItem
    {
        public Urun Urun { get; set; }
        public int Adet { get; set; }
        public double Indirim { get; set; }

        public decimal Tutar
        {
            get
            {
                return 0;
            }
        }
    }


}