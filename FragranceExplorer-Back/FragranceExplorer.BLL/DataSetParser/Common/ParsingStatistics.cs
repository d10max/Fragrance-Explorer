using System.Diagnostics;
using System.Text;

namespace FragranceExplorer.BLL.DataSetParser.Common;

public class ParsingStatistics
{
    private long _totalLinesRead;
    private long _successfulParses;
    private long _failedParses;
    private long _bytesProcessed;
    private readonly Stopwatch _stopwatch = new();

    public long TotalLinesRead => _totalLinesRead;
    public long SuccessfulParses => _successfulParses;
    public long FailedParses => _failedParses;
    public TimeSpan ElapsedTime => _stopwatch.Elapsed;

    public void Start() => _stopwatch.Restart();
    public void Stop() => _stopwatch.Stop();

    public void RecordSuccess(long byteSize = 0)
    {
        _totalLinesRead++;
        _successfulParses++;
        if (byteSize > 0)
        {
            _bytesProcessed += byteSize;
        }
    }

    public void RecordFailure(long byteSize = 0)
    {
        _totalLinesRead++;
        _failedParses++;
        if (byteSize > 0)
        {
            _bytesProcessed += byteSize;
        }
    }

    public double CalculateSuccessRate()
    {
        if (_totalLinesRead == 0)
        {
            return 0.0;
        }

        return Math.Round((double)_successfulParses / _totalLinesRead * 100.0, 2);
    }

    public double CalculateSpeedLinesPerSecond()
    {
        var seconds = _stopwatch.Elapsed.TotalSeconds;
        if (seconds <= 0.001 || _totalLinesRead == 0)
        {
            return 0.0;
        }

        return Math.Round(_totalLinesRead / seconds, 2);
    }

    public double CalculateThroughputMbPerSecond()
    {
        var seconds = _stopwatch.Elapsed.TotalSeconds;
        if (seconds <= 0.001 || _bytesProcessed == 0)
        {
            return 0.0;
        }

        var megabytes = _bytesProcessed / (1024.0 * 1024.0);
        return Math.Round(megabytes / seconds, 2);
    }

    public string GetSummary()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Dataset Parsing Results");
        sb.AppendLine($"Elapsed time: {_stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
        sb.AppendLine($"Lines read: {_totalLinesRead}");
        sb.AppendLine($"Successfully processed: {_successfulParses}");
        sb.AppendLine($"Skipped/Errors: {_failedParses}");
        sb.AppendLine($"Success rate: {CalculateSuccessRate()}%");
        sb.AppendLine($"Speed: {CalculateSpeedLinesPerSecond()} lines/sec ({CalculateThroughputMbPerSecond()} MB/s)");
        return sb.ToString();
    }
}
