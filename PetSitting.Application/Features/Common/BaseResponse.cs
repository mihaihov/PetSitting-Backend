namespace PetSitting.Application.Features.Common
{
    public record BaseResponse
    {
        public bool Success { get; set; } = true;
        public List<string>? ValidationErrors { get; set; }
    }
}