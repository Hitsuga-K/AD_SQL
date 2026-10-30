using Microsoft.EntityFrameworkCore;
using College_DB.Models;

namespace WebApplication1.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options)
            : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Group> Groups => Set<Group>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(e =>
            {
                e.ToTable("Students", "dbo");

                e.HasKey(s => s.StudentId);

                e.Property(s => s.StudentId)
                    .HasColumnName("Id");

                e.Property(s => s.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(s => s.LastName)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            modelBuilder.Entity<Group>(e =>
            {
                e.ToTable("Groups", "dbo");

                e.HasKey(g => g.GroupId);

                e.Property(g => g.GroupName)
                    .HasMaxLength(20)
                    .IsRequired();
            });
        }
    }
}