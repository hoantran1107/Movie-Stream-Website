namespace Movie.Domain.Entities;

public class MovieItem
{
    public int Id { get; set; }
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = "";
    public TimeSpan Duration { get; set; }
    public string PosterUrl { get; set; } = "";
    public string HlsManifestUrl { get; set; } = ""; // m3u8 (nếu dùng HLS)
    public string Mp4Url { get; set; } = "";         // fallback
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}