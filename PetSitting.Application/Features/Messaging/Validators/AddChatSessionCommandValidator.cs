using FluentValidation;
using PetSitting.Application.Features.Messaging.Commands;

namespace PetSitting.Application.Features.Messaging.Validators
{
    public class AddChatSessionCommandValidator : AbstractValidator<AddChatSessionCommand>
    {
        public AddChatSessionCommandValidator()
        {
            RuleFor(p => p.firstUser).NotEmpty().NotNull();
            RuleFor(p => p.secondUser).NotEmpty().NotNull();
            RuleFor(p => p.jobPostId).NotEmpty().NotNull();
        }
    }
}