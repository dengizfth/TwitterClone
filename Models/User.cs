using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IskurTwitterApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PersonalInformation { get; set; }
        public string Location { get; set; }
        public string WebPage { get; set; }
        public string ProfileImage { get; set; }
        public string BannerImage { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Tweet> Tweets { get; set; }
        public List<Follower> Followers { get; set; }
        public List<Like> Likes { get; set; }
    }
}