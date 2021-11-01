using ArizaTakipMVC.Models;
using ArizaTakipMVC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArizaTakipMVC.Controllers
{
    public class GorevController : Controller
    {
        // GET: Gorev
      
        DB_ArizaTakipEntities db = new DB_ArizaTakipEntities();

        public ActionResult GorevListesi()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Kullanici"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            GorevViewModel gvm = new GorevViewModel();

            int id = Convert.ToInt32(User.Identity.Name);   //Oturum açmış kullanıcın id'si

            var myKullanici = db.tbl_Kullanici.FirstOrDefault(k => k.id_Kullanici == id);  //Oturum açmış kullanıcı objesi oluşturulur. 

            ViewData["id_KullaniciTur"] = myKullanici.id_KullaniciTur;              //Kullanıcının türünü yetkilendirme vermek için ViewDatada tutulur. 

            //Yalnızca oturum açmış kullanıcının bağlı olduğu departmana ait görevler listelenir.
            gvm.Gorevler = db.tbl_Gorev.Where(t => t.tbl_Kullanici.id_Departman == myKullanici.id_Departman).ToList();
            gvm.GorevYeniKayit = null;

            return View(gvm);
        }


        //Yeni görev kaydı eklemeyi sağlar.
        [HttpPost]
        public ActionResult GorevKayit(GorevViewModel g)
        {
            try
            {
                g.GorevYeniKayit.id_GorevDurumTur = 1;    //Standart olarak beklemede durumu girilir.
                g.GorevYeniKayit.id_Kullanici = Convert.ToInt32(User.Identity.Name);  //Görev oluşturan mevcut kullanıcın id'si. (Müdür)

                db.tbl_Gorev.Add(g.GorevYeniKayit);  //Görev eklenir ve database'e kaydedilir.
                db.SaveChanges();

                return RedirectToAction("GorevListesi");  //Ekleme işleminden sonra Görev Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Seçilen görevin güncellenmesi sağlar.
        [HttpGet]
        public ActionResult GorevGuncelle(int id)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Kullanici"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            GorevViewModel gvm = new GorevViewModel();
          
            gvm.GorevYeniKayit = db.tbl_Gorev.Where(t => t.id_Gorev == id).SingleOrDefault(); //Güncellenecek olan görevi id'ye göre çekiyoruz.

            return View(gvm);
        }

        [HttpPost]
        public ActionResult GorevGuncelle(GorevViewModel g)
        {
            try
            {
                tbl_Gorev gorev = db.tbl_Gorev.FirstOrDefault(t => t.id_Gorev == g.GorevYeniKayit.id_Gorev); //Güncellenecek olan görevi id'ye göre çekiyoruz.
                gorev.GorevAciklama = g.GorevYeniKayit.GorevAciklama;  //Açıklamayı güncelliyoruz.
                gorev.GorevBasTarihi = g.GorevYeniKayit.GorevBasTarihi; //Görevin başlama tarihini güncelliyoruz.
                gorev.GorevBitTarihi = g.GorevYeniKayit.GorevBitTarihi; //Görevin bitiş tarihini güncelliyoruz.

                db.SaveChanges();
                return RedirectToAction("GorevListesi"); //Ekleme işleminden sonra Görev Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Görev durumunu güncelleme işlemi. (Beklemede,işleme alındı,görev tamamlandı,görev tamamlanamadı vs.)
        public ActionResult GorevDurumGuncelle(int id, byte id2)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Kullanici"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            try
            {
                tbl_Gorev gorev = db.tbl_Gorev.FirstOrDefault(t => t.id_Gorev == id); //Güncellenecek olan görev durumu id'ye göre çekiliyor.
                gorev.id_GorevDurumTur = id2;    //Görev durum id'si

                db.SaveChanges();

                return RedirectToAction("GorevListesi"); //Güncelleme işleminden sonra Görev Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}