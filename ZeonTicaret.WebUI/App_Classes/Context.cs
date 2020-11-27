using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeonTicaret.WebUI.Models;

namespace ZeonTicaret.WebUI.App_Classes
{
    public class Context
    {
        private static Entities1 baglanti;

        public static Entities1 Baglanti
        {
            get {
                if (baglanti == null)
                {
                    baglanti = new Entities1();
                }
                return baglanti; }
            set { baglanti = value; }
        }

    }
}