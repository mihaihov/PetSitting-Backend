namespace PetSitting.Application.Interfaces.Services
{
    public interface IFanoutService<TPublish, TRetrieve>
    {
        public Task Publish(TPublish? entity);
        public Task<IReadOnlyList<TPublish>?> Retrieve(TRetrieve retrieveBy);
    }
}