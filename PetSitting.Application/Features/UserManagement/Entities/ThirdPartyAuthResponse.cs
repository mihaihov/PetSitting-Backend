using PetSitting.Application.Features.Common;

namespace PetSitting.Application.Features.UserManagement.Entities
{
    public record ThirdPartyAuthResponse : BaseResponse
    {
        public string? JWToken {get;set;} = null;
        public string? RefreshsToken {get;set;} = null;

        //more properties later.
    }
}