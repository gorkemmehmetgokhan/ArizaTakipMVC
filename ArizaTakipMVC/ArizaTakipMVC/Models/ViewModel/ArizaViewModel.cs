using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArizaTakipMVC.Models.ViewModel
{
    public class ArizaViewModel
    {  
        public tbl_Ariza ArizaYeniKayit { get; set; }
        public List<tbl_Ariza> Arizalar { get; set; }
        public List<tbl_ArizaTur> ArizaTurleri { get; set; }
        public IEnumerable<SelectListItem> ArizaTurListesi { get; set; }

    }
}