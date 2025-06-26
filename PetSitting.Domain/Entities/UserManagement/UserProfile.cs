using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class UserProfile {
#pragma warning disable CS8618
        [Key]
        public string Id {get;set;}
#pragma warning restore CS8618

        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        
        public string? Bio {get;set;}
        public string? ProfilePictureUrl {get;set;}
        public string? CoverPictureUrl {get;set;} 
        public string? Location {get;set;}
        public DateTime? DateOfBirth {get;set;}
    }
}