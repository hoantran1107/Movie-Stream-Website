using AutoMapper;
using Movie.Application.Common.Mappings;
using Movie.Domain.Entities;

namespace Movie.Application.Features.Movies.DTOs;
public class MovieDto : IMapFrom<MovieItem>
{
    public int Id { get; init; }
    public string Slug { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string PosterUrl { get; init; } = null!;
    public string HlsManifestUrl { get; init; } = null!;
    public string Mp4Url { get; init; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<MovieItem, MovieDto>()
        .ForMember(d => d.Slug, opt => opt.MapFrom(s => s.Slug.Value.ToUpperInvariant()));
    }
}