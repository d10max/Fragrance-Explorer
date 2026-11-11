using FluentAssertions;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Common;
using FragranceExplorer.BLL.Parser.Houses.Services;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Parser.Houses.Services;

public class HouseDataSetParserTests : IDisposable
{
    private readonly List<string> _tempFilesToCleanup = new();

    private async Task<string> CreateTempJsonlFileAsync(IEnumerable<string> lines)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"house_parser_test_{Guid.NewGuid():N}.jsonl");
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
                try { File.Delete(file); } catch { }
            }
        }
    }

    [Fact]
    public async Task ParseAsync_WhenFileHasValidAndInvalidLines_ShouldYieldOnlyValidHousesAndTrackStatistics()
    {
        var lines = new[]
        {
            """{"id": "zara", "name": "ZARA", "displayName": "Zara", "archiveCount": 1183, "rating": 4.05, "olfactoryDna": [{"family": "Fresh", "notes": "Rose · Jasmine", "percentage": 78}], "similarHouseIds": ["avon"]}""",
            """{"id": "", "name": "BROKEN HOUSE", "rating": 4.0}""",
            """{"id": "avon", "name": "AVON", "displayName": "Avon", "archiveCount": 1137, "rating": 3.78}"""
        };

        var filePath = await CreateTempJsonlFileAsync(lines);
        var options = new ParserOptions { PathToDataset = filePath };
        var parser = new HouseDataSetParser(options);

        var results = new List<House>();
        await foreach (var house in parser.ParseAsync(options))
        {
            results.Add(house);
        }

        results.Should().HaveCount(2);
        results[0].Id.Should().Be("zara");
        results[0].OlfactoryDna.Should().ContainSingle();
        results[1].Id.Should().Be("avon");

        parser.Statistics.SuccessfulParses.Should().Be(2);
        parser.Statistics.FailedParses.Should().Be(1);
    }

    [Fact]
    public async Task ParseAsync_WithSkipAndMaxLimit_ShouldRespectOptions()
    {
        var lines = new[]
        {
            """{"id": "zara", "name": "ZARA", "rating": 4.05}""",
            """{"id": "avon", "name": "AVON", "rating": 3.78}""",
            """{"id": "byredo", "name": "BYREDO", "rating": 4.25}"""
        };

        var filePath = await CreateTempJsonlFileAsync(lines);
        var options = new ParserOptions
        {
            PathToDataset = filePath,
            SkipRecords = 1,
            MaxPerfumesToParse = 1
        };
        var parser = new HouseDataSetParser(options);

        var results = new List<House>();
        await foreach (var house in parser.ParseAsync(options))
        {
            results.Add(house);
        }

        results.Should().ContainSingle();
        results[0].Id.Should().Be("avon");
    }
}

