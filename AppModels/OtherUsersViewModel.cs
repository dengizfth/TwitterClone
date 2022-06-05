using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IskurTwitterApp.Models;

namespace IskurTwitterApp.AppModels
{
    public class OtherUsersViewModel
    {
        /*
         
                        Diğer Kullanıcıların profil sayfalarını ziyaret etmek içi current ve session'lara erişip
                    yeni bir page'de açılmalı.

                    Not : Ürünler sayfasındaki ürün detay mantığında 
         
         */


        public User currentUser { get; set; }
        public User sessionUser { get; set; }
    }
}