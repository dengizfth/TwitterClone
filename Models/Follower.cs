using System;

namespace IskurTwitterApp.Models
{
    public class Follower
    {
        public int Id { get; set; }
        public int FollowId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
