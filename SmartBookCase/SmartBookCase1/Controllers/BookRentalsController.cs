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
    public class BookRentalsController : Controller
    {

        SmartBookcaseDtbsEntities13 db = new SmartBookcaseDtbsEntities13();
      

        public ActionResult RentalOperations(string p)
        {
            var degerler = from d in db.BookArchive select d;
            if (!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(m => m.BookName.Contains(p));
            }
            return View(degerler.ToList());
        }

        [HttpGet]
        public ActionResult AddBookRental(int id)
        {
            var kitap = db.BookArchive.Where(i => i.BookID == id).SingleOrDefault();
            Session["BookID"] = kitap.BookID;
            return View(kitap);
            
        }

        [HttpPost]
        public ActionResult AddBookRental(BookRent b1, MemberInformation m1)
        {
            try
            {
                var varmi = db.MemberInformation.Where(i => i.MemberTcNo == m1.MemberTcNo).SingleOrDefault();
                if (varmi == null)
                {
                    ViewBag.Message = "Girilen Tc Kimlik Numarasına Kayıtlı bir Hesap Yoktur!! ";
                    return View();
                }
                b1.MemberID = varmi.MemberID;
                b1.BookID = (int)Session["BookID"];
                var forstock = db.BookArchive.Where(i => i.BookID == b1.BookID).SingleOrDefault();
                forstock.BookStock = forstock.BookStock - 1;
                b1.UserID = (int)Session["UserID"];
                b1.IsReturn = false;          
                b1.ReturnDate = b1.RentDate.AddDays(b1.RentDay); 
                db.BookRent.Add(b1);
                db.SaveChanges();
                var varmi2 = db.BookArchive.Where(a => a.BookID == b1.BookID).SingleOrDefault();
                MailMessage eposta = new MailMessage();
                eposta.From = new MailAddress("smartbookcase@hotmail.com");
                eposta.To.Add(varmi.MemberEmail);
                eposta.Subject = "SMART-BOOKCASE";
                eposta.Body = "Sayın " + varmi.MemberName + "; Kütüphanemizden " + varmi2.BookName + " İsimli Kitabı " + b1.RentDay + " gün Kiralamış bulunmaktasınız. Kİralama Başlangış tarihiniz "+ b1.RentDate + " olup, Teslim tarihiniz " + b1.ReturnDate + " 'dır. İYİ OKUMALAR... ";        
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("smartbookcase@hotmail.com", "TeamForza5");
                smtp.Port = 587;
                smtp.Host = "smtp.live.com";
                smtp.EnableSsl = true;
                smtp.Send(eposta);
         
           
                return RedirectToAction("RentalOperations", "BookRentals");
            }
            catch
            {
                return RedirectToAction("RentalOperations", "BookRentals");
            }
        }

        public ActionResult AllBookRental(int id)
        {
            var degerler = from d in db.BookRent select d;
            
                degerler = degerler.Where(m => m.BookID==id);
                degerler = degerler.Where(n => n.IsReturn == false);

            return View(degerler.ToList());          
        }

        public ActionResult BookRentalDetail(int id)
        {
            Class1 db2 = new Class1();

            var x = db.BookRent.Where(i => i.RentID == id).SingleOrDefault();
            var y = db.BookArchive.Where(i => i.BookID == x.BookID).SingleOrDefault();
            var z = db.MemberInformation.Where(i => i.MemberID == x.MemberID).SingleOrDefault();
            var w = db.UserInformation.Where(i => i.UserID == x.UserID).SingleOrDefault();
            db2.BookRentc = x;
            db2.BookArchivec = y;
            db2.MemberInformationc = z;
            db2.UserInformationc = w;
           
            return View("BookRentalDetail",db2);
        }


        public ActionResult ReturnOperations(string p)
        {
            var degerler = from d in db.MemberInformation select d;
            if (!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(m => m.MemberTcNo.Contains(p));
            }
            return View(degerler.ToList());
        }
        
       
        public ActionResult ReturnBookRental(int id)
        {
           
            Class1 db2 = new Class1();
            var degerler = from d in db.BookRent select d;
        
            degerler = degerler.Where(m => m.MemberID == id);
            
           
            return View(degerler.ToList());
        }

        public ActionResult ReturnBookConfirm(int id)
        {
            var x = db.BookRent.Where(i => i.RentID == id).SingleOrDefault();
           
            if (x.IsReturn == true)
            {
                return RedirectToAction("ReturnOperations", "BookRentals");
            } 
            x.IsReturn = true;
            x.ReturnDate = DateTime.Today;
            db.SaveChanges();

            var member = db.MemberInformation.Where(i => i.MemberID == x.MemberID).SingleOrDefault();
            var kitap = db.BookArchive.Where(i => i.BookID == x.BookID).SingleOrDefault();

            MailMessage eposta = new MailMessage();
            eposta.From = new MailAddress("smartbookcase@hotmail.com");
            eposta.To.Add(member.MemberEmail);
            eposta.Subject = "SMART-BOOKCASE";
            eposta.Body = "Sayın " + member.MemberName + "; Kütüphanemizden almış olduğunuz " +kitap.BookName +" isimli kitabı teslim etmiş bulunmaktasınız.";
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("smartbookcase@hotmail.com", "TeamForza5");
            smtp.Port = 587;
            smtp.Host = "smtp.live.com";
            smtp.EnableSsl = true;
            smtp.Send(eposta);
 
            return RedirectToAction("ReturnOperations", "BookRentals");

        }

        [HttpGet]
        public ActionResult RentalExtend(int id)
        {
            var kiralama = db.BookRent.Where(i => i.RentID == id).SingleOrDefault();
            return View(kiralama);
        }

        [HttpPost]
        public ActionResult RentalExtend(int id, BookRent p1)
        {
            try
            {
                var kiralama = db.BookRent.Where(i => i.RentID == id).SingleOrDefault();

                kiralama.RentDay = p1.RentDay;
                kiralama.ReturnDate = kiralama.RentDate.AddDays(p1.RentDay);
              


                db.SaveChanges();
                return RedirectToAction("RentalOperations", "BookRentals");
            }
            catch
            {
                return View(p1);
            }

        }



    }
}