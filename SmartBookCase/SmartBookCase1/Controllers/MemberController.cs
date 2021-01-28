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
    public class MemberController : Controller
    {
        SmartBookcaseDtbsEntities13 db = new SmartBookcaseDtbsEntities13();

        [HttpGet]
        public ActionResult AddMember()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(MemberInformation p1)
        {

            try
            {
                var varmi = db.MemberInformation.Where(i => i.MemberEmail == p1.MemberEmail).SingleOrDefault();
                if (varmi != null)
                {
                    ViewBag.Message = "Girilen E-maile Kayitli bir Kullanici Hesabi Var!! ";
                    return View();
                }
                var varmi2 = db.MemberInformation.Where(i => i.MemberTcNo == p1.MemberTcNo).SingleOrDefault();
                if (varmi2 != null)
                {
                    ViewBag.Message = "Girilen TC. No ile Kayitli bir Kullanici Hesabi Var!! ";
                    return View();
                }

                db.MemberInformation.Add(p1);
                db.SaveChanges();

                MailMessage eposta = new MailMessage();
                eposta.From = new MailAddress("smartbookcase@hotmail.com");
                eposta.To.Add(p1.MemberEmail);
                eposta.Subject = "SMART-BOOKCASE";
                eposta.Body = "Sayın " + p1.MemberName + "; Kütüphanemizde üye kaydınız başarıyla oluşturulmuştur.";
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("smartbookcase@hotmail.com", "TeamForza5");
                smtp.Port = 587;
                smtp.Host = "smtp.live.com";
                smtp.EnableSsl = true;
                smtp.Send(eposta);

                return RedirectToAction("ViewMember", "Member");
            }
            catch
            {
                return RedirectToAction("ViewMember", "Member");
            }

        }

        public ActionResult ViewMember(string p)
        {
            var degerler = from d in db.MemberInformation select d;
            if (!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(m => m.MemberName.Contains(p));
            }
            return View(degerler.ToList());
            //var uyeler = db.MemberInformation.ToList();
            //return View(uyeler);
        }
        [HttpGet]
        public ActionResult DeleteMember(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteMember(int id, UserInformation p1)
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
                var a = db.MemberInformation.Find(id);
                db.MemberInformation.Remove(a);
                db.SaveChanges();
            }
            catch
            {
                ViewBag.Message = " Kullaniciya Kayitli kiralama islemleri oldugu icin silme islemini gerceklestiremiyoruz. ";
                return View();
            }
                      

            return RedirectToAction("ViewMember", "Member");
        }

        [HttpGet]
        public ActionResult EditMember(int id)
        {
            var kisi = db.MemberInformation.Where(i => i.MemberID == id).SingleOrDefault();
            return View(kisi);
        }

        [HttpPost]
        public ActionResult EditMember(int id, MemberInformation p1)
        {
            try
            {
                var kisi = db.MemberInformation.Where(i => i.MemberID == id).SingleOrDefault();
                kisi.MemberName = p1.MemberName;
                kisi.MemberEmail = p1.MemberEmail;
                kisi.MemberPhone = p1.MemberPhone;
                kisi.MemberTcNo = p1.MemberTcNo;

                db.SaveChanges();
                return RedirectToAction("ViewMember", "Member");
            }
            catch
            {
                return View(p1);
            }


        }
    }
}