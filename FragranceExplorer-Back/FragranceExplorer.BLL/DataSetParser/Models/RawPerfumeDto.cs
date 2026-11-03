using System.Text.Json.Serialization;

namespace FragranceExplorer.BLL.DataSetParser.Models;

public class RawPerfumeDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public int? Year { get; set; } 

    public string Slug { get; set; } = string.Empty; 

    public string Description { get; set; } = string.Empty; 

    public int? People { get; set; }

    public List<RawPerfumersDto> Perfumers { get; set; } = new();

    public RawRatingDto? Longevity { get; set; }

    public RawRatingDto? Sillage { get; set; }

    public RawSeasonsDto? Seasons { get; set; }

    [JsonPropertyName("ai_summary")]
    public RawAiSummaryDto? AiSummary { get; set; }

    public RawSimilarDto? Similar { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string? Picture { get; set; } 
    
    public string? Thumbnail { get; set; } 

    public RawRatingDto? Rating { get; set; } 

    public List<RawAccordDto> Accords { get; set; } = new();

    public RawNotesDto? Notes { get; set; }
}
