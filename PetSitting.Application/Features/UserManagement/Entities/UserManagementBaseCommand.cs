using MediatR;
using PetSitting.Application.Features.Common;

namespace PetSitting.Application.Features.UserManagement.Entities
{
    public class UserManagementBaseCommand<T> : IRequest<T>
    {
        public string? Email {get;set;}
        public string? Password {get;set;}
        public string? FirebaseToken {get;set;}
    }
}