using MediatR;
using Microsoft.AspNetCore.Identity;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public record ResetPasswordCommand(string firebaseToken, string newPassword) : IRequest<BaseResponse>;

    public class ResetPasswordCommandHandler : BaseHandler<ResetPasswordCommand,BaseResponse,ResetPasswordCommandValidator>
    {
        private readonly IFirebaseService _firebaseservice;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ResetPasswordCommandHandler(IFirebaseService firebaseService, 
            IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _firebaseservice = firebaseService;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        protected override async Task<BaseResponse> HandleCommand(ResetPasswordCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            try
            {
                var tokenValidationResult = await _firebaseservice.VerifyTokenAsync(request.firebaseToken);
                if (tokenValidationResult == null)
                    throw new Exception("Token validation failed!");
                
                var sqlUser = await _userRepository.GetByIdAsync(tokenValidationResult.Uid);
                if(sqlUser == null)
                    throw new Exception("User not found in the database");
                sqlUser.PasswordHash = _userManager.PasswordHasher.HashPassword(sqlUser,request.newPassword);
                
                _userRepository.Update(sqlUser);
                await _firebaseservice.ResetPasswordAsync(request.firebaseToken, request.newPassword);
                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}