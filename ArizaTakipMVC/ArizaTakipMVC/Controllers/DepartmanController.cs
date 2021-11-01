using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArizaTakipMVC.Models;

namespace ArizaTakipMVC.Controllers
{
    public class DepartmanController : Controller
    {
        // GET: Departman
        DB_ArizaTakipEntities db = new DB_ArizaTakipEntities(); 


        //Admin sayfasında departmanları listelemeyi sağlar.
        public ActionResult DepartmanListesi()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            var departman = db.tbl_Departman.ToList(); //Departmanlar listesini çekiyoruz.
            return View(departman);
        }

        //Admin sayfasında yeni departman eklemeyi sağlar.
        [HttpGet]
        public ActionResult DepartmanEkle()
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }
            return View();
        }

        [HttpPost]
        public ActionResult DepartmanEkle(tbl_Departman d)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }
            try
            {
                db.tbl_Departman.Add(d);   //Yeni departman ekliyoruz ve database'e kaydediyoruz.
                db.SaveChanges();
                return RedirectToAction("DepartmanListesi"); //Ekleme işleminden sonra Departmanlar Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Admin sayfasında seçilen departmanı güncellemeyi sağlar.
        [HttpGet]
        public ActionResult DepartmanGuncelle(int id)
        {            
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }
            tbl_Departman departman = db.tbl_Departman.Where(t => t.id_Departman == id).SingleOrDefault(); //Güncellenecek olan departmanı id'ye göre çekiyoruz.
            return View(departman);
        }

        [HttpPost]
        public ActionResult DepartmanGuncelle(tbl_Departman d)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }
            try
            {
                tbl_Departman departman = db.tbl_Departman.Where(t => t.id_Departman == d.id_Departman).SingleOrDefault();  //Güncellenecek olan departmanı id'ye göre çekiyoruz.
                departman.DepartmanAd = d.DepartmanAd;  //Departman adını güncelliyoruz.
                db.SaveChanges();
                return RedirectToAction("DepartmanListesi"); //Güncelleme işleminden sonra Departmanlar Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Admin sayfasında seçilen departmanı silmeyi sağlar.
        public ActionResult DepartmanSil(int id)
        {
            //Eğer kullanıcı oturum açmadıysa login ekranına yönlendirilir.
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            try
            {
                tbl_Departman departman = db.tbl_Departman.Where(t => t.id_Departman == id).SingleOrDefault(); //Silinecek olan departmanı id'ye göre çekiyoruz.
                db.tbl_Departman.Remove(departman);  //Departmanı siliyoruz.
                db.SaveChanges();

                return RedirectToAction("DepartmanListesi");  //Silme işleminden sonra Departmanlar Listesine yönlendiriyoruz.
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}