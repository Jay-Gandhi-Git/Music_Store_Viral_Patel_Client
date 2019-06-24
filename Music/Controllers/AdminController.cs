using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Music.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        Music_DBEntities db = new Music_DBEntities();
        clsMail mail = new clsMail();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            try
            {
                Admin ad = db.Admins.Where(a => a.UserName == username && a.Password == password).FirstOrDefault();
                if (ad != null)
                {
                    Session["Admin_Id"] = ad.Admin_Id;
                    return RedirectToAction("Profile");
                }
                else
                { ViewBag.error = "Username or password is invalid"; }
            }
            catch { }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string username)
        {
            try
            {
                Admin ad = db.Admins.Where(a => a.UserName == username).FirstOrDefault();
                if (ad != null)
                {
                    mail.mailSend(username, "Forgot Password","Your password is "+ ad.Password.ToString());
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.errorMsg = "Email is not valid.";
                }
            }
            catch { ViewBag.errorMsg = "Technical error."; }
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Profile()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            MainProfile mp = db.MainProfiles.FirstOrDefault();
            return View(mp);
        }
        [HttpPost]
        public ActionResult Profile(int id=0)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                MainProfile mp = db.MainProfiles.FirstOrDefault();
                mp.Name = Request["name"];
                mp.Email = Request["email"];
                mp.ContactNumber = Request["contactnumber"];
                mp.Address = Request["address"];
                mp.CoverProfilePhoto = Request["CoverProfilePhoto"];
                mp.FacebookLink = Request["FacebookLink"];
                mp.YouTubeLink = Request["YouTubeLink"];
                mp.TwiterLink = Request["TwiterLink"];
                mp.Google_Link = Request["Google_Link"];
                mp.About_1 = Request["About_1"];
                db.Entry(mp).State = System.Data.Entity.EntityState.Modified;
                int i = db.SaveChanges();
                if (i > 0)
                { return RedirectToAction("Profile"); }
                else
                { ViewBag.error = "Failed to Update"; }
            }
            catch { }
            return View("Profile");
        }
       
        public ActionResult AdminInfo()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            return View(db.Admins.ToList());
        }
        public ActionResult AdminView(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            Admin ad = db.Admins.Find(id);
            Session["Admin_Id"] = id;
            if (ad == null)
            {
                ViewBag.error = "Data Not Found | Technical error";
            }
            return View(ad);
        }
        [HttpPost]
        public ActionResult AdminView()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            
            int id=Convert.ToInt32( Session["Admin_Id"]);
            Admin ad = db.Admins.Find(id);
            try
            {
                if (ad != null)
                {
                    ad.MobileNumber = Request["mobilenumber"];
                    ad.Password = Request["password"];
                    ad.Name = Request["name"];
                    db.Entry(ad).State = System.Data.Entity.EntityState.Modified;
                    int i = db.SaveChanges();
                    if (i > 0)
                    { return RedirectToAction("AdminInfo"); }
                    else
                    { ViewBag.error = "Failed to update";}
                }
            }
            catch{}
            return View(ad);
        }
        public ActionResult AdminDelete(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            Admin ad = db.Admins.Find(id);
            Session["Admin_Id"] = id;
            if (ad == null)
            {
                ViewBag.error = "Data Not Found | Technical error";
            }
            return View(ad);
        }
        [HttpPost]
        public ActionResult AdminDelete()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            int id = Convert.ToInt32(Session["Admin_Id"]);
            Admin ad = db.Admins.Find(id);
            try
            {
                if (ad != null)
                {
                    db.Admins.Remove(ad);

                    int i = db.SaveChanges();
                    if (i > 0)
                    { return RedirectToAction("AdminInfo"); }
                    else
                    { ViewBag.error = "Failed to update"; }
                }
            }
            catch { }
            return View(ad);
        }
        public ActionResult AddSongType()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            return View(db.AudioTypes.OrderBy(a=>a.AudioType1).ToList());
        }
        [HttpPost]
        public ActionResult AddSongType(string type)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                AudioType at = new AudioType();
                at.AudioType1 = type;
                db.AudioTypes.Add(at);
                int i = db.SaveChanges();
                if (i > 0)
                { return RedirectToAction("SongCreate"); }
                else
                { ViewBag.error = "Filed to add song type"; }
            }
            catch { ViewBag.error = "Technical Error"; }
            return View();
        }
        public ActionResult EditSongType(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            AudioType at=db.AudioTypes.Find(id);
            Session["AudioType_Id"]=id;
            if (at == null)
            {
                ViewBag.error = "Data Not Found | Technical error";
            }
            return View(at);
        }
        [HttpPost]
        public ActionResult EditSongType()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
                int id = Convert.ToInt32(Session["AudioType_Id"]);
                AudioType at = db.AudioTypes.Find(id);
                if (at != null)
                {
                    at.AudioType1 = Request["type"];
                    db.Entry(at).State = System.Data.Entity.EntityState.Modified;
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        return RedirectToAction("AddSongType");
                    }
                    else { ViewBag.error = "Failed to Update"; }
                }
            return View(at);
        }

        public ActionResult DeleteSongType(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            AudioType at = db.AudioTypes.Find(id);
            Session["AudioType_Id"] = id;
            if (at == null)
            {
                ViewBag.error = "Data Not Found | Technical error";
            }
            return View(at);
        }
        [HttpPost]
        public ActionResult DeleteSongType()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }

            int id = Convert.ToInt32(Session["AudioType_Id"]);
            AudioType at = db.AudioTypes.Find(id);
            
            if (at != null)
            {
                db.AudioTypes.Remove(at);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return RedirectToAction("AddSongType");
                }
                else { ViewBag.error = "Failed to Update"; }
            }
            return View(at);
        }
        public ActionResult SongInfo()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(db.Audios.OrderBy(a=>a.Title).ToList());
        }
        public ActionResult SongCreate()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AudioType = db.AudioTypes.OrderBy(a=>a.AudioType1).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult SongCreate(string title, string url, string SingerName, string ComposerName, string LyricsWriter, string MusicProduser, string MusicLabel)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Audio ad = new Audio();
                ad.AudioUrl = "";
                ad.AudioType_Id = Convert.ToInt32(Request["AudioType_Id"]);
                ad.SingerName = SingerName;
                ad.ComposerName = ComposerName;
                ad.LyricsWriter = LyricsWriter;
                ad.MusicProducer = MusicProduser;
                ad.MusicLabel = MusicLabel;
                HttpPostedFileBase file = Request.Files["url"];
                if (file.FileName.Length > 0)
                {
                    string filename = "Song_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_") + file.FileName;
                    string ext = System.IO.Path.GetExtension(filename);
                    if (ext.ToLower() == ".mp3")
                    {
                        file.SaveAs(Server.MapPath("~/Uploaded/" + filename));
                        ad.Title = title;
                        ad.AudioUrl = filename;
                        ad.Created_On = DateTime.Now;
                        db.Audios.Add(ad);
                        int i = db.SaveChanges();
                        if (i > 0)
                        {
                            return RedirectToAction("SongInfo");
                        }
                        else
                        {
                            ViewBag.error = "Filed to upload song";
                        }
                    }
                    else { ViewBag.error = "Song format is not valid | It must be .mp3"; }

                }


            }
            catch { ViewBag.error = "Technical Error"; }
            return View();
        }

        public ActionResult SongView(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            Audio ad = db.Audios.Find(id);
            Session["Audio_Id"] = id;
            ViewBag.AudioType = db.AudioTypes.ToList();
            if (ad == null)
            { ViewBag.error = "Data Not Found | Technical error"; }


            return View(ad);
        }
        [HttpPost]
        public ActionResult SongView(string SingerName, string ComposerName, string LyricsWriter, string MusicProduser, string MusicLabel)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            int id = Convert.ToInt32(Session["Audio_Id"]);
            Audio ad = db.Audios.Find(id);
            ad.AudioType_Id = Convert.ToInt32(Request["AudioType_Id"]);
            if (ad != null)
            {
                ad.Title = Request["title"];

                HttpPostedFileBase file = Request.Files["url"];
                if (file.FileName.Length > 0)
                {
                    string filename = "Song_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_") + file.FileName;
                    string ext = Path.GetExtension(filename);
                    if (ext.ToLower() == ".mp3")
                    {
                        try
                        {
                            System.IO.File.Delete(Server.MapPath("~/Uploaded/" + ad.AudioUrl));
                        }
                        catch { }
                        file.SaveAs(Server.MapPath("~/Uploaded/" + filename));
                        ad.AudioUrl = filename;

                    }
                    else { ViewBag.error = "Song format is not valid | It must be .mp3"; }

                }
                ad.Modified_On = DateTime.Now;
                ad.SingerName = SingerName;
                ad.ComposerName = ComposerName;
                ad.LyricsWriter = LyricsWriter;
                ad.MusicProducer = MusicProduser;
                ad.MusicLabel = MusicLabel;
                db.Entry(ad).State = System.Data.Entity.EntityState.Modified;
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return RedirectToAction("SongInfo");
                }
                else
                {
                    ViewBag.error = "Filed to upload song";
                }
            }
            return View(ad);
        }
        public ActionResult SongDelete(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            if (id != null)
            {
                Audio ad = db.Audios.Find(id);
                Session["Audio_Id"] = id;
                ViewBag.AudioTypeName = db.Audios.Find(id).AudioType.AudioType1;
                if (ad == null)
                {
                    ViewBag.error = "Data Not Found | Technical error";

                }
                return View(ad);
            }
            else { ViewBag.error = "Technical error"; }

            return RedirectToAction("SongInfo");
        }
        [HttpPost]
        public ActionResult SongDelete()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            int id = Convert.ToInt32(Session["Audio_Id"]);

            Audio ad = db.Audios.Find(id);
            if (ad != null)
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath("~/Uploaded/" + ad.AudioUrl));
                }
                catch { }
                db.Audios.Remove(ad);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return RedirectToAction("SongInfo");
                }
                else
                { ViewBag.error = ""; }
            }
            return View(ad);
        }
        public ActionResult VideoInfo()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            return View(db.Videos.OrderBy(a=>a.Title).ToList());
        }
        public ActionResult AddVideoType()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            return View(db.VideoTypes.OrderBy(a=>a.VideoType1).ToList());
        }
        [HttpPost]
        public ActionResult AddVideoType(string type)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                VideoType vt = new VideoType();
                vt.VideoType1 = type;
                db.VideoTypes.Add(vt);
                int i = db.SaveChanges();
                if (i > 0)
                { return RedirectToAction("VideoCreate"); }
                else
                { ViewBag.error = "Filed to add song type"; }
            }
            catch
            {
                ViewBag.error = "Technical Error";
            }
            return View();
        }


        public ActionResult EditVideoType(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            VideoType vt = db.VideoTypes.Find(id);
            Session["VideoType_Id"] = id;
            if (vt == null)
            {
                ViewBag.error = "Data Not Found | Technical error";
            }
            return View(vt);
        }
        [HttpPost]
        public ActionResult EditVideoType()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }

            int id = Convert.ToInt32(Session["VideoType_Id"]);
            VideoType vt = db.VideoTypes.Find(id);
            if (vt != null)
            {
                vt.VideoType1 = Request["type"];
                db.Entry(vt).State = System.Data.Entity.EntityState.Modified;
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return RedirectToAction("AddVideoType");
                }
                else { ViewBag.error = "Failed to Update"; }
            }
            return View(vt);
        }

        public ActionResult DeleteVideoType(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            VideoType vt = db.VideoTypes.Find(id);
            Session["VideoType_Id"] = id;
            if (vt == null)
            {
                ViewBag.error = "Data Not Found | Technical error";
            }
            return View(vt);
        }
        [HttpPost]
        public ActionResult DeleteVideoType()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            int id = Convert.ToInt32(Session["VideoType_Id"]);
            AudioType at = db.AudioTypes.Find(id);
            VideoType vt = db.VideoTypes.Find(id);
            if (vt != null)
            {
                db.VideoTypes.Remove(vt);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return RedirectToAction("AddVideoType");
                }
                else { ViewBag.error = "Failed to Update"; }
            }
            return View(at);
        }
        public ActionResult VideoCreate()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.VideoType = db.VideoTypes.OrderBy(a=>a.VideoType1).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult VideoCreate(string url, string title, string SingerName, string ComposerName, string LyricsWriter, string MusicProduser, string MusicLabel)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Video vd = new Video();
                vd.Title = title;
                vd.VideoUrl = url;
                vd.Created_On = DateTime.Now;
                vd.SingerName = SingerName;
                vd.ComposerName = ComposerName;
                vd.LyricsWriter = LyricsWriter;
                vd.MusicLabel = MusicLabel;
                vd.MusicProducer = MusicProduser;
                vd.VideoType_Id =Convert.ToInt32( Request["VideoType_Id"]);
                db.Videos.Add(vd);
                int i=db.SaveChanges();
                if (i > 0)
                { return RedirectToAction("VideoInfo"); }
                else
                { ViewBag.error = "Filed to upload Video"; }

            }
            catch { }
            return View();
        }
        public ActionResult VideoView(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            Video vd = db.Videos.Find(id);
            ViewBag.VideoType = db.VideoTypes.ToList();
            Session["Video_Id"] = id;
            if (vd == null)
            {
                ViewBag.error = "Data Not Found | Technical error";

            }
            return View(vd);
        }
        [HttpPost]
        public ActionResult VideoView(string SingerName, string ComposerName, string LyricsWriter, string MusicProduser, string MusicLabel)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
                int id = Convert.ToInt32(Session["Video_Id"]);
                Video vd = db.Videos.Find(id);
                try
                {
                    if (vd != null)
                    {
                        vd.Modified_On = DateTime.Now;
                        vd.Title = Request["title"];
                        vd.VideoUrl = Request["url"];
                        vd.VideoType_Id = Convert.ToInt32(Request["VideoType_Id"]);
                        vd.SingerName = SingerName;
                        vd.ComposerName = ComposerName;
                        vd.LyricsWriter = LyricsWriter;
                        vd.MusicLabel = MusicLabel;
                        vd.MusicProducer = MusicProduser;
                        db.Entry(vd).State = System.Data.Entity.EntityState.Modified;
                        int i = db.SaveChanges();
                        if (i > 0)
                        { return RedirectToAction("VideoInfo"); }
                        else
                        { return View(vd); }
                    }
                }
                catch { }
            return View(vd);
        }
        public ActionResult VideoDelete(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }           

            Video vd = db.Videos.Find(id);
            Session["Video_Id"] = id;
            ViewBag.VideoTypeName = db.Videos.Find(id).VideoType.VideoType1;
            if (vd == null)
            {
                ViewBag.error = "Data Not Found | Technical error";
            }
            return View(vd);
        }
        [HttpPost]
        public ActionResult VideoDelete()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            int id = Convert.ToInt32(Session["Video_Id"]);
            Video vd = db.Videos.Find(id);
            if (vd != null)
            {
                
                db.Videos.Remove(vd);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return RedirectToAction("VideoInfo");
                }
                else
                { ViewBag.error = ""; }
            }
            return View(vd);
        }

        public ActionResult AlbumCreate()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AlbumCreate(string title,string url)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                PhotoAlbum pa=new PhotoAlbum();
                pa.CoverPhoto = "";
                HttpPostedFileBase file = Request.Files["url"];
                if (file.FileName.Length > 0)
                {
                    string filename = "Album_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_") + file.FileName;
                    string ext = System.IO.Path.GetExtension(filename);
                    if (ext.ToLower() == ".jpg" || ext.ToLower()=="jpeg")
                    {
                        file.SaveAs(Server.MapPath("~/Uploaded/" + filename));
                        pa.Title = title;
                        pa.CoverPhoto = filename;
                        pa.Created_On = DateTime.Now;
                        db.PhotoAlbums.Add(pa);
                        int i = db.SaveChanges();
                        if (i > 0)
                        {
                            return RedirectToAction("AlbumInfo");
                        }
                        else
                        {
                            ViewBag.error = "Filed to upload album";
                        }
                    }
                    else { ViewBag.error = "Image format is not valid | It must be .jpg or .jpeg"; }

                }


            }
            catch { ViewBag.error = "Technical Error"; }
            return View();
        }
        public ActionResult AlbumInfo()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            return View(db.PhotoAlbums.ToList());
        }
        public ActionResult AlbumDetail(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            Session["PhotoAlbum_Id"] = id;
            return View(db.PhotoAlbums.Find(id).PhotoAlbumDetails.ToList());
        }
        
        public ActionResult AlbumPhotoDelete(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                PhotoAlbumDetail pad = db.PhotoAlbumDetails.Find(id);
                if (pad != null)
                {
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Uploaded/" + pad.Photo));
                    }
                    catch { }
                    db.PhotoAlbumDetails.Remove(pad);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        return RedirectToAction("AlbumDetail", new { id = Session["PhotoAlbum_Id"] });
                    }
                    else
                    { ViewBag.error = "Failed to delete photo"; }
                }
            }
            catch { ViewBag.error = "Technical Error"; }
            return View();
        }
        public ActionResult AddPhotos()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
           return View();
        }
        [HttpPost]
        public ActionResult AddPhotos(int id=0)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                PhotoAlbumDetail pad = new PhotoAlbumDetail();
                pad.Photo = "";
                pad.PhotoAlbum_Id = Convert.ToInt32(Session["PhotoAlbum_Id"]);
                HttpPostedFileBase file = Request.Files["url"];
                if (file.FileName.Length > 0)
                {
                    string filename = "Image_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_") + file.FileName;
                    string ext = System.IO.Path.GetExtension(filename);
                    if (ext.ToLower() == ".jpg" || ext.ToLower() == "jpeg")
                    {
                        file.SaveAs(Server.MapPath("~/Uploaded/" + filename));
                        pad.Photo = filename;
                        db.PhotoAlbumDetails.Add(pad);
                        int i = db.SaveChanges();
                        if (i > 0)
                        {
                            return RedirectToAction("AlbumDetail", new { id = Session["PhotoAlbum_Id"] });
                        }
                        else
                        {
                            ViewBag.error = "Filed to upload photo";
                        }
                    }
                    else { ViewBag.error = "Image format is not valid | It must be .jpg or .jpeg"; }

                }
            }
            catch { ViewBag.error = "Technical Error"; }
            return View();
        }
        public ActionResult AlbumView(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            Session["PhotoAlbum_Id"] = id;
            PhotoAlbum pa = db.PhotoAlbums.Find(id);
            if (pa == null)
            {
                ViewBag.error = "Data Not Found | Technical error";

            }
            return View(pa);
        }

        [HttpPost]
        public ActionResult AlbumView()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            int id = Convert.ToInt32(Session["PhotoAlbum_Id"]);
            PhotoAlbum pa = db.PhotoAlbums.Find(id);
            if (pa != null)
            {
                pa.Title = Request["title"];

                HttpPostedFileBase file = Request.Files["url"];
                if (file.FileName.Length > 0)
                {
                    string filename = "Album_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_") + file.FileName;
                    string ext = Path.GetExtension(filename);
                    if (ext.ToLower() == ".jpg" || ext.ToLower() == "jpeg")
                    {
                        try
                        {
                            System.IO.File.Delete(Server.MapPath("~/Uploaded/" + pa.CoverPhoto));
                        }
                        catch { }
                        file.SaveAs(Server.MapPath("~/Uploaded/" + filename));
                        pa.CoverPhoto = filename;

                    }
                    else { ViewBag.error = "Image format is not valid | It must be .jpg or .jpeg"; }

                }
                pa.Modified_On = DateTime.Now;
                db.Entry(pa).State = System.Data.Entity.EntityState.Modified;
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return RedirectToAction("AlbumInfo");
                }
                else
                {
                    ViewBag.error = "Filed to upload album";
                }  
            }
            return View(pa);
        }
        public ActionResult AlbumDelete(int id)
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            PhotoAlbum pa = db.PhotoAlbums.Find(id);
            Session["PhotoAlbum_Id"] = id;
            if(pa==null)
            { ViewBag.error = "Data Not Found | Technical error"; }
            return View(pa);
        }
        [HttpPost]
        public ActionResult AlbumDelete()
        {
            if (Session["Admin_Id"] == null)
            {
                return RedirectToAction("Index");
            }
            int id = Convert.ToInt32(Session["PhotoAlbum_Id"]);
            PhotoAlbum pa = db.PhotoAlbums .Find(id);
            try
            {
               
                if (pa != null)
                {
                    foreach (var item in pa.PhotoAlbumDetails.ToList())
                    {
                        try
                        {
                            System.IO.File.Delete(Server.MapPath("~/Uploaded/" + item.Photo));
                        }
                        catch { }
                        db.PhotoAlbumDetails.Remove(item);
                        db.SaveChanges();
                    }
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Uploaded/" + pa.CoverPhoto));
                    }
                    catch { }
                    db.PhotoAlbums .Remove(pa);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        return RedirectToAction("AlbumInfo", new { id = Session["PhotoAlbum_Id"] });
                    }
                    else
                    { ViewBag.error = "Failed to delete album"; }
                }
            }
            catch { ViewBag.error = "Technical Error"; }
            return View(pa);
        }

        public ActionResult Logout()
        {
            Session["Admin_Id"] = null;
            return RedirectToAction("Home", "VeeralPatel");
        }
    }
}