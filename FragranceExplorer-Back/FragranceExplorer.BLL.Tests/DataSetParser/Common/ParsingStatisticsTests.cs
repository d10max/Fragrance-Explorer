using FragranceExplorer.BLL.DataSetParser.Common;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.DataSetParser.Common;

public class ParsingStatisticsTests
{
    [Fact]
    public void InitialState_ShouldHaveZeroCountersAndZeroMetrics()
    {
        var stats = new ParsingStatistics();

        stats.TotalLinesRead.Should().Be(0);
        stats.SuccessfulParses.Should().Be(0);
        stats.FailedParses.Should().Be(0);
        stats.CalculateSuccessRate().Should().Be(0.0);
        stats.CalculateSpeedLinesPerSecond().Should().Be(0.0);
        stats.CalculateThroughputMbPerSecond().Should().Be(0.0);
    }

    [Fact]
    public void RecordSuccess_ShouldIncrementTotalAndSuccessfulCounts()
    {
        var stats = new ParsingStatistics();

        stats.RecordSuccess(1024);

        stats.TotalLinesRead.Should().Be(1);
        stats.SuccessfulParses.Should().Be(1);
        stats.FailedParses.Should().Be(0);
    }

    [Fact]
    public void RecordFailure_ShouldIncrementTotalAndFailedCounts()
    {
        var stats = new ParsingStatistics();

        stats.RecordFailure(512);

        stats.TotalLinesRead.Should().Be(1);
        stats.SuccessfulParses.Should().Be(0);
        stats.FailedParses.Should().Be(1);
    }

    [Fact]
    public void CalculateSuccessRate_WhenAllSuccessful_ShouldReturn100()
    {
        var stats = new ParsingStatistics();

        stats.RecordSuccess();
        stats.RecordSuccess();
        stats.RecordSuccess();

        stats.CalculateSuccessRate().Should().Be(100.0);
    }

    [Fact]
    public void CalculateSuccessRate_WhenAllFailed_ShouldReturnZero()
    {
        var stats = new ParsingStatistics();

        stats.RecordFailure();
        stats.RecordFailure();

        stats.CalculateSuccessRate().Should().Be(0.0);
    }

    [Fact]
    public void CalculateSuccessRate_WhenMixed_ShouldCalculateCorrectPercentage()
    {
        var stats = new ParsingStatistics();

        stats.RecordSuccess();
        stats.RecordSuccess();
        stats.RecordSuccess();
        stats.RecordFailure();

        // 3 out of 4 = 75.0%
        stats.CalculateSuccessRate().Should().Be(75.0);
    }

    [Fact]
    public void CalculateSuccessRate_ShouldRoundToTwoDecimalPlaces()
    {
        var stats = new ParsingStatistics();

        stats.RecordSuccess();
        stats.RecordSuccess();
        stats.RecordFailure();

        // 2 out of 3 = 66.666... -> 66.67%
        stats.CalculateSuccessRate().Should().Be(66.67);
    }

    [Fact]
    public async Task StartAndStop_ShouldMeasureElapsedTime()
    {
        var stats = new ParsingStatistics();

        stats.Start();
        await Task.Delay(50);
        stats.Stop();

        stats.ElapsedTime.Should().BeGreaterThan(TimeSpan.Zero);
    }

    [Fact]
    public void CalculateSpeedAndThroughput_WhenNoLinesRead_ShouldReturnZero()
    {
        var stats = new ParsingStatistics();

        stats.CalculateSpeedLinesPerSecond().Should().Be(0.0);
        stats.CalculateThroughputMbPerSecond().Should().Be(0.0);
    }

    [Fact]
    public void GetSummary_ShouldContainAllExpectedMetricsAndLabels()
    {
        var stats = new ParsingStatistics();
        stats.Start();
        stats.RecordSuccess(1024);
        stats.RecordFailure(512);
        stats.Stop();

        var summary = stats.GetSummary();

        summary.Should().Contain("Dataset Parsing Results");
        summary.Should().Contain("Elapsed time:");
        summary.Should().Contain("Lines read: 2");
        summary.Should().Contain("Successfully processed: 1");
        summary.Should().Contain("Skipped/Errors: 1");
        summary.Should().Contain("Success rate: 50%");
        summary.Should().Contain("Speed:");
    }
}
