using PetSitting.Application.Features.Common;
using MediatR;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using PetSitting.Domain.Enums;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using FirebaseAdmin.Auth;
using PetSitting.Application.Features.UserManagement.Entities;

namespace PetSitting.Application.Features.UserManagement.Commands
{

    public record RegisterCommand(string? firstName, string? lastName, string? username, string email, string password) : IRequest<BaseResponse> {}

    public class RegisterCommandHandler : BaseCommandHandler<RegisterCommand,BaseResponse,RegisterCommandValidator>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<IdentityRole> _roleRepository;
        private readonly IFirebaseService _firebaseServices;
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterCommandHandler(IUserRepository userRepository, IBaseRepository<IdentityRole> roleRepository, IFirebaseService firebaseServices,
            UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _firebaseServices = firebaseServices;
            _userManager = userManager;
        }

        protected override async Task<BaseResponse> HandleCommand(RegisterCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            string? firebaseUID = null;
            using var userTransactions = await _userRepository.BeginTransactionAsync();

            try
            {
                //creates firebaseUser 
                var firebaseUser = await _firebaseServices.CreateUserWithEmailAndPasswordAsync(request.email,request.password);
                if (firebaseUser == null)
                    throw new Exception("Failed to create user in Firebase!");

                firebaseUID = firebaseUser.User.LocalId;

                //creates sql user.
                var newUser = new ApplicationUser
                {
                    Id = firebaseUser.User.LocalId,
                    Email = request.email,
                    UserName = request.username,
                    PasswordHash = _userManager.PasswordHasher.HashPassword(new ApplicationUser(), request.password),
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

                await _userRepository.AddRole(new IdentityUserRole<string> { RoleId = role.Id.ToString(), UserId = firebaseUser.User.LocalId });
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