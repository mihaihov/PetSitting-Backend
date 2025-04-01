using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.PostManagement
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public required string AuthorId { get; set; }
        public required ApplicationUser Author { get; set; }

        // Discriminator to differentiate post types
        public PostType PostType { get; set; }
    }

    public enum PostType
    {
        General,
        Job
    }
}
