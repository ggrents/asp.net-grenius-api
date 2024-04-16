using AutoMapper;
using grenius_api.Application.Models.Responses;
using grenius_api.Application.Services;
using grenius_api.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Artist, ArtistResponseDTO>();
        CreateMap<Song, SongResponseDTO>();
        CreateMap<Feature, FeatureResponseDTO>();
        CreateMap<Genre, GenreResponseDTO>();
        CreateMap<Producer, ProducerResponseDTO>();
        CreateMap<Lyrics, LyricsResponseDTO>();
        CreateMap<Annotation, AnnotationResponseDTO>()
            .ConvertUsing<AnnotationToAnnotationResponseDTOConverter>();
        CreateMap<Album, AlbumResponseDTO>().ForMember(dest => dest.AlbumType, opt => opt.MapFrom(src => GetAlbumTypeDescription(src.AlbumTypeId)));
    }

    private string GetAlbumTypeDescription(AlbumType albumType)
    {
        switch ((int)albumType)
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

public class AnnotationToAnnotationResponseDTOConverter : ITypeConverter<Annotation, AnnotationResponseDTO>
{
    private readonly IAnnotationService _annotationService;

    public AnnotationToAnnotationResponseDTOConverter(IAnnotationService annotationService)
    {
        _annotationService = annotationService;
    }

    public AnnotationResponseDTO Convert(Annotation source, AnnotationResponseDTO destination, ResolutionContext context)
    {
        return _annotationService.ConvertEntityToResponse(source);
    }
}
