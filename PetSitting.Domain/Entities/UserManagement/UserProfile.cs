using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class UserProfile {

        [Key]
        public string ApplicationUserId { get; set; } = string.Empty;
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? User { get; set; }
        
        public string? Bio {get;set;}
        public string? ProfilePictureUrl {get;set;}
        public string? CoverPictureUrl {get;set;} 
        public string? Location {get;set;}
        public DateTime? DateOfBirth {get;set;}
    }
}