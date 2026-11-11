using System.Text.Json.Serialization;

namespace FragranceExplorer.BLL.Parser.Perfumes.Models;

public class RawSimilarItemDto
{
    public int Id { get; set; }

    public string Slug { get; set; } = string.Empty;
}

public class RawSimilarDto
{
    [JsonPropertyName("reminds_me_of")]
    public List<RawSimilarItemDto> RemindsMeOf { get; set; } = new();

    [JsonPropertyName("also_liked")]
    public List<RawSimilarItemDto> AlsoLiked { get; set; } = new();
}
