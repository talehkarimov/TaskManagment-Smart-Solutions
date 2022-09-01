using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagment.Models;

namespace TaskManagment.Data
{
    public sealed class AppDbContext : IdentityDbContext
    {
        public IConfiguration Configuration;

        public new DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<TaskManagment.Models.Task> Tasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            var dbConnection = Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(dbConnection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserTask>()
               .HasKey(ut => new { ut.TaskId, ut.ApplicationUserId });
            builder.Entity<UserTask>()
                .HasOne(ut => ut.ApplicationUser)
                .WithMany(a => a.UserTasks)
                .HasForeignKey(i => i.ApplicationUserId);
            builder.Entity<UserTask>()
                .HasOne(ut => ut.Task)
                .WithMany(t => t.UserTasks)
                .HasForeignKey(t => t.TaskId);
        }
    }
}
