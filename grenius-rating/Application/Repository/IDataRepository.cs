namespace grenius_rating.Application.Repository
{
    public interface IDataRepository
    {
        Task AddRatingCount(int entityId, int columnType);
    }
}
