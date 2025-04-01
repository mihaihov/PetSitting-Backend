using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.Security;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class ApplicationUser : IdentityUser {
        public string FirstName {get;set;} = string.Empty;
        public string LastName {get;set;} = string.Empty;
        public DateTime DateJoined {get;set;}
        public bool IsVerified {get;set;} = false;
        public bool IsPetSitter {get;set;} = false;

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}