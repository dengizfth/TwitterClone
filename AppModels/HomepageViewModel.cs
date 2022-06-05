using IskurTwitterApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IskurTwitterApp.AppModels
{
    public class HomepageViewModel
    {
        
        public List<User> connectedUsers { get; set; }
        public List<User> otherUsers { get; set; }
        public User sessionUser { get; set; }

        public Retweet retweet { get; set; }
    }
}