using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TODOBack.Models;

namespace TODOBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel>? Users { get; set; }
        public DbSet<TaskModel>? Tasks { get; set; }
        public DbSet<BillingModel>? Billings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("Users").HasKey(e => e.UserId);
            modelBuilder.Entity<TaskModel>().ToTable("Tasks").HasKey(e => e.TaskId);
            modelBuilder.Entity<BillingModel>().ToTable("Billings").HasKey(e => e.TxID);

            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.Tasks)
                .WithOne(e => e.Organizer)
                .HasForeignKey(e => e.OrganizerId)
                .HasPrincipalKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BillingModel>()
                .HasOne(e => e.User)
                .WithOne(e => e.Billing)
                .HasForeignKey<BillingModel>(e => e.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BillingModel>()
                .Property(e => e.Deadline)
                .HasColumnType("timestamp without time zone");
        }
    }
}
