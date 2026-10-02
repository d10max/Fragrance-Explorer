using System.Text.Json;

namespace FragranceExplorer.BLL.DataSetParser.Readers;

public class JsonLinesReader<T> : BaseDataSetReader<T> where T : class
{
    protected static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected override T? ParseLine(string processedLine)
    {
        if (!IsValidJsonFormat(processedLine))
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(processedLine, DefaultJsonOptions);
    }

    protected virtual bool IsValidJsonFormat(string line)
    {
        if (line.Length < 2)
        {
            return false;
        }

        var trimmed = line.Trim();
        return (trimmed.StartsWith('{') && trimmed.EndsWith('}')) ||
               (trimmed.StartsWith('[') && trimmed.EndsWith(']'));
    }
}
