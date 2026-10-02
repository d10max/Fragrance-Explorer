namespace FragranceExplorer.BLL.DataSetParser.Models;


public class RawNotesTieredDto
{
    public List<RawNoteDto>? Top { get; set; }

    public List<RawNoteDto>? Middle { get; set; }

    public List<RawNoteDto>? Base { get; set; }

}

public class RawNoteDto
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public int Weight { get; set; }
}

public class RawNotesDto
{
    public RawNotesTieredDto? Tiered { get; set; }

    public List<RawNoteDto>? Flat { get; set; }
}