using PetSitting.Application.Common;
using MediatR;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Domain.Entities.UserManagement;
using FirebaseAdmin.Auth;
using PetSitting.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSitting.Domain.Enums;
using PetSitting.Domain.Interfaces.Services;

namespace PetSitting.Application.Features.UserManagement.Commands
{

    public record RegisterCommand(string? firstName, string? lastName, string? username, string email, string password) : IRequest<RegisterCommandResponse> {}
    public record RegisterCommandResponse : BaseResponse;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
    {
        private readonly IFirebaseServices _firebaseService;
        private readonly ApplicationDbContext _dbContext;
        public RegisterCommandHandler(IFirebaseServices firebaseServices, ApplicationDbContext dbContext)
        {
            _firebaseService = firebaseServices;
            _dbContext = dbContext;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string? firebaseUID = null;
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                RegisterCommandResponse response = new RegisterCommandResponse();

                var validator = new RegisterCommandValidator();
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.Errors.Any())
                {
                    response.Success = false;
                    response.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors)
                        response.ValidationErrors.Add(error.ErrorMessage);

                    return response;
                }

                //creates firebaseUser 
                var firebaseUser = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
                    {
                        Email = request.email,
                        Password = request.password,
                        DisplayName = string.IsNullOrEmpty(request.username) ? string.Empty : request.username,
                        EmailVerified = false,
                        Disabled = false,
                    }
                );
                if (firebaseUser == null)
                    throw new Exception("Failed to create user in Firebase!");

                firebaseUID = firebaseUser.Uid;

                //creates sql user.
                var newUser = new ApplicationUser
                {
                    Id = firebaseUser.Uid,
                    Email = request.email,
                    UserName = request.email,
                    FirstName = string.IsNullOrEmpty(request.firstName) ? string.Empty : request.firstName,
                    LastName = string.IsNullOrEmpty(request.lastName) ? string.Empty : request.lastName,
                    DateJoined = DateTime.Now,
                    IsPetSitter = false,
                    IsVerified = false,

                };
                
                await _dbContext.Users.AddAsync(newUser);

                var role = await _dbContext.Roles.Where(r => r.Name == Roles.PetOwner.ToString()).FirstOrDefaultAsync();
                if (role == null)
                    throw new Exception("Default role does not exists in the database!");

                await _dbContext.UserRoles.AddAsync(new IdentityUserRole<string> { RoleId = role.Id, UserId = firebaseUser.Uid });
                await _dbContext.UserProfiles!.AddAsync(new UserProfile { User = newUser });
                await _dbContext.UserSettings!.AddAsync(new UserSettings { User = newUser });

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return response;
            }
            catch(Exception)
            {
                await transaction.RollbackAsync();

                if(!string.IsNullOrEmpty(firebaseUID))
                {
                    await FirebaseAuth.DefaultInstance.DeleteUserAsync(firebaseUID);
                }

                throw;
            }
        }
    }
}