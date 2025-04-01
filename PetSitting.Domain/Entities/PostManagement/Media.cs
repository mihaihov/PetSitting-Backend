using System.ComponentModel.DataAnnotations;

namespace PetSitting.Domain.Entities.PostManagement
{
    public class Media
    {
        [Key]
        public Guid Id {get;set;}
        public string Url {get;set;} = string.Empty;
        public MediaType Type {get;set;}
        public required string PostId {get;set;}
        public Post? Post {get;set;}
    }

    public enum MediaType
    {
        Image,
        Video
    }
}