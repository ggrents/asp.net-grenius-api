using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Domain.Entities;

namespace grenius_api.Application.Services
{
    public class AnnotationService : IAnnotationService
    {
        public AnnotationResponseDTO ConvertEntityToResponse(Annotation annotation)
        {
            if (annotation is null)
            {
                throw new ArgumentNullException(nameof(annotation));
            }

            if (annotation.Lyrics is null)
            {
                throw new ArgumentNullException(nameof(annotation.Lyrics), "Annotation's lyrics is null.");
            }

            if (string.IsNullOrEmpty(annotation.Lyrics.Text))
            {
                throw new ArgumentNullException(nameof(annotation.Lyrics.Text), "Lyrics text is null or empty.");
            }

            if (annotation.StartSymbol < 0 || annotation.StartSymbol >= annotation.Lyrics.Text.Length ||
                annotation.EndSymbol < 0 || annotation.EndSymbol >= annotation.Lyrics.Text.Length)
            {
                throw new ArgumentException("Start or end symbol is out of range of lyrics text.");
            }

            string fragment = annotation.Lyrics.Text.Substring(annotation.StartSymbol, annotation.EndSymbol - annotation.StartSymbol + 1);

            return new AnnotationResponseDTO
            {
                Id = annotation.Id,
                Fragment = fragment,
                Explanation = annotation.Text,
                LyricsId = annotation.LyricsId
            };
        }

        public Annotation ConvertRequestToEntity(AnnotationRequestDTO requestDto, Lyrics lyrics)
        {
            if (requestDto is null)
            {
                throw new ArgumentNullException(nameof(requestDto));
            }

            if (lyrics is null || lyrics.Text is null)
            {
                throw new ArgumentNullException(nameof(lyrics));
            }

            if (string.IsNullOrEmpty(requestDto.Fragment))
            {
                throw new ArgumentException("Fragment cannot be null or empty", nameof(requestDto.Fragment));
            }

            int startIndex = lyrics.Text.IndexOf(requestDto.Fragment);
            if (startIndex < 0)
            {
                throw new ArgumentException("Fragment not found in lyrics text", nameof(requestDto.Fragment));
            }

            int endIndex = startIndex + requestDto.Fragment.Length - 1;

            return new Annotation
            {
                StartSymbol = startIndex,
                EndSymbol = endIndex,
                Text = requestDto.Fragment,
                LyricsId = lyrics.Id,
                Lyrics = lyrics
            };
        }
    }

}