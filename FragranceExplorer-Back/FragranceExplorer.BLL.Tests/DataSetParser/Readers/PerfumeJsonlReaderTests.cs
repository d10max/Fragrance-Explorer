using FragranceExplorer.BLL.DataSetParser.Common;
using FragranceExplorer.BLL.DataSetParser.Readers;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.DataSetParser.Readers;

public class PerfumeJsonlReaderTests : IDisposable
{
    private readonly List<string> _tempFilesToCleanup = new();

    private async Task<string> CreateTempJsonlFileAsync(IEnumerable<string> lines)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"perfume_test_{Guid.NewGuid():N}.jsonl");
        await File.WriteAllLinesAsync(tempFile, lines);
        _tempFilesToCleanup.Add(tempFile);
        return tempFile;
    }

    public void Dispose()
    {
        foreach (var file in _tempFilesToCleanup)
        {
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    // Ignore cleanup errors in tests
                }
            }
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ReadAsync_WhenFilePathIsNullOrWhitespace_ShouldThrowArgumentException(string? invalidPath)
    {
        var reader = new PerfumeJsonlReader();

        var act = async () =>
        {
            await foreach (var _ in reader.ReadAsync(invalidPath!))
            {
            }
        };

        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("filePath");
    }

    [Fact]
    public async Task ReadAsync_WhenFileNotFound_ShouldThrowFileNotFoundException()
    {
        var reader = new PerfumeJsonlReader();
        var nonExistentPath = Path.Combine(Path.GetTempPath(), $"non_existent_{Guid.NewGuid():N}.jsonl");

        var act = async () =>
        {
            await foreach (var _ in reader.ReadAsync(nonExistentPath))
            {
            }
        };

        await act.Should().ThrowAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task ReadAsync_WhenFileHasValidJsonLines_ShouldDeserializeAllRecords()
    {
        var reader = new PerfumeJsonlReader();
        var lines = new[]
        {
            "{\"id\": 1, \"name\": \"Aventus\", \"brand\": \"Creed\", \"gender\": \"male\"}",
            "{\"id\": 2, \"name\": \"Sauvage\", \"brand\": \"Dior\", \"gender\": \"male\"}"
        };
        var filePath = await CreateTempJsonlFileAsync(lines);

        var results = new List<object>();
        await foreach (var item in reader.ReadAsync(filePath))
        {
            results.Add(item);
        }

        results.Should().HaveCount(2);
        reader.LinesProcessed.Should().Be(2);
        reader.ProgressPercentage.Should().Be(100.0);
    }

    [Fact]
    public async Task ReadAsync_WhenFileContainsEmptyOrWhitespaceLines_ShouldIgnoreThem()
    {
        var reader = new PerfumeJsonlReader();
        var lines = new[]
        {
            "",
            "   ",
            "{\"id\": 1, \"name\": \"Bleu de Chanel\", \"brand\": \"Chanel\"}",
            ""
        };
        var filePath = await CreateTempJsonlFileAsync(lines);

        var results = new List<object>();
        await foreach (var item in reader.ReadAsync(filePath))
        {
            results.Add(item);
        }

        results.Should().HaveCount(1);
        reader.LinesProcessed.Should().Be(4);
    }

    [Fact]
    public async Task ReadAsync_WhenLineDoesNotContainId_ShouldCountAsErrorAndSkip()
    {
        var reader = new PerfumeJsonlReader();
        var lines = new[]
        {
            "{\"name\": \"Perfume Without Id\", \"brand\": \"Test\"}",
            "{\"id\": 10, \"name\": \"Valid Perfume\", \"brand\": \"Test\"}"
        };
        var filePath = await CreateTempJsonlFileAsync(lines);

        var results = new List<object>();
        await foreach (var item in reader.ReadAsync(filePath))
        {
            results.Add(item);
        }

        results.Should().HaveCount(1);
        reader.ErrorCount.Should().Be(1);
    }

    [Fact]
    public async Task ReadAsync_WhenLineHasMalformedJson_ShouldHandleErrorAndContinueReading()
    {
        var reader = new PerfumeJsonlReader();
        var lines = new[]
        {
            "{\"id\": not_a_valid_json_structure",
            "{\"id\": 5, \"name\": \"Valid Perfume\", \"brand\": \"Test\"}"
        };
        var filePath = await CreateTempJsonlFileAsync(lines);

        var results = new List<object>();
        await foreach (var item in reader.ReadAsync(filePath))
        {
            results.Add(item);
        }

        results.Should().HaveCount(1);
        reader.LinesProcessed.Should().Be(2);
    }

    [Fact]
    public async Task ReadAsync_WhenCancellationRequested_ShouldThrowOperationCanceledException()
    {
        var reader = new PerfumeJsonlReader();
        var lines = Enumerable.Range(1, 50).Select(i => $"{{\"id\": {i}, \"name\": \"Perfume {i}\"}}");
        var filePath = await CreateTempJsonlFileAsync(lines);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var act = async () =>
        {
            await foreach (var _ in reader.ReadAsync(filePath, cts.Token))
            {
            }
        };

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task ReadAsync_Metrics_ShouldCalculateAverageBytesPerLineCorrectly()
    {
        var reader = new PerfumeJsonlReader();
        var lines = new[]
        {
            "{\"id\": 1, \"name\": \"A\"}",
            "{\"id\": 2, \"name\": \"B\"}"
        };
        var filePath = await CreateTempJsonlFileAsync(lines);

        await foreach (var _ in reader.ReadAsync(filePath))
        {
        }

        reader.TotalBytesRead.Should().BeGreaterThan(0);
        reader.GetAverageBytesPerLine().Should().BeGreaterThan(0);
    }

    [Fact]
    public void ShouldYieldNext_WhenSkipAndMaxAreConfigured_ShouldRespectRules()
    {
        var options = new ParserOptions
        {
            SkipRecords = 1,
            MaxPerfumesToParse = 2
        };
        var reader = new PerfumeJsonlReader(options);

        // 1st record: skipped (SkipRecords = 1)
        reader.ShouldYieldNext().Should().BeFalse();
        reader.SkippedCount.Should().Be(1);

        // 2nd record: yielded (1st yielded)
        reader.ShouldYieldNext().Should().BeTrue();
        reader.YieldedCount.Should().Be(1);

        // 3rd record: yielded (2nd yielded, reached MaxPerfumesToParse)
        reader.ShouldYieldNext().Should().BeTrue();
        reader.YieldedCount.Should().Be(2);

        // 4th record: should not yield because max reached
        reader.ShouldYieldNext().Should().BeFalse();
        reader.YieldedCount.Should().Be(2);
    }

    [Fact]
    public void GetReaderStatusSummary_ShouldReturnFormattedString()
    {
        var reader = new PerfumeJsonlReader();

        var summary = reader.GetReaderStatusSummary();

        summary.Should().Contain("[PerfumeJsonlReader]");
        summary.Should().Contain("Processed:");
        summary.Should().Contain("yielded:");
        summary.Should().Contain("errors:");
        summary.Should().Contain("Progress:");
    }
}
