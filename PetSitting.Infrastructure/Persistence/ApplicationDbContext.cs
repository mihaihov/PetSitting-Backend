using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.Security;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Infrastructure
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile>? UserProfiles {get;set;}
        public DbSet<UserSettings>? UserSettings {get;set;}
        public DbSet<RefreshToken>? RefreshTokens {get;set;}
        public DbSet<Post>? Posts {get;set;}
        public DbSet<JobPost> JobPosts {get;set;}
        public DbSet<JobApplication> JobApplications {get;set;}
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserSettings>()
                .HasOne(us => us.User)
                .WithOne()
                .HasForeignKey<UserSettings>(us => us.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .HasDiscriminator<PostType>("PostType")
                .HasValue<Post>(PostType.General)
                .HasValue<JobPost>(PostType.Job);

            builder.Entity<JobPost>()
                .HasMany(jp => jp.Applications)
                .WithOne(ja => ja.JobPost)
                .HasForeignKey(ja => ja.JobPostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobPost>()
                .Property(jp => jp.Payment)
                .HasColumnType("decimal(10,2)");

            builder.Entity<ApplicationUser>()
                .HasMany(au => au.Posts)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobApplication>()
                .HasOne(ja => ja.Applicant)
                .WithMany()
                .HasForeignKey(ja => ja.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Media>()
                .HasOne(m => m.Post)
                .WithMany(p => p.MediaFiles)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}