using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Domain.Entities;

namespace grenius_api.Application.Services
{
    public interface IAnnotationService
    {
        public Annotation ConvertRequestToEntity(AnnotationRequestDTO requestDto, Lyrics lyrics);
        public AnnotationResponseDTO ConvertEntityToResponse(Annotation annotation);
    }
}
