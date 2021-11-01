using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArizaTakipMVC.Models.ViewModel
{
    public class KullaniciViewModel
    {
        public List<tbl_Kullanici> Kullanicilar { get; set; }
        public tbl_Kullanici kullaniciYeniKayit { get; set; }
        public IEnumerable<SelectListItem> DepartmanListesi { get; set; }
        public IEnumerable<SelectListItem> KullaniciTurListesi { get; set; }
    }
}