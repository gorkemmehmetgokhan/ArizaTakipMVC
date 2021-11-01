using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArizaTakipMVC.Models.ViewModel
{
    public class GorevViewModel
    {
        public List<tbl_Gorev> Gorevler { get; set; }
        public tbl_Gorev GorevYeniKayit { get; set; }

    }
}