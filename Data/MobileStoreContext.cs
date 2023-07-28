using Microsoft.EntityFrameworkCore;
using MobileStore.Models;

namespace MobileStore.Data
{
    public class MobileStoreContext : DbContext
    {
        protected readonly IConfiguration _config;
        public MobileStoreContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _config = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SaleRecord> SaleRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection here (e.g., SQL Server, SQLite, etc.)
            optionsBuilder.UseSqlServer(_config.GetConnectionString("MobileStoreCS"));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships between entities here
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    ur => ur.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    ur => ur.HasOne<User>().WithMany().HasForeignKey("UserId")
                );

            modelBuilder.Entity<User>()
            .HasMany(u => u.SaleRecords)
            .WithOne()
            .HasForeignKey(s => s.SaleId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SaleRecord>().HasKey(s => s.SaleId);
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            // Query the database for the user based on the provided username and password
            var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            return user;
        }



        
    }
}
