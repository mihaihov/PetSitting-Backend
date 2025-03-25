using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class UserSettings {
        [Key]
        public string Id {get;set;}

        [ForeignKey(nameof(Id))]
        public virtual ApplicationUser User {get;set;} = new ApplicationUser();

        public bool ReceiveEmailNotifications {get;set;} = false;
        public bool ReceiveSMSNotifications {get;set;} = false;
        public bool IsProfilePublic {get;set;} = true;
        public string PreferedLanguage {get;set;} = "en-us";

    }
}