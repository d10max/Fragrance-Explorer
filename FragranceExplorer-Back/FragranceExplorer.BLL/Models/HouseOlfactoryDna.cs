namespace FragranceExplorer.BLL.Models;

public class HouseOlfactoryDna 
{
    public string Family { get; private set; }

    public  string Notes { get; private set; }

    public int Percentage { get; private set; }

    public HouseOlfactoryDna(string family, string notes, int percentage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(family);
        ArgumentException.ThrowIfNullOrWhiteSpace(notes);
        ArgumentOutOfRangeException.ThrowIfNegative(percentage);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(percentage, 100);

        Family = family;
        Notes = notes;
        Percentage = percentage;
    }
}