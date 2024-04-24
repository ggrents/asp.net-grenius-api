using AutoMapper;
using grenius_api.Application.Models.Responses;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace grenius_api.Application.Controllers
{
    [Authorize]
    [Route("api/rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;

        public RatingController(GreniusContext db,IMapper mapper )
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet("most-requested-artists")]
        [SwaggerOperation(Summary = "Get most requested artists")]
        [SwaggerResponse(200, Type = typeof(ArtistRatingResponseDTO))]
        public async Task<IActionResult> GetMostRequestedArtists(int number= 50, CancellationToken cancellationToken)
        {
            var mostRequestedArtists = await _db.ArtistsRating
                                                .Include(a=>a.Artist)
                                                .OrderByDescending(a => a.Count)
                                                .Take(number)
                                                .ToListAsync(cancellationToken);

            return Ok(_mapper.Map<List<ArtistRatingResponseDTO>>(mostRequestedArtists));
        }


        [HttpGet("most-requested-songs")]
        [SwaggerOperation(Summary = "Get most requested songs")]
        [SwaggerResponse(200, Type = typeof(SongRatingResponseDTO))]
        public async Task<IActionResult> GetMostRequestedSongs(int number = 50, CancellationToken cancellationToken)
        {
            var mostRequestedSongs = await _db.SongsRating
                                                .Include(a => a.Song)
                                                .OrderByDescending(a => a.Count)
                                                .Take(number)
                                                .ToListAsync(cancellationToken);

            return Ok(_mapper.Map<List<SongRatingResponseDTO>>(mostRequestedSongs));
        }
    }
}
