using FluentValidation;
using PetSitting.Application.Features.Messaging.Commands;

namespace PetSitting.Application.Features.Messaging.Validators
{
    public class AddChatSessionCommandValidator : AbstractValidator<AddChatSessionCommand>
    {
        public AddChatSessionCommandValidator()
        {
            RuleFor(p => p.petOwnerId).NotEmpty().NotNull();
            RuleFor(p => p.petSitterId).NotEmpty().NotNull();
            RuleFor(p => p.jobPostId).NotEmpty().NotNull();
        }
    }
}