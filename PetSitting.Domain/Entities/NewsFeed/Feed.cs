using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.NewsFeed
{
    public class Feed
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string PostId { get; set; }
        public required string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}