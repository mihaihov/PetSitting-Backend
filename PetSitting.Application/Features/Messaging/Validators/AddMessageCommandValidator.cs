using FluentValidation;
using PetSitting.Application.Features.Messaging.Commands;

namespace PetSitting.Application.Features.Messaging.Validators
{
    public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
    {
        public AddMessageCommandValidator()
        {
            RuleFor(m => m.chatSessionId).NotEmpty().NotNull();
            RuleFor(m => m.senderId).NotEmpty().NotNull();
            RuleFor(m => m.recipientId).NotEmpty().NotNull();
            RuleFor(m => m.content).NotEmpty().NotNull();
        }
    }
}