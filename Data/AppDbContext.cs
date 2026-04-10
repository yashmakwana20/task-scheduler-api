using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<TaskItems> TaskItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
