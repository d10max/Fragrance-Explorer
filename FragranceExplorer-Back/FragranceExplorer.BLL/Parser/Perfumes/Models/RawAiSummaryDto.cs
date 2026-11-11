namespace FragranceExplorer.BLL.Parser.Perfumes.Models;

public class RawAiSummaryItemDto
{
    public string Text { get; set; } = string.Empty;
}

public class RawAiSummaryDto
{
    public List<RawAiSummaryItemDto> Pros { get; set; } = new();

    public List<RawAiSummaryItemDto> Cons { get; set; } = new();
}

