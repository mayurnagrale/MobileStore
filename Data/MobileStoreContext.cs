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
            optionsBuilder.UseSqlServer(_config.GetConnectionString("MobileStoreCS"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationship between user and role
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
            modelBuilder.Entity<SaleRecord>().Property(s => s.SaleId).ValueGeneratedOnAdd();

        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            // Query the database for the user based on the provided username and password
            var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            return user;
        }



        
    }
}
