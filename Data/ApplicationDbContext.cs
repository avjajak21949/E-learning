using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Demo03.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Demo03.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedUserRole(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<StudentClass>()
                .HasOne(sc => sc.Class)
                .WithMany(c => c.StudentClasses)
                .HasForeignKey(sc => sc.ClassID);

            modelBuilder.Entity<StudentClass>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentClasses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Course)
                .WithMany(co => co.Classes)
                .HasForeignKey(c => c.CourseID);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Course>()
                .HasOne(co => co.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(co => co.CategoryID);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Schedules)
                .HasForeignKey(s => s.ClassID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.UploadedBy)
                .WithMany()
                .HasForeignKey(d => d.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Class)
                .WithMany()
                .HasForeignKey(d => d.ClassID)
                .OnDelete(DeleteBehavior.Restrict);

            // Meeting configurations
            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Host)
                .WithMany()
                .HasForeignKey(m => m.HostUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Class)
                .WithMany(c => c.Meetings)
                .HasForeignKey(m => m.ClassID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Attendance relationships
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Class)
                .WithMany()
                .HasForeignKey(a => a.ClassID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Comment relationships
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Document)
                .WithMany(d => d.Comments)
                .HasForeignKey(c => c.DocumentID)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void SeedUserRole(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            var studentAccount = new IdentityUser
            {
                Id = "user1",
                UserName = "student@gmail.com",
                Email = "student@gmail.com",
                NormalizedUserName = "STUDENT@GMAIL.COM",
                NormalizedEmail = "STUDENT@GMAIL.COM",
                EmailConfirmed = true
            };

            var managerAccount = new IdentityUser
            {
                Id = "user2",
                UserName = "manager@gmail.com",
                Email = "manager@gmail.com",
                NormalizedUserName = "MANAGER@GMAIL.COM",
                NormalizedEmail = "MANAGER@GMAIL.COM",
                EmailConfirmed = true
            };

            var teacherAccount = new IdentityUser
            {
                Id = "user3",
                UserName = "teacher@gmail.com",
                Email = "teacher@gmail.com",
                NormalizedUserName = "TEACHER@GMAIL.COM",
                NormalizedEmail = "TEACHER@GMAIL.COM",
                EmailConfirmed = true
            };

            // Hash passwords
            studentAccount.PasswordHash = hasher.HashPassword(studentAccount, "123456");
            managerAccount.PasswordHash = hasher.HashPassword(managerAccount, "123456");
            teacherAccount.PasswordHash = hasher.HashPassword(teacherAccount, "123456");

            // Add users to database
            builder.Entity<IdentityUser>().HasData(
                studentAccount,
                managerAccount,
                teacherAccount
            );

            // Define roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "role1",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                },
                new IdentityRole
                {
                    Id = "role2",
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole
                {
                    Id = "role3",
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                }
            );

            // Assign roles to users
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "user1",
                    RoleId = "role1"  // Student
                },
                new IdentityUserRole<string>
                {
                    UserId = "user2",
                    RoleId = "role2"  // Manager
                },
                new IdentityUserRole<string>
                {
                    UserId = "user3",
                    RoleId = "role3"  // Teacher
                }
            );
        }
    }
}
