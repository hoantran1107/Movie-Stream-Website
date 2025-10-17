using Movie.Application.Common.Mappings;

namespace Movie.Application.Features.Movies.DTOs;

public class MovieDetailDto : IMapFrom<MovieDto>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string PosterUrl { get; set; }
    public string HlsManifestUrl { get; set; }
    public string Mp4Url { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Director { get; set; }
    public List<string> Cast { get; set; }
    public List<string> Genres { get; set; }
    public double Rating { get; set; }
}