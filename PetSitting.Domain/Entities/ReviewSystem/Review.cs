using System.ComponentModel.DataAnnotations;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.ReviewSystem
{
    public class Review
    {
        public string Id  {get;set;}= Guid.NewGuid().ToString();
        public required string Title {get;set;}
        public required string Content {get;set;}
        public DateTime PostedAt {get;set;} = DateTime.Now;
        public DateTime? UpdatedAt {get;set;}
        public int? UpdatedCount {get;set;} = 0;
        public required int Rating {get;set;}

        //FK
        public ApplicationUser Author {get;set;} = null!;
        public required string AuthorId {get;set;}
        public Post Post {get;set;} = null!;
        public required string PostId {get;set;}

        public static Review CreateReview(string title, string content, DateTime? postedAt,DateTime? updatedAt,
            int? updateCount, int rating, string authorId, string postId)
        {
            Review review = new Review {
                Title = title,
                Content = content,
                PostedAt = postedAt != null ? (DateTime)postedAt : DateTime.Now,
                UpdatedAt = updatedAt,
                UpdatedCount = updateCount,
                Rating = rating,
                AuthorId = authorId,
                PostId = postId
            };
            return review;
        }
    }
}