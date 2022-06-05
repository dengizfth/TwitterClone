using System.ComponentModel.DataAnnotations;

namespace IskurTwitterApp.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public string CommentText { get; set; }

        // Yapılan Yoruma Yorum Yapılmış mı ?
        //false | 1 boşluk sola Atılan tweete yorum yapılmış gibi gözüksün, ana yorumlara ekle
        //true || 2 boşluk kadar sola kaydır. Yoruma yapılan Yorum gibi gözüksük
        //public bool Comcom { get; set; }

        // Kimin tweetin altında yorum yapıyorsa yakalasın.
        public int UserId { get; set; }

        public User User { get; set; }

        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }

        //public int CommentId { get; set; }
        //public Comment Comment { get; set; }
    }
}