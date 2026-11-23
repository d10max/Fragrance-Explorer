using FluentAssertions;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Services;
using FragranceExplorer.DAL.Entities;

namespace FragranceExplorer.BLL.Tests.Services;

public class PerfumeSearchServiceTests
{
    private readonly PerfumeSearchService _searchService = new();

    private static PerfumeSearchEntry CreateEntry(int id, string name, string brand, double rating, int people) =>
        PerfumeSearchEntry.FromSearchData(new PerfumeSearchData(id, name, brand, people, rating));

    private static List<PerfumeSearchEntry> CreateSampleCatalog() =>
    [
        CreateEntry(1, "Acqua di Giò", "Giorgio Armani", rating: 4.5, people: 2500),
        CreateEntry(2, "N°5", "Chanel", rating: 4.2, people: 3000),
        CreateEntry(3, "Sauvage", "Dior", rating: 4.4, people: 5000),
        CreateEntry(4, "Baccarat Rouge 540", "Maison Francis Kurkdjian", rating: 4.6, people: 4200),
        CreateEntry(5, "L'Interdit", "Givenchy", rating: 4.3, people: 1800)
    ];

    [Fact]
    public void Search_WhenCatalogIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => _searchService.Search("Acqua", null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Search_ShouldReturnResults_WhenQueryIsValid()
    {
        // Arrange
        var catalog = CreateSampleCatalog();

        // Act
        var results = _searchService.Search("Acqua", catalog);

        // Assert
        results.Should().ContainSingle().Which.Should().Be(1);
    }

    [Theory]
    [InlineData("Sovage", 3)]               // Sauvage
    [InlineData("Bakarat Ruge 540", 4)]     // Baccarat Rouge 540
    [InlineData("Acqua di Gio", 1)]         // Acqua di Giò
    [InlineData("L'Interdit", 5)]           // L'Interdit
    public void Search_WhenQueryHasTyposOrMissingDiacritics_ShouldReturnExpectedTopMatch(
        string query,
        int expectedPerfumeId)
    {
        // Arrange
        var catalog = CreateSampleCatalog();

        // Act
        var results = _searchService.Search(query, catalog);

        // Assert
        results.Should().NotBeEmpty();
        results.First().Should().Be(expectedPerfumeId);
    }

    [Fact]
    public void Search_WhenBrandAndNameOrderIsSwapped_ShouldReturnMatchingPerfume()
    {
        // Arrange
        var catalog = CreateSampleCatalog();

        // Act
        var results = _searchService.Search("Sauvage Dior", catalog);

        // Assert
        results.Should().NotBeEmpty();
        results.First().Should().Be(3); // Sauvage by Dior
    }

    [Fact]
    public void Search_WhenQueryDoesNotMatchAnything_ShouldReturnEmpty()
    {
        // Arrange
        var catalog = CreateSampleCatalog();

        // Act
        var results = _searchService.Search("zzzzxxxxqqqq12345", catalog);

        // Assert
        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData("", 10)]
    [InlineData("   ", 10)]
    [InlineData("Sauvage", 0)]
    [InlineData("Sauvage", -5)]
    public void Search_WhenQueryIsBlankOrMaxResultsIsNonPositive_ShouldReturnEmpty(
        string query,
        int maxResults)
    {
        // Arrange
        var catalog = CreateSampleCatalog();

        // Act
        var results = _searchService.Search(query, catalog, maxResults);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void Search_WhenScoresAreEqual_ShouldOrderByPeopleThenByRating()
    {
        // Arrange
        var catalog = new List<PerfumeSearchEntry>
        {
            CreateEntry(1, "Sauvage", "Dior", rating: 4.1, people: 100),
            CreateEntry(2, "Sauvage", "Dior", rating: 4.8, people: 500),
            CreateEntry(3, "Sauvage", "Dior", rating: 4.9, people: 100)
        };

        // Act
        var results = _searchService.Search("Sauvage", catalog);

        // Assert
        results.Should().ContainInOrder(2, 3, 1);
    }
}
