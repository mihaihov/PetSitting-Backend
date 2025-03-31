using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class UserProfile {
#pragma warning disable CS8618
        [Key]
        public string Id {get;set;}
#pragma warning restore CS8618

        [ForeignKey(nameof(Id))]
        public virtual ApplicationUser User {get;set;} = new ApplicationUser();
        
        public string? Bio {get;set;}
        public string? ProfilePictureUrl {get;set;}
        public string? CoverPictureUrl {get;set;} 
        public string? Location {get;set;}
        public DateTime? DateOfBirth {get;set;}
    }
}