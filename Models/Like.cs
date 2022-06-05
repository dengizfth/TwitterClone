using System;

namespace IskurTwitterApp.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
