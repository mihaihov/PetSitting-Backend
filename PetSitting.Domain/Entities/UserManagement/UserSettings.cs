using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class UserSettings {
        [Key]
        public required string Id {get;set;}

        [ForeignKey(nameof(Id))]
        public virtual ApplicationUser User {get;set;} = new ApplicationUser();

        public bool ReceiveNotifications {get;set;} = false;
        public string PreferedLanguage {get;set;} = "en-us";

    }
}