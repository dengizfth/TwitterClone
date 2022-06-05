using System;
using System.Web.Mvc;
using IskurTwitterApp.Models;
using IskurTwitterApp.AppModels;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.IO;


namespace IskurTwitterApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        dataContext db = new dataContext();

        public ActionResult Index()
        {
            var sessionId = Convert.ToInt32(User.Identity.Name);

            var sessionUser = db.Users.FirstOrDefault(x => x.UserId == sessionId);

            if (db.Users.Find(sessionId) != null)
            {
                var user = db.Users.Where(x => x.UserId == sessionId).Include("Tweets").Include("Followers").Include("Tweets.Likes").Include("Tweets.Comment").FirstOrDefault();
                var allUsers = db.Users.Select(x => x).Include("Tweets").Include("Followers").Include("Tweets.Likes").Include("Tweets.Comment").ToList(); //Db deki tüm userlar
                var otherUsers = allUsers.Where(x => x.UserId != sessionId).ToList();

                var notFollowingUsers = otherUsers.Where(x => !user.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim
                var _connectedUsers = allUsers.Where(x => !notFollowingUsers.Select(y => y.UserId).Contains(x.UserId)).ToList();  // Ben ve takip ettiklerim

                var vmHomePage = new HomepageViewModel()
                {
                    connectedUsers = _connectedUsers,
                    otherUsers = notFollowingUsers,
                    sessionUser = user
                };

                ViewBag.notFollowing = notFollowingUsers;

                return View(vmHomePage);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult Tweet(Tweet tweet, HttpPostedFileBase TwitPostImage)
        {
            var Id = Convert.ToInt32(User.Identity.Name);
            var userName = db.Users.Find(Id).Username;

            string imagepath = "";
            string imagename = "";


            if (db.Users.Any(x => x.Username == userName))  //İstenen kayıt varsa true yoksa false döndürür
            {
                var user = db.Users.FirstOrDefault(x => x.Username == userName);
                tweet.SendTime = DateTime.Now;
                tweet.UserId = user.UserId;

                try
                {
                    if (TwitPostImage != null && TwitPostImage.ContentLength > 0)
                    {


                        imagename = Guid.NewGuid().ToString().Substring(0, 10) + "-" + Path.GetFileName(TwitPostImage.FileName);
                        imagepath = Path.Combine(Server.MapPath("~/Content/images/timg"), imagename);
                        TwitPostImage.SaveAs(imagepath);

                        //user.Image = imagename;
                        tweet.TwitImage = imagename;

                        ViewBag.mesaj = "Tweeet Resim Güncelleme Başarılı";
                    }
                }
                catch
                {
                    ViewBag.mesaj = " Tweeet Beklenmedik Bir Hata Oluştu";
                }




                db.Tweets.Add(tweet);
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Profile()
        {
            var sessionId = Convert.ToInt32(User.Identity.Name);
            var sessionUser = db.Users.FirstOrDefault(x => x.UserId == sessionId);

            var myTweets = db.Tweets.Where(x => x.TweetId == sessionId).ToList();

            var currentUser = db.Users   //Tıkladığım kişinin bilgileri eager loading
               .Where(x => x.UserId == sessionId)
               .Include("Tweets")
               .Include("Followers")
               .Include("Tweets.Likes")
               .FirstOrDefault();


            var followers = db.Followers.Where(x => x.FollowId == currentUser.UserId).Include("User").ToList();

            ViewBag.followers = followers.Count;


            var allUsers = db.Users.Select(x => x)
                .Include("Tweets")
                .Include("Followers")
                .Include("Tweets.Likes").ToList(); //Db deki tüm userlar    

            var otherUsers = allUsers.Where(x => x.UserId != sessionId).ToList();


            var notFollowingUsers = otherUsers.Where(x => !sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim

            ViewBag.notFollowing = notFollowingUsers;

            var usrvm = new UserViewModel()
            {
                CurrentUser = currentUser
            };


            return View(usrvm);
        }

        [HttpPost]
        public ActionResult Profile(UserViewModel usr)
        {
            var sessionId = Convert.ToInt32(User.Identity.Name);
            var sessionName = db.Users.Find(sessionId).Username;

            User user = new User();
            user.Name = usr.Name;

            if (usr.Username == sessionName)
            {
                return RedirectToAction("Profile");
            }
            else
            {
                var selectedUser = db.Users.FirstOrDefault(x => x.Username == sessionName);
                var myTweets = db.Tweets.Where(x => x.UserId == selectedUser.UserId).ToList();
                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(UserViewModel usr, HttpPostedFileBase ProfilePicture)
        {
            int sessionId = Convert.ToInt32(User.Identity.Name);

            var sessionUser = db.Users.Where(x => x.UserId == sessionId).FirstOrDefault();

            if (sessionId > 0)
            {
                sessionUser.Name = usr.Name;
                sessionUser.PersonalInformation = usr.PersonalInformation;
                sessionUser.Location = usr.Location;
                sessionUser.WebPage = usr.WebPage;
                sessionUser.BirthDate = usr.BirthDate;

                string imagepath = "";
                string imagename = "";

                try
                {
                    if (ProfilePicture != null && ProfilePicture.ContentLength > 0)
                    {


                        imagename = Guid.NewGuid().ToString().Substring(0, 10) + "-" + Path.GetFileName(ProfilePicture.FileName);
                        imagepath = Path.Combine(Server.MapPath("~/Content/images"), imagename);
                        ProfilePicture.SaveAs(imagepath);

                        //user.Image = imagename;
                        sessionUser.ProfileImage = imagename;

                        ViewBag.mesaj = "Resim Güncelleme Başarılı";
                    }
                }
                catch
                {
                    ViewBag.mesaj = "Beklenmedik Bir Hata Oluştu";
                }
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            else
            {
                return RedirectToAction("login");
            }

;


        }

        #region Follow

        [HttpPost]
        public ActionResult Follow(User user)
        {
            Follower follower = new Follower();
            follower.UserId = Convert.ToInt32(User.Identity.Name);
            follower.FollowId = user.UserId;
            db.Followers.Add(follower);
            db.SaveChanges();

            return RedirectToAction("Followers");
        }

        [HttpPost]
        public ActionResult UnFollow(User _clickedUser)
        {
            var sessionId = Convert.ToInt32(User.Identity.Name);
            var sessionUser = db.Users.Find(sessionId);
            var deleteFollow = db.Followers.FirstOrDefault(x => x.UserId == sessionId && x.FollowId == _clickedUser.UserId);
            db.Followers.Remove(deleteFollow);
            db.SaveChanges();
            return RedirectToAction("Followers");
        }

        public ActionResult Followers()
        {
            var sessionId = Convert.ToInt32(User.Identity.Name);
            var sessionUser = db.Users.Where(x => x.UserId == sessionId).Include("Tweets").Include("Followers").Include("Tweets.Likes").FirstOrDefault();

            var allUsers = db.Users.Select(x => x).Include("Tweets").Include("Followers").Include("Tweets.Likes").ToList(); //Db deki tüm userlar            
            var otherUsers = allUsers.Where(x => x.UserId != sessionId).ToList();

  

            var notFollowingUsers = otherUsers.Where(x => !sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim
            var followerUsers = otherUsers.Where(x => db.Followers.Select(y => y.Id).Contains(x.UserId)).ToList();
            var followingUsers = otherUsers.Where(x => sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList();  // Ben ve takip ettiklerim

            ViewBag.notFollowing = notFollowingUsers;
            ViewBag.following = followingUsers;
            ViewBag.follower = followerUsers;

            return View(sessionUser);
        }
        #endregion

        [HttpGet]
        public ActionResult OtherProfile(int profileUserId)
        {

            var sessionId = Convert.ToInt32(User.Identity.Name);

            var _currentUser = db.Users.Where(x => x.UserId == profileUserId).Include("Tweets").Include("Followers").FirstOrDefault();
            var _sessionUser = db.Users.Where(x => x.UserId == sessionId).Include("Followers").FirstOrDefault();

            var vmOther = new OtherUsersViewModel()
            {
                sessionUser = _sessionUser,
                currentUser = _currentUser
            };

            var followingUser = _currentUser.Followers.Where(x => x.UserId == _currentUser.UserId).ToList();
            var followers = db.Followers.Where(x => x.FollowId == _currentUser.UserId).Include("User").ToList();

            ViewBag.followers = followers.Count;

            var allUsers = db.Users.Select(x => x).Include("Tweets").Include("Followers").Include("Tweets.Likes").ToList(); //Db deki tüm userlar
            var otherUsers = allUsers.Where(x => x.UserId != sessionId).ToList();
            var notFollowingUsers = otherUsers.Where(x => !_sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim

            ViewBag.notFollowing = notFollowingUsers;

            var followStatus = _sessionUser.Followers.Where(x => x.FollowId == _currentUser.UserId).ToList();
            if (followStatus.Count == 0)
            {
                ViewBag.followStatus = false;
            }
            else
            {
                ViewBag.followStatus = true;
            }
            return View(vmOther);
        }

        [HttpPost]
        public ActionResult OtherProfile(User usr)
        {

            var sessionId = Convert.ToInt32(User.Identity.Name);

            if (usr.UserId == sessionId)
            {
                return RedirectToAction("Profile");
            }
            else
            {

                //Tıkladığım kişinin bilgileri eager loading
                var _currentUser = db.Users.Where(x => x.UserId == usr.UserId).Include("Tweets").Include("Followers").FirstOrDefault();

                var _sessionUser = db.Users.Where(x => x.UserId == sessionId).Include("Followers").FirstOrDefault();

                var vmOther = new OtherUsersViewModel()
                {
                    sessionUser = _sessionUser,
                    currentUser = _currentUser
                };

                var followingUser = _currentUser.Followers.Where(x => x.UserId == _currentUser.UserId).ToList();

                var followers = db.Followers.Where(x => x.FollowId == _currentUser.UserId).Include("User").ToList();

                ViewBag.followers = followers.Count;

                var allUsers = db.Users.Select(x => x).Include("Tweets").Include("Followers").Include("Tweets.Likes").ToList(); //Db deki tüm userlar

                var otherUsers = allUsers.Where(x => x.UserId != sessionId).ToList();

                var notFollowingUsers = otherUsers.Where(x => !_sessionUser.Followers.Select(y => y.FollowId).Contains(x.UserId)).ToList(); //Takip etmediklerim

                ViewBag.notFollowing = notFollowingUsers;

                var followStatus = _sessionUser.Followers.Where(x => x.FollowId == _currentUser.UserId).ToList();

                if (followStatus.Count == 0)
                {
                    ViewBag.followStatus = false;
                }
                else
                {
                    ViewBag.followStatus = true;
                }
                return View(vmOther);
            }
        }

        [HttpPost]
        public ActionResult Like(Tweet twt)
        {
            var userId = Convert.ToInt32(User.Identity.Name);

            if (db.Likes.Any(x => x.UserId == userId && x.TweetId == twt.TweetId))
            {
                var like = db.Likes.SingleOrDefault(x => x.UserId == userId && x.TweetId == twt.TweetId);
                db.Likes.Remove(like);
                db.SaveChanges();
            }
            else
            {
                Like like = new Like();
                like.TweetId = twt.TweetId;
                like.UserId = (int)userId;
                db.Likes.Add(like);
                db.SaveChanges();
            }
            if (Request.UrlReferrer.Segments.Length == 3)
            {
                if (Request.UrlReferrer.Segments[2] == "OtherProfile")
                {
                    return RedirectToAction(Request.UrlReferrer.Segments[2], "Home", new { profileUserId = twt.UserId });
                }
                else
                {
                    return RedirectToAction(Request.UrlReferrer.Segments[2], "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Retweet(int TweetId)
        {
            var sessionId = Convert.ToInt32(User.Identity.Name); // retweetYapanUserId

            // Eğer önceden retweet yapıldıysa tekrar yapılmasın !
            if (!db.Retweets.Any(x => x.UserId == sessionId && x.TweetId == TweetId))
            {
                // ReTweet log
                Retweet rtw = new Retweet();
                rtw.TweetId = TweetId;
                rtw.UserId = sessionId;
                db.Retweets.Add(rtw);

                // ReTweet ekle
                var reTweetItem = db.Tweets.FirstOrDefault(x => x.TweetId == TweetId);
                var reTweetItemUser = db.Users.FirstOrDefault(x => x.UserId == reTweetItem.UserId);
                Tweet newReTweet = new Tweet();
                newReTweet.TweetText = reTweetItem.TweetText;
                newReTweet.TwitImage = reTweetItem.TwitImage;
                newReTweet.SendTime = DateTime.Now;
                newReTweet.UserId = sessionId;
                newReTweet.TweetOwnerName = reTweetItemUser.Name;
                newReTweet.IsRetweet = true;
                db.Tweets.Add(newReTweet);

                db.SaveChanges();

            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult Comment(Comment cmt)
        {
            var sessionId = Convert.ToInt32(User.Identity.Name); // modelden gelicek

            // comment ekle
            Comment newComment = new Comment();
            newComment.TweetId = cmt.TweetId;
            newComment.UserId = sessionId;
            newComment.CommentText = cmt.CommentText;
            db.Comments.Add(newComment);

           

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}