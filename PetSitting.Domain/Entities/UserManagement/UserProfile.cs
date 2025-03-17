using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class UserProfile {
        [Key]
        public required string Id {get;set;}

        [ForeignKey(nameof(Id))]
        public virtual ApplicationUser User {get;set;} = new ApplicationUser();
        
        public string? Bio {get;set;}
        public string? ProfilePictureUrl {get;set;}
        public string? CoverPictureUrl {get;set;} 
    }
}