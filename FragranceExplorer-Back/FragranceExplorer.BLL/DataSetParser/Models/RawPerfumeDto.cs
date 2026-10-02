namespace FragranceExplorer.BLL.DataSetParser.Models;

public class RawPerfumeDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string? Picture { get; set; }
    
    public string? Thumbnail { get; set; }

    public RawRatingDto? Rating { get; set; }

    public List<RawAccordDto> Accords { get; set; } = new();

    public RawNotesDto? Notes { get; set; }
}