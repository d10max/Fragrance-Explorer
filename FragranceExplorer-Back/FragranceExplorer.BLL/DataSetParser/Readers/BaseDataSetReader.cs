using System.Runtime.CompilerServices;
using System.Text;
using FragranceExplorer.BLL.DataSetParser.Interfaces;

namespace FragranceExplorer.BLL.DataSetParser.Readers;

public abstract class BaseDataSetReader<T> : IDataSetReader<T> where T : class
{
    protected long _totalBytesRead;
    protected long _fileSizeBytes;
    protected long _linesProcessed;

    public long TotalBytesRead => _totalBytesRead;
    public long LinesProcessed => _linesProcessed;
    public double ProgressPercentage => CalculateProgress();

    public virtual async IAsyncEnumerable<T> ReadAsync(
        string filePath, 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Dataset file not found at path: {filePath}", filePath);
        }

        var fileInfo = new FileInfo(filePath);
        _fileSizeBytes = fileInfo.Length;
        _totalBytesRead = 0;
        _linesProcessed = 0;

        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan);
        using var streamReader = new StreamReader(fileStream, Encoding.UTF8);

        string? line;
        while ((line = await streamReader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) is not null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lineByteCount = Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;
            _totalBytesRead += lineByteCount;
            _linesProcessed++;

            var processedLine = PreProcessLine(line);
            if (processedLine is null)
            {
                continue;
            }

            T? result;
            try
            {
                result = ParseLine(processedLine);
            }
            catch (Exception ex)
            {
                HandleLineError(line, ex);
                continue;
            }

            if (result is not null)
            {
                yield return result;
            }
        }
    }

    public double CalculateProgress()
    {
        if (_fileSizeBytes <= 0)
        {
            return 0.0;
        }

        var progress = (double)_totalBytesRead / _fileSizeBytes * 100.0;
        return Math.Clamp(Math.Round(progress, 2), 0.0, 100.0);
    }

    public double GetAverageBytesPerLine()
    {
        if (_linesProcessed == 0)
        {
            return 0.0;
        }

        return Math.Round((double)_totalBytesRead / _linesProcessed, 2);
    }

    protected virtual string? PreProcessLine(string rawLine)
    {
        if (string.IsNullOrWhiteSpace(rawLine))
        {
            return null;
        }

        return rawLine.Trim();
    }

    protected abstract T? ParseLine(string processedLine);

    protected virtual void HandleLineError(string rawLine, Exception ex)
    {
        
    }
}
