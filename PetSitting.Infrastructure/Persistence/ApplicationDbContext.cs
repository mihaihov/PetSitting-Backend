using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetSitting.Domain.Entities.Messaging;
using PetSitting.Domain.Entities.NewsFeed;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.ReviewSystem;
using PetSitting.Domain.Entities.Security;
using PetSitting.Domain.Entities.Stripe;
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
        public DbSet<Message> Messages{get;set;}
        public DbSet<ChatSession> ChatSessions {get;set;}
        public DbSet<Review> Reviews {get;set;}
        public DbSet<StripeAccount> StripeAccounts {get;set;}
        public DbSet<StripeTransaction> StripeTransactions {get;set;}
        public DbSet<Feed> Feeds {get;set;}
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(au => au.UserProfile)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.ApplicationUserId)
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

            builder.Entity<Message>()
                .HasOne(m => m.ChatSession)
                .WithMany(cs => cs.Messages)
                .HasForeignKey(m => m.ChatSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Author)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StripeAccount>()
                .HasOne(sa => sa.ApplicationUser)
                .WithOne(au => au.StripeAccount)
                .HasForeignKey<StripeAccount>(au => au.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StripeTransaction>()
                .HasOne(st => st.StripeAccount)
                .WithMany(sa => sa.StripeTransactions)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<StripeTransaction>()
                .HasOne(st => st.JobPost)
                .WithMany(jp => jp.StripeTransactions)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StripeTransaction>()
                .Property(st => st.Amount)
                .HasColumnType("decimal(10,2)");

        }
    }
}