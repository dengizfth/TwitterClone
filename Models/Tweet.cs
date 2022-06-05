using System;
using System.Collections.Generic;

namespace IskurTwitterApp.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string TweetText { get; set; }
        public string TwitImage { get; set; }
        public string TweetOwnerName { get; set; }
        public DateTime SendTime { get; set; }
        public List<Like> Likes { get; set; }
        public List<Retweet> Retweet { get; set; }
        public List<Comment> Comment { get; set; }

       
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsRetweet { get; set; }
    }
}