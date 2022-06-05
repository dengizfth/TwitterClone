using IskurTwitterApp.Models;
using System.Web.Mvc;
using System.Linq;
using System;
using System.Web.Security;

namespace IskurTwitterApp.Controllers
{
    public class LoginController : Controller
    {

        dataContext db = new dataContext();

        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string kullanigirisadi,string kullanicigirissifre)
        {
            var userController = db.Users.FirstOrDefault(username => username.Username == kullanigirisadi);
            if (userController != null)
            {
                FormsAuthentication.SetAuthCookie(userController.UserId.ToString(), false);
                ViewBag.mesaj = "Giriş Başarılı";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.mesaj = "Giriş Bilgileri Uyuşmuyor";
                return View();
            }
          
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(User user,string repeat)
        {
            var EMailController = db.Users.FirstOrDefault(m => m.Email == user.Email);

            if (EMailController == null )
            {

                User newuser = new User();

                if (user.Password == repeat)
                {
                   
                    newuser = user;
                    newuser.RegistrationDate = DateTime.Now;
                    newuser.ProfileImage = "default_profile.png";
                    db.Users.Add(newuser);
                    db.SaveChanges();

                    ViewBag.Mesaj = user.Email + "Kayıt Başarılı ile Gerçekleşti ";
                    //    //dbModelBuilder.Entity<User>().Property(x => x.RegistrationDate).HasDefaultValue(DateTime.Now);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Mesaj = "Şifreler Uyuşmuyor";
                }

            }
            else
            {
                ViewBag.Mesaj = user.Email+ "Mail Adresi Kayıtlı ";
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Login/Index");
        }

    }
}