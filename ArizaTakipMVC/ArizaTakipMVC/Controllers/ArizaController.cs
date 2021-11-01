using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArizaTakipMVC.Models;
using ArizaTakipMVC.Models.ViewModel;

namespace ArizaTakipMVC.Controllers
{
    public class ArizaController : Controller
    {
        // GET: Ariza
        DB_ArizaTakipEntities db = new DB_ArizaTakipEntities();


        //Arızaların listelenmesini sağlar.
        public ActionResult ArizaListesi()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Kullanici"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            ArizaViewModel avm = new ArizaViewModel();
            List<SelectListItem> ArizaTurListesi = new List<SelectListItem>();

            // Arıza türlerini SelectListItem ile liste şeklinde alıp dropdownlist'e ekliyoruz.
            foreach (tbl_ArizaTur item in db.tbl_ArizaTur.ToList())
            {
                ArizaTurListesi.Add(new SelectListItem { Value = item.id_ArizaTur.ToString(), Text = item.ArizaTurAd });
            }

            //Oturum açmış kullanıcın id'si
            int id = Convert.ToInt32(User.Identity.Name);

            //Oturum açmış kullanıcı objesi oluşturulur. 
            var myKullanici = db.tbl_Kullanici.FirstOrDefault(k => k.id_Kullanici == id);

            //Kullanıcının türünü yetkilendirme vermek için ViewDatada tutulur. 
            ViewData["id_KullaniciTur"] = myKullanici.id_KullaniciTur;

            //Yalnızca oturum açmış kullanıcının bağlı olduğu departmana ait arızalar listelenir.
            avm.Arizalar = db.tbl_Ariza.Where(t => t.tbl_Kullanici.id_Departman == myKullanici.id_Departman).ToList();
           
            //Arıza türleri listesini çekiyoruz.
            avm.ArizaTurleri = db.tbl_ArizaTur.ToList();
            avm.ArizaTurListesi = ArizaTurListesi;
            avm.ArizaYeniKayit = null;
            return View(avm);
        }

        //Yeni arıza kaydı oluşturulur.
        [HttpPost]
        public ActionResult ArizaKayit(ArizaViewModel a)
        {
            try
            {
                a.ArizaYeniKayit.ArizaGirisTarihi = DateTime.Now;       //Arıza giriş tarihi olarak mevcut oluşturulma tarihi tutulur.
                a.ArizaYeniKayit.id_ArizaDurumTur = 1;                 //Standart olarak beklemede durumu girilir.
                a.ArizaYeniKayit.id_Kullanici = Convert.ToInt32(User.Identity.Name);  //Arızayı oluşturan mevcut kullanıcın id'si.

                db.tbl_Ariza.Add(a.ArizaYeniKayit);  
                db.SaveChanges();

                return RedirectToAction("ArizaListesi"); //Ekleme işleminden sonra Arıza Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Arızayı güncelleme işlemini sağlar.
        [HttpGet]
        public ActionResult ArizaGuncelle(int id)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Kullanici"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            ArizaViewModel avm = new ArizaViewModel(); 
            List<SelectListItem> ArızaTurListesi = new List<SelectListItem>();  //Arıza türleri için tanımlanan SelectListItem

            // Arıza türlerini SelectListItem ile liste şeklinde alıp dropdownlist'e ekliyoruz.
            foreach (tbl_ArizaTur item in db.tbl_ArizaTur.ToList())
            {
                ArızaTurListesi.Add(new SelectListItem { Value = item.id_ArizaTur.ToString(), Text = item.ArizaTurAd });
            }

            avm.ArizaTurListesi = ArızaTurListesi;

             //Güncellenecek olan arızayı id ye göre alıyoruz.
            avm.ArizaYeniKayit = db.tbl_Ariza.Where(t => t.id_Ariza == id).SingleOrDefault();

            return View(avm);
        }

        [HttpPost]
        public ActionResult ArizaGuncelle(ArizaViewModel a)
        {
            try
            {
                tbl_Ariza ariza = db.tbl_Ariza.FirstOrDefault(t => t.id_Ariza == a.ArizaYeniKayit.id_Ariza); //Güncellenecek olan arızayı id'ye göre çekiyoruz.
                ariza.Aciklama = a.ArizaYeniKayit.Aciklama;        //Açıklama güncellenir.
                ariza.id_ArizaTur = a.ArizaYeniKayit.id_ArizaTur;  //Arıza türü güncellenir.
                db.SaveChanges();
                return RedirectToAction("ArizaListesi");          //Güncelleme işleminden sonra Arıza Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Arıza durumunu güncelleme işlemi. (Beklemede,işleme alındı,arıza giderildi,arıza giderilemedi vs.)
        public ActionResult ArizaDurumGuncelle(int id, byte id2)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Kullanici"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            try
            {
                tbl_Ariza ariza = db.tbl_Ariza.FirstOrDefault(t => t.id_Ariza == id); //Güncellenecek olan arıza id ye göre alınır.
                ariza.id_ArizaDurumTur = id2;                                        // Arıza durum id'si

                db.SaveChanges();

                return RedirectToAction("ArizaListesi");  //Güncelleme işleminden sonra Arıza Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Seçili olan arızayı silmeyi sağlar.
        public ActionResult ArizaSil(int id)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Kullanici"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            try
            {
                tbl_Ariza ariza = db.tbl_Ariza.Where(t => t.id_Ariza == id).SingleOrDefault(); //Silinecek olan arızayı id'ye göre alıyoruz.
                db.tbl_Ariza.Remove(ariza);   //Arızayı siliyoruz.
                db.SaveChanges();

                return RedirectToAction("ArizaListesi");     //Silme işleminden sonra Arıza Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Admin sayfasında arıza türlerinin listelenmesini sağlar.
        public ActionResult ArizaTurListesi()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            var arizaTur = db.tbl_ArizaTur.ToList();    //Arıza türleri listesini çekiyoruz.
            return View(arizaTur);
        }


        //Admin sayfasında yeni arıza türü eklemeyi sağlar.
        [HttpGet]
        public ActionResult ArizaTurEkle()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ArizaTurEkle(tbl_ArizaTur a)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }
            try
            {
                db.tbl_ArizaTur.Add(a);  //Yeni arıza türü eklenir ve db'ye kaydedilir.
                db.SaveChanges();
                return RedirectToAction("ArizaTurListesi"); //Ekleme işleminden sonra Arıza Türleri Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Admin sayfasında seçili olan arıza türünü güncellemeyi sağlar.
        [HttpGet]
        public ActionResult ArizaTurGuncelle(int id)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }
            tbl_ArizaTur arizaTur = db.tbl_ArizaTur.Where(t => t.id_ArizaTur == id).SingleOrDefault(); //Güncellenecek olan arıza türünü id'ye göre alıyoruz.
            return View(arizaTur);
        }

        [HttpPost]
        public ActionResult ArizaTurGuncelle(tbl_ArizaTur a)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }
            try
            {
                tbl_ArizaTur arizaTur = db.tbl_ArizaTur.Where(t => t.id_ArizaTur == a.id_ArizaTur).SingleOrDefault(); //Güncellenecek olan arıza türünü id'ye göre alıyoruz.
                arizaTur.ArizaTurAd = a.ArizaTurAd;   //Arıza tür adı güncellenir.
                db.SaveChanges();
                return RedirectToAction("ArizaTurListesi");  //Güncelleme işleminden sonra Arıza Türleri Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Admin sayfasında seçili olan arıza türünü silmeyi sağlar.
        public ActionResult ArizaTurSil(int id)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login","Kullanici");
            }

            try
            {
                tbl_ArizaTur arizaTur = db.tbl_ArizaTur.Where(t => t.id_ArizaTur == id).SingleOrDefault(); //Arıza türünü id'ye göre alıyoruz.
                db.tbl_ArizaTur.Remove(arizaTur);  // id'ye göre alınan arıza türünü siliyoruz.
                db.SaveChanges();

                return RedirectToAction("ArizaTurListesi"); // Silme işleminden sonra Arıza Türleri Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}