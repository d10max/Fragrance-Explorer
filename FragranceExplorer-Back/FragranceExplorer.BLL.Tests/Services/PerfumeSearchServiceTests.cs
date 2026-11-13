using FluentAssertions;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Repositories;
using FragranceExplorer.BLL.Services;
using FragranceExplorer.DAL.Enums;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Services;

public class PerfumeSearchServiceTests
{
    private static List<Perfume> CreateSampleCatalog() =>
    [
        new Perfume(1, "Acqua di Giò", "Giorgio Armani", GenderCategory.Male, 4.5, people: 2500),
        new Perfume(2, "N°5", "Chanel", GenderCategory.Female, 4.2, people: 3000),
        new Perfume(3, "Sauvage", "Dior", GenderCategory.Male, 4.4, people: 5000),
        new Perfume(4, "Baccarat Rouge 540", "Maison Francis Kurkdjian", GenderCategory.Unisex, 4.6, people: 4200),
        new Perfume(5, "L'Interdit", "Givenchy", GenderCategory.Female, 4.3, people: 1800)
    ];

    [Fact]
    public void Constructor_WhenRepositoryIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => _ = new PerfumeSearchService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Search_ShouldReturnResults_WhenQueryIsValid()
    {
        // Arrange
        var repository = new FakePerfumeRepository(CreateSampleCatalog());
        var searchService = new PerfumeSearchService(repository);

        // Act
        var results = searchService.Search("Acqua").ToList();

        // Assert
        results.Should().ContainSingle(p => p.Name == "Acqua di Giò");
    }

    [Theory]
    [InlineData("Sovage", "Sauvage")]
    [InlineData("Bakarat Ruge 540", "Baccarat Rouge 540")]
    [InlineData("Acqua di Gio", "Acqua di Giò")]
    [InlineData("L'Interdit", "L'Interdit")]
    public void Search_WhenQueryHasTyposOrMissingDiacritics_ShouldReturnExpectedTopMatch(
        string query,
        string expectedPerfumeName)
    {
        // Arrange
        var repository = new FakePerfumeRepository(CreateSampleCatalog());
        var searchService = new PerfumeSearchService(repository);

        // Act
        var results = searchService.Search(query).ToList();

        // Assert
        results.Should().NotBeEmpty();
        results[0].Name.Should().Be(expectedPerfumeName);
    }

    [Fact]
    public void Search_WhenBrandAndNameOrderIsSwapped_ShouldReturnMatchingPerfume()
    {
        // Arrange
        var repository = new FakePerfumeRepository(CreateSampleCatalog());
        var searchService = new PerfumeSearchService(repository);

        // Act
        var results = searchService.Search("Sauvage Dior").ToList();

        // Assert
        results.Should().NotBeEmpty();
        results[0].Name.Should().Be("Sauvage");
        results[0].Brand.Should().Be("Dior");
    }

    [Fact]
    public void Search_WhenQueryDoesNotMatchAnything_ShouldReturnEmpty()
    {
        // Arrange
        var repository = new FakePerfumeRepository(CreateSampleCatalog());
        var searchService = new PerfumeSearchService(repository);

        // Act
        var results = searchService.Search("zzzzxxxxqqqq12345");

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
        var repository = new FakePerfumeRepository(CreateSampleCatalog());
        var searchService = new PerfumeSearchService(repository);

        // Act
        var results = searchService.Search(query, maxResults);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void Search_WhenScoresAreEqual_ShouldOrderByPeopleThenByRating()
    {
        // Arrange
        var catalog = new List<Perfume>
        {
            new(1, "Sauvage", "Dior", GenderCategory.Male, rating: 4.1, people: 100),
            new(2, "Sauvage", "Dior", GenderCategory.Male, rating: 4.8, people: 500),
            new(3, "Sauvage", "Dior", GenderCategory.Male, rating: 4.9, people: 100)
        };
        var repository = new FakePerfumeRepository(catalog);
        var searchService = new PerfumeSearchService(repository);

        // Act
        var results = searchService.Search("Sauvage").ToList();

        // Assert
        results.Select(p => p.Id).Should().ContainInOrder(2, 3, 1);
    }

    private sealed class FakePerfumeRepository(IEnumerable<Perfume> perfumes) : IPerfumeRepository
    {
        public IEnumerable<Perfume> GetAll() => perfumes;

        public Perfume? GetById(int id) => perfumes.FirstOrDefault(p => p.Id == id);
    }
}
