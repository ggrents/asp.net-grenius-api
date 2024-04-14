using AutoMapper;
using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Domain.Entities;

namespace grenius_api.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Artist, ArtistResponseDTO>();
            CreateMap<Song, SongResponseDTO>();
            CreateMap<Feature, FeatureResponseDTO>();
            CreateMap<Album, AlbumResponseDTO>().ForMember(dest => dest.AlbumType, opt =>
            {
                opt.MapFrom(src => GetAlbumTypeDescription(src.AlbumTypeId));
            });
        }
        private string GetAlbumTypeDescription(AlbumType albumType)
{
        switch ((int) albumType)
        {
            case 0:
                return "Album";
            case 1:
                return "EP";
            case 2:
                return "Single";
            case 3:
                return "Mixtape";
            
            default:
            throw new NotImplementedException();
    }
}

    }
}
