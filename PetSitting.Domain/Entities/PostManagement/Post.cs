using PetSitting.Domain.Entities.NewsFeed;
using PetSitting.Domain.Entities.ReviewSystem;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.PostManagement
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public required string AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }

        public ICollection<Media>? MediaFiles { get; set; }

        public string? Status { get; set; } = "Active"; //can be "Active", "Inactive", "Archived", etc.

        // Discriminator to differentiate post types. Default value is assigned due to configuration in OnModelCreating.
        public PostType PostType { get; set; }

        //FK
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

    public enum PostType
    {
        General,
        Job
    }
}
