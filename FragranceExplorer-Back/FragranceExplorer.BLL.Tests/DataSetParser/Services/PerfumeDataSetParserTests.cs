using FragranceExplorer.BLL.DataSetParser.Common;
using FragranceExplorer.BLL.DataSetParser.Mappers;
using FragranceExplorer.BLL.DataSetParser.Readers;
using FragranceExplorer.BLL.DataSetParser.Services;
using FragranceExplorer.BLL.DataSetParser.Validators;
using FragranceExplorer.BLL.Models;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.DataSetParser.Services;

public class PerfumeDataSetParserTests : IDisposable
{
    private readonly List<string> _tempFilesToCleanup = new();

    private async Task<string> CreateTempJsonlFileAsync(IEnumerable<string> lines)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"parser_test_{Guid.NewGuid():N}.jsonl");
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
                    // Ignore cleanup errors
                }
            }
        }
    }

    [Fact]
    public void Constructor_Default_ShouldInitializeSuccessfully()
    {
        var parser = new PerfumeDataSetParser();

        parser.Should().NotBeNull();
        parser.Statistics.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WhenPerfumeMapperIsNull_ShouldThrowArgumentNullException()
    {
        var act = () => new PerfumeDataSetParser(
            null!,
            new PerfumeJsonlReader(),
            new PerfumeDtoValidator(),
            new ParsingStatistics());

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("perfumeMapper");
    }

    [Fact]
    public void Constructor_WhenReaderIsNull_ShouldThrowArgumentNullException()
    {
        var act = () => new PerfumeDataSetParser(
            new PerfumeMapper(),
            null!,
            new PerfumeDtoValidator(),
            new ParsingStatistics());

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("reader");
    }

    [Fact]
    public void Constructor_WhenValidatorIsNull_ShouldThrowArgumentNullException()
    {
        var act = () => new PerfumeDataSetParser(
            new PerfumeMapper(),
            new PerfumeJsonlReader(),
            null!,
            new ParsingStatistics());

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("validator");
    }

    [Fact]
    public void Constructor_WhenStatisticsIsNull_ShouldThrowArgumentNullException()
    {
        var act = () => new PerfumeDataSetParser(
            new PerfumeMapper(),
            new PerfumeJsonlReader(),
            new PerfumeDtoValidator(),
            null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("statistics");
    }

    [Fact]
    public async Task ParseAsync_WhenValidDatasetProvided_ShouldYieldPerfumesAndRecordStatistics()
    {
        var lines = new[]
        {
            "{\"id\": 1, \"name\": \"Aventus\", \"brand\": \"Creed\", \"gender\": \"male\", \"rating\": {\"average\": 4.6}}",
            "{\"id\": 2, \"name\": \"Sauvage\", \"brand\": \"Dior\", \"gender\": \"male\", \"rating\": {\"average\": 4.5}}"
        };
        var filePath = await CreateTempJsonlFileAsync(lines);
        var options = new ParserOptions { PathToDataset = filePath };
        var parser = new PerfumeDataSetParser(options);

        var perfumes = new List<Perfume>();
        await foreach (var p in parser.ParseAsync(options))
        {
            perfumes.Add(p);
        }

        perfumes.Should().HaveCount(2);
        perfumes.Select(p => p.Name).Should().Contain(new[] { "Aventus", "Sauvage" });
        parser.Statistics.SuccessfulParses.Should().Be(2);
        parser.Statistics.FailedParses.Should().Be(0);
        parser.Statistics.CalculateSuccessRate().Should().Be(100.0);
    }

    [Fact]
    public async Task ParseAsync_WhenRecordIsInvalid_ShouldSkipAndRecordFailure()
    {
        var lines = new[]
        {
            "{\"id\": 1, \"name\": \"Valid 1\", \"brand\": \"Brand\"}",
            "{\"id\": 0, \"name\": \"Invalid Id 0\", \"brand\": \"Brand\"}", // Fails validator (Id <= 0)
            "{\"id\": 2, \"name\": \"Valid 2\", \"brand\": \"Brand\"}"
        };
        var filePath = await CreateTempJsonlFileAsync(lines);
        var options = new ParserOptions { PathToDataset = filePath };
        var parser = new PerfumeDataSetParser(options);

        var perfumes = new List<Perfume>();
        await foreach (var p in parser.ParseAsync(options))
        {
            perfumes.Add(p);
        }

        perfumes.Should().HaveCount(2);
        perfumes.Select(p => p.Name).Should().Contain(new[] { "Valid 1", "Valid 2" });
        parser.Statistics.SuccessfulParses.Should().Be(2);
        parser.Statistics.FailedParses.Should().Be(1);
    }

    [Fact]
    public async Task ParseAsync_WhenSkipRecordsIsConfigured_ShouldSkipSpecifiedCount()
    {
        var lines = new[]
        {
            "{\"id\": 1, \"name\": \"First\", \"brand\": \"B\"}",
            "{\"id\": 2, \"name\": \"Second\", \"brand\": \"B\"}",
            "{\"id\": 3, \"name\": \"Third\", \"brand\": \"B\"}"
        };
        var filePath = await CreateTempJsonlFileAsync(lines);
        var options = new ParserOptions
        {
            PathToDataset = filePath,
            SkipRecords = 1
        };
        var parser = new PerfumeDataSetParser(options);

        var perfumes = new List<Perfume>();
        await foreach (var p in parser.ParseAsync(options))
        {
            perfumes.Add(p);
        }

        perfumes.Should().HaveCount(2);
        perfumes.Select(p => p.Name).Should().Contain(new[] { "Second", "Third" });
    }

    [Fact]
    public async Task ParseAsync_WhenMaxPerfumesToParseIsConfigured_ShouldStopAfterLimit()
    {
        var lines = new[]
        {
            "{\"id\": 1, \"name\": \"Perfume 1\", \"brand\": \"B\"}",
            "{\"id\": 2, \"name\": \"Perfume 2\", \"brand\": \"B\"}",
            "{\"id\": 3, \"name\": \"Perfume 3\", \"brand\": \"B\"}",
            "{\"id\": 4, \"name\": \"Perfume 4\", \"brand\": \"B\"}"
        };
        var filePath = await CreateTempJsonlFileAsync(lines);
        var options = new ParserOptions
        {
            PathToDataset = filePath,
            MaxPerfumesToParse = 2
        };
        var parser = new PerfumeDataSetParser(options);

        var perfumes = new List<Perfume>();
        await foreach (var p in parser.ParseAsync(options))
        {
            perfumes.Add(p);
        }

        perfumes.Should().HaveCount(2);
        perfumes.Select(p => p.Name).Should().Contain(new[] { "Perfume 1", "Perfume 2" });
    }

    [Fact]
    public async Task ParseAsync_WhenCancellationRequested_ShouldThrowOperationCanceledException()
    {
        var lines = Enumerable.Range(1, 20).Select(i => $"{{\"id\": {i}, \"name\": \"Perfume {i}\", \"brand\": \"B\"}}");
        var filePath = await CreateTempJsonlFileAsync(lines);
        var options = new ParserOptions { PathToDataset = filePath };
        var parser = new PerfumeDataSetParser(options);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var act = async () =>
        {
            await foreach (var _ in parser.ParseAsync(options, cts.Token))
            {
            }
        };

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void GetStatisticsReport_ShouldReturnSummaryString()
    {
        var parser = new PerfumeDataSetParser();

        var report = parser.GetStatisticsReport();

        report.Should().Contain("Dataset Parsing Results");
        report.Should().Contain("Lines read: 0");
    }
}
