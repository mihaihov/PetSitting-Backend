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
        public required ApplicationUser Author { get; set; }

        public ICollection<Media>? MediaFiles {get;set;}
        // Discriminator to differentiate post types
        public PostType PostType { get; set; } = PostType.General;
    }

    public enum PostType
    {
        General,
        Job
    }
}
