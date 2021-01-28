using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartBookCase1.Models.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace SmartBookCase1.Controllers
{
    public class BookArchiveController : Controller
    {
        // GET: BookArchive
        SmartBookcaseDtbsEntities13 db = new SmartBookcaseDtbsEntities13();


        [HttpGet]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(BookArchive p1)
        {

            try
            {
                var varmi = db.BookArchive.Where(i => i.BookName == p1.BookName).SingleOrDefault();
                if (varmi != null)
                {
                    ViewBag.Message = "Girilen İsimde Kayitli bir Kitap Zaten var!! ";
                    return View();
                }

                var varmi2 = db.BookArchive.Where(i => i.BookBarcode == p1.BookBarcode).SingleOrDefault();
                if (varmi2 != null)
                {
                    ViewBag.Message = "Girilen Barkodta Kayitli bir Kitap Zaten var!! ";
                    return View();
                }

                db.BookArchive.Add(p1);
                db.SaveChanges();


                return RedirectToAction("ViewBook", "BookArchive");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult ViewBook(string p)
        {
            var degerler = from d in db.BookArchive select d;
            if (!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(m => m.BookName.Contains(p));
            }
            return View(degerler.ToList());          
        }

        [HttpGet]
        public ActionResult DeleteBook(int id)
        {

            return View();
        }

        [HttpPost]
        public ActionResult DeleteBook(int id, UserInformation p1)
        {
            int kullaniciID = (int)Session["UserID"];
            var kisi = db.UserInformation.Where(i => i.UserID == kullaniciID).SingleOrDefault();

            string pswrd = Encrypt.MD5Create(p1.UserPassword);
            p1.UserPassword = pswrd;
            if (kisi.UserPassword != p1.UserPassword)
            {
                ViewBag.Message = " Sifre Yanlis Girilmistir, Lutfen Tekrar giris yapiniz ";
                return View();
            }
            try
            {
                var a = db.BookArchive.Find(id);
                db.BookArchive.Remove(a);
                db.SaveChanges();
            }
            catch
            {
                ViewBag.Message = " Kitaba Kayitli kiralama islemleri oldugu icin silme islemini gerceklestiremiyoruz. ";
                return View();
            }          

            return RedirectToAction("ViewBook", "BookArchive");
        }


        [HttpGet]
        public ActionResult EditBook(int id)
        {
            var kitap = db.BookArchive.Where(i => i.BookID == id).SingleOrDefault();
            return View(kitap);
        }

        [HttpPost]
        public ActionResult EditBook(int id, BookArchive p1)
        {
            try
            {
                var kitap = db.BookArchive.Where(i => i.BookID == id).SingleOrDefault();
                kitap.BookName = p1.BookName;
                kitap.BookCategory = p1.BookCategory;
                kitap.BookStock = p1.BookStock;
                kitap.BookAuthor = p1.BookAuthor;
                kitap.BookBarcode = p1.BookBarcode;
                kitap.BookPublisherHouse = p1.BookPublisherHouse;

                db.SaveChanges();
                return RedirectToAction("ViewBook", "BookArchive");
            }
            catch
            {
                return View(p1);
            }


        }


    }
}