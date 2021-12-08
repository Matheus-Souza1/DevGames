using DevGames.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevGames.API.Persistence
{
    public class DevGamesContext : DbContext
    {
        public DevGamesContext(DbContextOptions<DevGamesContext> options) : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Board>()
                .HasKey(b => b.Id);

            builder.Entity<Board>()
                .HasMany(b => b.Posts)
                .WithOne()
                .HasForeignKey(b => b.BoardId);

            builder.Entity<Post>()
                .HasKey(p => p.Id);

            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne()
                .HasForeignKey(p => p.PostId);

            builder.Entity<Comment>()
                .HasKey(c => c.Id);
        }
    }
}
