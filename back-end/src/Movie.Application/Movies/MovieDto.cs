namespace Movie.Application.Movies;
public record MovieDto(int Id, string Slug, string Title, string Description,
    string PosterUrl, string HlsManifestUrl, string Mp4Url);