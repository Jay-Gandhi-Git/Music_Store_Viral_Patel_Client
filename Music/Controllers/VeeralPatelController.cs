using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Music.Controllers
{
    public class VeeralPatelController : Controller
    {
        // GET: VeeralPatel
        Music_DBEntities db = new Music_DBEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            MainProfile mp = db.MainProfiles.FirstOrDefault();
            Session["Twitter"] = mp.TwiterLink;
            Session["Facebook"] = mp.FacebookLink;
            Session["Youtube"] = mp.YouTubeLink;
            Session["google+"] = mp.Google_Link;
            return View();
        }

        public ActionResult Photos()
        {
            return View(db.PhotoAlbums.ToList());
        }
        public ActionResult AlbumDetail(int id)
        {
            Session["PhotoAlbum_Id"] = id;
            return View(db.PhotoAlbums.Find(id).PhotoAlbumDetails.ToList());
        }

        public ActionResult Songs(int id=0)
        {
            return View(db.Audios.Where(m=>m.AudioType_Id==id).ToList());
        }

        public ActionResult Videos(int id=0)
        {
            return View(db.Videos.Where(m=>m.VideoType_Id==id).ToList());
        }
        public ActionResult ContactUs()
        {
            MainProfile mp = db.MainProfiles.FirstOrDefault();
            return View(mp);
        }
        [HttpPost]
        public ActionResult ContactUs(string Email, string Name, string Phone, string Message)
        {
            MainProfile mp = db.MainProfiles.FirstOrDefault();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("jaygandhi933@gmail.com");
                mail.To.Add(Email);
                mail.Subject = "Music Store";
                mail.Body = "Name: " + Name + System.Environment.NewLine + "Contact Number is " + Phone + System.Environment.NewLine + Message;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("jaygandhi933@gmail.com", "9033344995jalu");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                ViewBag.msg = "Mail has been sent";
            }
            catch
            {
                ViewBag.error = "Mail has not been sent";
            }
            return View(mp);
        }
 
        public ActionResult AboutUs()
        {
            return View();
        }

    }
}