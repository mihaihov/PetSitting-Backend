using PetSitting.Application.Common;
using MediatR;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using PetSitting.Domain.Enums;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using FirebaseAdmin.Auth;

namespace PetSitting.Application.Features.UserManagement.Commands
{

    public record RegisterCommand(string? firstName, string? lastName, string? username, string email, string password) : IRequest<RegisterCommandResponse> {}
    public record RegisterCommandResponse : BaseResponse;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<IdentityRole> _roleRepository;
        private readonly IFirebaseServices _firebaseServices;
        public RegisterCommandHandler(IUserRepository userRepository, IBaseRepository<IdentityRole> roleRepository, IFirebaseServices firebaseServices)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _firebaseServices = firebaseServices;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string? firebaseUID = null;
            using var userTransactions = await _userRepository.BeginTransactionAsync();

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
                var firebaseUser = await _firebaseServices.CreateUserAsync(new UserRecordArgs
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
                    PasswordHash = request.password,
                    FirstName = string.IsNullOrEmpty(request.firstName) ? string.Empty : request.firstName,
                    LastName = string.IsNullOrEmpty(request.lastName) ? string.Empty : request.lastName,
                    DateJoined = DateTime.Now,
                    IsPetSitter = false,
                    IsVerified = false,

                };
                
                await _userRepository.AddAsync(newUser);

                var role = await _roleRepository.FirstOrDefaultAsync(r => r.Name == Roles.PetOwner.ToString());
                if (role == null)
                    throw new Exception("Default role does not exists in the database!");

                await _userRepository.AddRole(new IdentityUserRole<string> { RoleId = role.Id.ToString(), UserId = firebaseUser.Uid });
                await _userRepository.AddUserProfile(new UserProfile { User = newUser });
                await _userRepository.AddUserSettings(new UserSettings { User = newUser });

                await _userRepository.SaveChangesAsync();
                await _roleRepository.SaveChangesAsync();
                
                await _userRepository.CommitTransactionAsync();

                return response;
            }
            catch(Exception)
            {
                await userTransactions.RollbackAsync();

                if(!string.IsNullOrEmpty(firebaseUID))
                {
                    await FirebaseAuth.DefaultInstance.DeleteUserAsync(firebaseUID);
                }

                throw;
            }
        }
    }
}