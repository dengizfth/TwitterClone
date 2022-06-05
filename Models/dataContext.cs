using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace IskurTwitterApp.Models
{
    public class dataContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public dataContext() : base("dataContext") { }

        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Retweet> Retweets { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
