using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IskurTwitterApp.AppModels;
using IskurTwitterApp.Models;

namespace IskurTwitterApp.Controllers
{


    // AdMin Hesap Oluşturma ve Giriş Yapma işlemi

    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }



        public ActionResult Dashport()
        {
            dataContext db = new dataContext();
            DaschboardViewModel DVM = new DaschboardViewModel();

            DVM.UserCount = db.Users.Count();
            DVM.TweetCount = db.Tweets.Count();
            DVM.RetweetCount = db.Retweets.Count();
            DVM.LikeCount = db.Likes.Count();
            DVM.CommentCount = db.Comments.Count();





            return View(DVM);
        }


    }
}