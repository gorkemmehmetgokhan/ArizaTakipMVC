using ArizaTakipMVC.Models;
using ArizaTakipMVC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ArizaTakipMVC.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici

        DB_ArizaTakipEntities db = new DB_ArizaTakipEntities();


        //Admin Layout'una yönlendirir.
        public ActionResult AdminIndex()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        //Kullanıcı Layout'una yönlendirir.
        public ActionResult KullaniciIndex()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Kullanici"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }


        //Sitede oturum açma işlemini sağlar
        public ActionResult Login()
        {
            //kullanıcının oturum açma işlemi başarili ise
            if (User.Identity.IsAuthenticated && Session["Admin"] != null)
            {
                return RedirectToAction("AdminIndex");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login(tbl_Kullanici k)
        {
            if (ModelState.IsValid)
            {
                using (DB_ArizaTakipEntities ctx = new DB_ArizaTakipEntities())
                {
                    var kullanici = ctx.tbl_Kullanici.FirstOrDefault(x => x.Eposta == k.Eposta && x.Sifre == k.Sifre); //Eposta ve Sifreye göre siteye giriş kontrolü sağlanır.
                  
                    if (kullanici != null && kullanici.id_KullaniciTur == 1)  //Eğer kullanıcı türü 1 ise admindir ve admin sayfasına yönlendirir.
                    {
                        Session["Admin"] = kullanici;        //Kullanıcıyı session'da tutuyoruz.
                        FormsAuthentication.SetAuthCookie(kullanici.ToString(), true);  //Kullanıcıyı cookie'de tutuyoruz.
                        FormsAuthentication.SetAuthCookie(kullanici.id_Kullanici.ToString(), true);
                        return RedirectToAction("AdminIndex");  //Oturum açtıksan sonra admin sayfasına yönlendirilir.
                    }
                    else if (kullanici != null && kullanici.id_KullaniciTur != 1)  //Eğer kullanıcı türü 1 değilse yani admin değilse kullanıcı sayfasına yönlendirilir.
                    {
                        Session["Kullanici"] = kullanici;   //Kullanıcıyı session'da tutuyoruz.
                        FormsAuthentication.SetAuthCookie(kullanici.ToString(), true);   //Kullanıcıyı cookie'de tutuyoruz.
                        FormsAuthentication.SetAuthCookie(kullanici.id_Kullanici.ToString(), true);
                        return RedirectToAction("KullaniciIndex");  //Oturum açtıktan sonra kullanıcı sayfasına yönlendirilir.
                    }
                }
            }
            return View();
        }


        // Siteden çıkış yapmayı sağlar.
        public ActionResult Logout()
        {
            Session["Kullanici"] = null;
            Session["Admin"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        //Admin sayfasında kullanıcıları listelemeyi sağlar.
        public ActionResult KullaniciListesi()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }
            KullaniciViewModel kvm = new KullaniciViewModel();

            kvm.Kullanicilar = db.tbl_Kullanici.ToList();  //Kullanıcılar listesini çekiyoruz.
            return View(kvm);
        }


        //Admin sayfasında yeni kullanıcı eklemeyi sağlar.
        [HttpGet]
        public ActionResult KullaniciEkle()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }

            KullaniciViewModel kvm = new KullaniciViewModel();
            List<SelectListItem> DepartmanListesi = new List<SelectListItem>();
            List<SelectListItem> KullaniciTurListesi = new List<SelectListItem>();

            //Departmanları SelectListItem ile liste şeklinde alıp dropdownlist'e ekliyoruz. (Kullanıcıya departman eklemek için.)
            foreach (tbl_Departman item in db.tbl_Departman.ToList())
            {
                DepartmanListesi.Add(new SelectListItem { Value = item.id_Departman.ToString(), Text = item.DepartmanAd });
            }

            // Kullanıcı Türlerini SelectListItem ile liste şeklinde alıp dropdownlist'e ekliyoruz.  (Kullanıcıya kullanıcı türü eklemek için.)
            foreach (tbl_KullaniciTur item in db.tbl_KullaniciTur.ToList())
            {
                KullaniciTurListesi.Add(new SelectListItem { Value = item.id_KullaniciTur.ToString(), Text = item.TurAd });
            }

            kvm.DepartmanListesi = DepartmanListesi;
            kvm.KullaniciTurListesi = KullaniciTurListesi;
            kvm.Kullanicilar = null;
            kvm.kullaniciYeniKayit = null;
            return View(kvm);
        }

        [HttpPost]
        public ActionResult KullaniciEkle(KullaniciViewModel k)
        {

            if (ModelState.IsValid)
            {

                db.tbl_Kullanici.Add(k.kullaniciYeniKayit); //Yeni kullanıcı eklenir ve database'e kaydedilir.
                db.SaveChanges();

                return RedirectToAction("KullaniciListesi"); //Ekleme işleminden sonra Kullanıcı Listesine yönlendiriyoruz.
            }
            else
                return View();
        }

        //Admin sayfasında seçilen kullanıcıyı güncellemeyi sağlar.
        [HttpGet]
        public ActionResult KullaniciGuncelle(int id)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }

            KullaniciViewModel kvm = new KullaniciViewModel();
            List<SelectListItem> DepartmanListesi = new List<SelectListItem>();
            List<SelectListItem> KullaniciTurListesi = new List<SelectListItem>();

            //Departmanları SelectListItem ile liste şeklinde alıp dropdownlist'e ekliyoruz.
            foreach (tbl_Departman item in db.tbl_Departman.ToList())
            {
                DepartmanListesi.Add(new SelectListItem { Value = item.id_Departman.ToString(), Text = item.DepartmanAd });
            }

            //Kullanıcı türlerini SelectListItem ile liste şeklinde alıp dropdownlist'e ekliyoruz.
            foreach (tbl_KullaniciTur item in db.tbl_KullaniciTur.ToList())
            {
                KullaniciTurListesi.Add(new SelectListItem { Value = item.id_KullaniciTur.ToString(), Text = item.TurAd });
            }

            kvm.DepartmanListesi = DepartmanListesi;
            kvm.KullaniciTurListesi = KullaniciTurListesi;
            kvm.Kullanicilar = null;
            kvm.kullaniciYeniKayit = db.tbl_Kullanici.Where(t => t.id_Kullanici == id).SingleOrDefault(); //Güncellenecek kullanıcıyı id'ye göre alıyoruz.
            return View(kvm);
        }

        [HttpPost]
        public ActionResult KullaniciGuncelle(KullaniciViewModel k)
        {

            if (ModelState.IsValid)
            {

                tbl_Kullanici kullanici = db.tbl_Kullanici.FirstOrDefault(t => t.id_Kullanici == k.kullaniciYeniKayit.id_Kullanici); //Güncellenecek kullanıcıyı id'ye göre alıyoruz.
                kullanici.Ad = k.kullaniciYeniKayit.Ad;             //Ad güncellenir.
                kullanici.Soyad = k.kullaniciYeniKayit.Soyad;      //Soyad güncellenir.
                kullanici.Eposta = k.kullaniciYeniKayit.Eposta;   //E-posta güncellenir.
                kullanici.Sifre = k.kullaniciYeniKayit.Sifre;    //Şifre güncellenir.
                kullanici.id_Departman = k.kullaniciYeniKayit.id_Departman;  //Kullanıcının departmanı güncellenir.
                kullanici.id_KullaniciTur = k.kullaniciYeniKayit.id_KullaniciTur; //Kullanıcının kullanıcı türü güncellenir.
                db.SaveChanges();
                return RedirectToAction("KullaniciListesi"); //Güncelleme işleminden sonra Kullanıcı Listesine yönlendiriyoruz.
            }
            else
                return View();
        }


        //Admin sayfasında seçilen kullanıcıyı silmeyi sağlar.
        public ActionResult KullaniciSil(int id)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }

            try
            {
                tbl_Kullanici kullanici = db.tbl_Kullanici.Where(t => t.id_Kullanici == id).SingleOrDefault(); //Silinecek kullanıcı id'ye göre alınır.
                db.tbl_Kullanici.Remove(kullanici); //Kullanıcı silinir.
                db.SaveChanges();

                return RedirectToAction("KullaniciListesi");  //Silme işleminden sonra Kullanıcı Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}