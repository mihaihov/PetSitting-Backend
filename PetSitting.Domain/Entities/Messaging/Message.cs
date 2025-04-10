using System.ComponentModel.DataAnnotations;

namespace PetSitting.Domain.Entities.Messaging
{
    public class Message
    {
#pragma warning disable
        public string MessageId {get;set;}
        public string ChatSessionId {get;set;}
        public ChatSession ChatSession {get;set;}
        public string SenderId {get;set;}
        public string RecipientId {get;set;}
        public string Content {get;set;}
        public DateTime Timestamp{get;set;}
#pragma warning restore
    }
}