using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IskurTwitterApp.Models
{

   // Yetkili Tablosu : Admin Panel
    public class Official
    {
        // Admin Panaline Giriş Yapan Kullanıcılar için ayrı tablo
        public int Id { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }     
        public List<Tweet> Tweets { get; set; }



    }
}