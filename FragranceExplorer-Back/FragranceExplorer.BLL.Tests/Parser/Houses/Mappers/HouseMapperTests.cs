using FluentAssertions;
using FragranceExplorer.BLL.Parser.Houses.Mappers;
using FragranceExplorer.BLL.Parser.Houses.Models;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Parser.Houses.Mappers;

public class HouseMapperTests
{
    private readonly HouseMapper _mapper = new();

    [Fact]
    public void MapToDomain_WhenDtoIsValid_ShouldMapAllPropertiesAndOlfactoryDna()
    {
        var dto = new RawHouseDto
        {
            Id = "zara",
            Name = "ZARA",
            DisplayName = "Zara",
            Initials = "ZA",
            Location = "A Coruña · Est. 1975",
            ArchiveCount = 1183,
            Rating = 4.05,
            OlfactoryDna =
            [
                new RawOlfactoryDnaDto { Family = "Fresh", Notes = "Rose · Jasmine", Percentage = 78 }
            ],
            FamilyCounts = new Dictionary<string, int> { ["All"] = 1183, ["Fresh"] = 602 },
            SimilarHouseIds = ["dolce-gabbana", "giorgio-armani"],
            BrandAliases = ["Zara", "zara"]
        };

        var house = _mapper.MapToDomain(dto);

        house.Should().NotBeNull();
        house!.Id.Should().Be("zara");
        house.Name.Should().Be("ZARA");
        house.DisplayName.Should().Be("Zara");
        house.Initials.Should().Be("ZA");
        house.ArchiveCount.Should().Be(1183);
        house.Rating.Should().Be(4.05);
        house.OlfactoryDna.Should().ContainSingle(d => d.Family == "Fresh" && d.Percentage == 78);
        house.FamilyCounts.Should().ContainKey("All").WhoseValue.Should().Be(1183);
        house.SimilarHouseIds.Should().HaveCount(2);
        house.BrandAliases.Should().HaveCount(2);
    }

    [Fact]
    public void MapToDomain_WhenOptionalFieldsAreMissing_ShouldApplyFallbacks()
    {
        var dto = new RawHouseDto
        {
            Id = "byredo",
            Name = "Byredo",
            Initials = null,
            DisplayName = null,
            Rating = null,
            ArchiveCount = 24
        };

        var house = _mapper.MapToDomain(dto);

        house.Should().NotBeNull();
        house!.DisplayName.Should().Be("Byredo");
        house.Initials.Should().Be("BY");
        house.Count.Should().Be("24 perfumes");
        house.Rating.Should().Be(0.0);
    }

    [Fact]
    public void MapToDomain_WhenIdOrNameIsMissing_ShouldReturnNull()
    {
        var dto = new RawHouseDto { Id = "", Name = "ZARA" };

        var house = _mapper.MapToDomain(dto);

        house.Should().BeNull();
    }
}

