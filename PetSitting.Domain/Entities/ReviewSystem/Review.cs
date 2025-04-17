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
        public int? UpdatedCount {get;set;}
        public required int Rating {get;set;}

        //FK
        public ApplicationUser Author {get;set;} = null!;
        public required string AuthorId {get;set;}
        public Post Post {get;set;} = null!;
        public required string PostId {get;set;}
    }
}