using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class UserSettings {
#pragma warning disable CS8618
        [Key]
        public string Id {get;set;}
#pragma warning restore CS8618

        [ForeignKey(nameof(Id))]
        public virtual ApplicationUser User {get;set;} = new ApplicationUser();

        public bool ReceiveEmailNotifications {get;set;} = false;
        public bool ReceiveSMSNotifications {get;set;} = false;
        public bool IsProfilePublic {get;set;} = true;
        public string PreferedLanguage {get;set;} = "en-us";

    }
}