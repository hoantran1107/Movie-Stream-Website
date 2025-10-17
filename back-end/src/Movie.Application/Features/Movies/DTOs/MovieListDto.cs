using AutoMapper;
using Movie.Application.Common.Mappings;

namespace Movie.Application.Features.Movies.DTOs;

public class MovieListDto : IMapFrom<List<MovieDto>>
{
    public int Id { get; set; }
    public string Slug { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string PosterUrl { get; set; } = null!;
    public string HlsManifestUrl { get; set; } = null!;
    public string Mp4Url { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<MovieDto, MovieListDto>()
        .ForMember(d => d.Slug, opt => opt.MapFrom(s => s.Slug.ToUpperInvariant()));
    }
}