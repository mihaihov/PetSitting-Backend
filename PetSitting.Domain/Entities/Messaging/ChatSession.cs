using System.ComponentModel.DataAnnotations;

namespace PetSitting.Domain.Entities.Messaging
{
    public class ChatSession
    {
#pragma warning disable
        [Key]
        public string ChatSessionId {get;set;}
        public string FirstUserId {get;set;}
        public string SecondUserId {get;set;}
        public string JobPostId {get;set;}
        public bool IsActive{get;set;}

        public ICollection<Message> Messages {get; set;} = new List<Message>();
#pragma warning restore
    }
}