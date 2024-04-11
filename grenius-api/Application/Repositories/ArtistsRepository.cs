using grenius_api.Domain.Entities;
using grenius_api.Domain.Exceptions;
using grenius_api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace grenius_api.Application.Repositories
{
    public interface IArtistsRepository
    {
        Task<List<Artist>> GetArtists();
        Task<Artist?> GetArtist(int id);
    }
    public class ArtistsRepository : IArtistsRepository
    {
        private readonly GreniusContext _db;
        public ArtistsRepository(GreniusContext db)
        {
            _db = db;
        }

        public async Task<List<Artist>> GetArtists()
        {
            return await _db.Artists.ToListAsync();
        }

        public async Task<Artist?> GetArtist(int id)
        {
            return await _db.Artists.FirstOrDefaultAsync(a=>a.Id==id);
        }
    }
}
