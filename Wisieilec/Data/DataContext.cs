using Microsoft.EntityFrameworkCore;
using Wisieilec.Data.Entities;

namespace Wisieilec.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lobby>()
                .HasMany(l => l.Users)
                .WithOne(u => u.Lobby)
                .HasForeignKey(u => u.LobbyId);
        }
    }
}