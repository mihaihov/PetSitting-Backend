using PetSitting.Application.Common;
using MediatR;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Domain.Entities.UserManagement;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSitting.Domain.Enums;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Application.Interfaces.Repositories;

namespace PetSitting.Application.Features.UserManagement.Commands
{

    public record RegisterCommand(string? firstName, string? lastName, string? username, string email, string password) : IRequest<RegisterCommandResponse> {}
    public record RegisterCommandResponse : BaseResponse;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
    {
        private readonly IFirebaseServices _firebaseService;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<IdentityRole> _roleRepository;
        public RegisterCommandHandler(IFirebaseServices firebaseServices, IUserRepository userRepository, IBaseRepository<IdentityRole> roleRepository)
        {
            _firebaseService = firebaseServices;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string? firebaseUID = null;
            using var userTransactions = await _userRepository.BeginTransactionAsync();
            using var rolesTransactions = await _roleRepository.BeginTransactionAsync();

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
                
                await _userRepository.AddAsync(newUser);

                var roles = await _roleRepository.GetAllAsync();
                var role = roles.Where(r => r.Name == Roles.PetOwner.ToString()).FirstOrDefault();
                if (role == null)
                    throw new Exception("Default role does not exists in the database!");

                await _userRepository.AddRole(new IdentityUserRole<string> { RoleId = role.Id, UserId = firebaseUser.Uid });
                await _userRepository.AddUserProfile(new UserProfile { User = newUser });
                await _userRepository.AddUserSettings(new UserSettings { User = newUser });

                await _userRepository.SaveChangesAsync();
                await _roleRepository.SaveChangesAsync();
                
                await userTransactions.CommitAsync();
                await rolesTransactions.CommitAsync();

                return response;
            }
            catch(Exception)
            {
                await userTransactions.RollbackAsync();
                await rolesTransactions.RollbackAsync();

                if(!string.IsNullOrEmpty(firebaseUID))
                {
                    await FirebaseAuth.DefaultInstance.DeleteUserAsync(firebaseUID);
                }

                throw;
            }
        }
    }
}