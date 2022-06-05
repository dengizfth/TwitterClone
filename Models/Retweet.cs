﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IskurTwitterApp.Models
{
    public class Retweet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }

    }
}
