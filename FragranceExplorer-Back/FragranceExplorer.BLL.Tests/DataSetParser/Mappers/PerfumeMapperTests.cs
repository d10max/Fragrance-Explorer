using FragranceExplorer.BLL.DataSetParser.Interfaces;
using FragranceExplorer.BLL.DataSetParser.Mappers;
using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Enums;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.DataSetParser.Mappers;

public class PerfumeMapperTests
{
    private readonly PerfumeMapper _mapper = new();

    private static RawPerfumeDto CreateBaseDto() => new()
    {
        Id = 1,
        Name = "Aventus",
        Brand = "Creed",
        Gender = "male",
        Picture = "https://example.com/aventus.jpg",
        Thumbnail = "https://example.com/thumb.jpg",
        Rating = new RawRatingDto { Average = 4.6 }
    };

    [Fact]
    public void Constructor_Default_ShouldInitializeSuccessfully()
    {
        var mapper = new PerfumeMapper();

        mapper.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WhenAccordMapperIsNull_ShouldThrowArgumentNullException()
    {
        var noteMapper = new NoteMapper();

        var act = () => new PerfumeMapper(null!, noteMapper);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("accordMapper");
    }

    [Fact]
    public void Constructor_WhenNoteMapperIsNull_ShouldThrowArgumentNullException()
    {
        var accordMapper = new AccordMapper();

        var act = () => new PerfumeMapper(accordMapper, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("noteMapper");
    }

    [Fact]
    public void MapToDomain_WhenDtoIsNull_ShouldReturnNull()
    {
        var result = _mapper.MapToDomain(null);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void MapToDomain_WhenIdIsZeroOrNegative_ShouldReturnNull(int invalidId)
    {
        var dto = CreateBaseDto();
        dto.Id = invalidId;

        var result = _mapper.MapToDomain(dto);

        result.Should().BeNull();
    }

    [Fact]
    public void MapToDomain_WhenDtoIsValid_ShouldMapAllCoreProperties()
    {
        var dto = CreateBaseDto();

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Aventus");
        result.Brand.Should().Be("Creed");
        result.Gender.Should().Be(GenderCategory.Male);
        result.ImageUrl.Should().Be("https://example.com/aventus.jpg");
        result.Rating.Should().Be(4.6);
    }

    [Theory]
    [InlineData("female")]
    [InlineData("Female")]
    [InlineData("  FEMALE  ")]
    public void MapToDomain_WhenGenderIsFemale_ShouldMapToFemale(string genderInput)
    {
        var dto = CreateBaseDto();
        dto.Gender = genderInput;

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Gender.Should().Be(GenderCategory.Female);
    }

    [Theory]
    [InlineData("male")]
    [InlineData("Male")]
    [InlineData("  MALE  ")]
    public void MapToDomain_WhenGenderIsMale_ShouldMapToMale(string genderInput)
    {
        var dto = CreateBaseDto();
        dto.Gender = genderInput;

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Gender.Should().Be(GenderCategory.Male);
    }

    [Theory]
    [InlineData("unisex")]
    [InlineData("Unisex")]
    [InlineData("other")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void MapToDomain_WhenGenderIsUnisexOrUnknownOrNull_ShouldMapToUnisex(string? genderInput)
    {
        var dto = CreateBaseDto();
        dto.Gender = genderInput!;

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Gender.Should().Be(GenderCategory.Unisex);
    }

    [Fact]
    public void MapToDomain_WhenBothPictureAndThumbnailProvided_ShouldPreferPicture()
    {
        var dto = CreateBaseDto();
        dto.Picture = "https://example.com/main.png";
        dto.Thumbnail = "https://example.com/thumb.png";

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.ImageUrl.Should().Be("https://example.com/main.png");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MapToDomain_WhenPictureIsMissingOrWhitespace_ShouldFallbackToThumbnail(string? emptyPicture)
    {
        var dto = CreateBaseDto();
        dto.Picture = emptyPicture;
        dto.Thumbnail = "  https://example.com/thumb.png  ";

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.ImageUrl.Should().Be("https://example.com/thumb.png");
    }

    [Fact]
    public void MapToDomain_WhenBothPictureAndThumbnailAreMissingOrWhitespace_ShouldSetImageUrlToNull()
    {
        var dto = CreateBaseDto();
        dto.Picture = null;
        dto.Thumbnail = "   ";

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.ImageUrl.Should().BeNull();
    }

    [Fact]
    public void MapToDomain_WhenRatingIsNull_ShouldDefaultRatingToZero()
    {
        var dto = CreateBaseDto();
        dto.Rating = null;

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Rating.Should().Be(0.0);
    }

    [Fact]
    public void MapToDomain_WhenRatingAverageIsNull_ShouldDefaultRatingToZero()
    {
        var dto = CreateBaseDto();
        dto.Rating = new RawRatingDto { Average = null };

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Rating.Should().Be(0.0);
    }

    [Fact]
    public void MapToDomain_WhenRatingExceedsMax_ShouldClampToFive()
    {
        var dto = CreateBaseDto();
        dto.Rating = new RawRatingDto { Average = 6.8 };

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Rating.Should().Be(5.0);
    }

    [Fact]
    public void MapToDomain_WhenRatingIsNegative_ShouldClampToZero()
    {
        var dto = CreateBaseDto();
        dto.Rating = new RawRatingDto { Average = -1.5 };

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Rating.Should().Be(0.0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MapToDomain_WhenNameIsNullOrWhitespace_ShouldUseFallback(string? emptyName)
    {
        var dto = CreateBaseDto();
        dto.Name = emptyName!;

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Unknown Perfume");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MapToDomain_WhenBrandIsNullOrWhitespace_ShouldUseFallback(string? emptyBrand)
    {
        var dto = CreateBaseDto();
        dto.Brand = emptyBrand!;

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Brand.Should().Be("Unknown Brand");
    }

    [Fact]
    public void MapToDomain_WhenNameAndBrandHaveWhitespace_ShouldTrimThem()
    {
        var dto = CreateBaseDto();
        dto.Name = "   Sauvage   ";
        dto.Brand = "   Dior   ";

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Sauvage");
        result.Brand.Should().Be("Dior");
    }

    [Fact]
    public void MapToDomain_WhenAccordsAndNotesArePresent_ShouldMapAndAddThemToPerfume()
    {
        var dto = CreateBaseDto();
        dto.Accords = new List<RawAccordDto>
        {
            new() { Name = "citrus", Strength = 85 },
            new() { Name = "woody", Strength = 70 }
        };
        dto.Notes = new RawNotesDto
        {
            Tiered = new RawNotesTieredDto
            {
                Top = new List<RawNoteDto> { new() { Name = "Lemon", Weight = 80 } },
                Base = new List<RawNoteDto> { new() { Name = "Cedar", Weight = 90 } }
            }
        };

        var result = _mapper.MapToDomain(dto);

        result.Should().NotBeNull();
        result!.Accords.Should().HaveCount(2);
        result.Accords.Select(a => a.Name).Should().Contain(new[] { "citrus", "woody" });

        result.Notes.Should().HaveCount(2);
        result.Notes.Select(n => n.Name).Should().Contain(new[] { "Lemon", "Cedar" });
    }
}
