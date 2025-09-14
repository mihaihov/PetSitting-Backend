using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class UserSettings {
        [Key]
        public string ApplicationUserId { get; set; } = string.Empty;
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? User {get;set;}
        public bool ReceiveEmailNotifications {get;set;} = false;
        public bool ReceiveSMSNotifications {get;set;} = false;
        public bool IsProfilePublic {get;set;} = true;
        public string PreferedLanguage {get;set;} = "en-us";

    }
}