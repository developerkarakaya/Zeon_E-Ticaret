using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZeonTicaret.WebUI.Controllers
{
    public class KullaniciController : Controller
    {
        
        public ActionResult GirisYap()
        {
            return View();
        }

        public ActionResult KayitOl()
        {
            return View();
        }
    }
}