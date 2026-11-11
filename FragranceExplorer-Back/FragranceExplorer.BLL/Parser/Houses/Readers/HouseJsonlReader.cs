using FragranceExplorer.BLL.Parser.Common;
using FragranceExplorer.BLL.Parser.Common.Readers;
using FragranceExplorer.BLL.Parser.Houses.Models;

namespace FragranceExplorer.BLL.Parser.Houses.Readers;

public class HouseJsonlReader : JsonLinesReader<RawHouseDto>
{
    private readonly ParserOptions _options;
    private long _yieldedCount;
    private long _skippedCount;
    private long _errorCount;

    public long YieldedCount => _yieldedCount;
    public long SkippedCount => _skippedCount;
    public long ErrorCount => _errorCount;

    public HouseJsonlReader(ParserOptions? options = null)
    {
        _options = options ?? new ParserOptions { PathToDataset = "house_catalog.jsonl" };
    }

    protected override string? PreProcessLine(string rawLine)
    {
        var sanitized = base.PreProcessLine(rawLine);
        if (sanitized is null)
        {
            return null;
        }

        if (!sanitized.Contains("\"id\""))
        {
            _errorCount++;
            return null;
        }

        return sanitized;
    }

    public bool ShouldYieldNext()
    {
        if (_skippedCount < _options.SkipRecords)
        {
            _skippedCount++;
            return false;
        }

        if (_options.MaxPerfumesToParse.HasValue && _yieldedCount >= _options.MaxPerfumesToParse.Value)
        {
            return false;
        }

        _yieldedCount++;
        return true;
    }

    protected override void HandleLineError(string rawLine, Exception ex)
    {
        base.HandleLineError(rawLine, ex);
        _errorCount++;
    }

    public string GetReaderStatusSummary()
    {
        return $"[HouseJsonlReader] Processed: {_linesProcessed} lines, " +
               $"yielded: {_yieldedCount}, skipped: {_skippedCount}, errors: {_errorCount}. " +
               $"Progress: {ProgressPercentage}%, avg line length: {GetAverageBytesPerLine()} B.";
    }
}