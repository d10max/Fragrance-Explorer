using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.DataSetParser.Validators;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.DataSetParser.Validators;

public class PerfumeDtoValidatorTests
{
    private static RawPerfumeDto CreateValidDto() => new()
    {
        Id = 1,
        Name = "Perfume Name",
        Brand = "Perfume Brand",
        Gender = "unisex",
        Picture = "https://example.com/perfume.jpg",
        Thumbnail = "https://example.com/perfume.jpg",
        Rating = new RawRatingDto
        {
            Average = 4.5
        },
        Accords = new List<RawAccordDto>
        {
            new() { Name = "woody", Strength = 80 }
        },
        Notes = new RawNotesDto
        {
            Tiered = new RawNotesTieredDto
            {
                Top = new List<RawNoteDto> { new() { Name = "Bergamot", Weight = 80 } },
                Middle = new List<RawNoteDto> { new() { Name = "Rose", Weight = 60 } },
                Base = new List<RawNoteDto> { new() { Name = "Cedar", Weight = 90 } }
            }
        }
    };

    [Fact]
    public void Validate_WhenDtoIsValid_ShouldReturnTrue()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();

        var result = validator.Validate(dto, out var errors);

        result.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenDtoIsNull_ShouldReturnFalse()
    {
        var validator = new PerfumeDtoValidator();

        var result = validator.Validate(null, out var errors);

        result.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Perfume DTO cannot be null.");
    }

    [Fact]
    public void Validate_WhenIdIsZero_ShouldReturnFalse()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Id = 0;

        var result = validator.Validate(dto, out var errors);

        result.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Perfume ID must be greater than zero. Received: 0");
    }

    [Fact]
    public void Validate_WhenIdIsNegative_ShouldReturnFalse()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Id = -1;

        var result = validator.Validate(dto, out var errors);

        result.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Perfume ID must be greater than zero. Received: -1");
    }

    [Fact]
    public void Validate_WhenRatingMissing_ShouldReturnTrue()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Rating = null;

        var result = validator.Validate(dto, out var errors);

        result.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenRatingAverageIsNull_ShouldReturnTrue()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Rating = new RawRatingDto { Average = null };

        var result = validator.Validate(dto, out var errors);

        result.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenAverageIsLimitValueMin_ShouldReturnTrue()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Rating = new RawRatingDto { Average = 0.0 };

        var result = validator.Validate(dto, out var errors);

        result.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenAverageIsLimitValueMax_ShouldReturnTrue()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Rating = new RawRatingDto { Average = 5.0 };

        var result = validator.Validate(dto, out var errors);

        result.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenAverageIsNegative_ShouldReturnFalse()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Rating = new RawRatingDto { Average = -0.1 };

        var result = validator.Validate(dto, out var errors);

        result.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Perfume rating must be between 0 and 5. Received: -0.1");
    }

    [Fact]
    public void Validate_WhenAverageIsOverMaxLimitValue_ShouldReturnFalse()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Rating = new RawRatingDto { Average = 5.1 };

        var result = validator.Validate(dto, out var errors);

        result.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Perfume rating must be between 0 and 5. Received: 5.1");
    }

    [Fact]
    public void Validate_WhenUrlsAreNullOrWhitespace_ShouldReturnTrue()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Picture = null;
        dto.Thumbnail = "   ";

        var result = validator.Validate(dto, out var errors);

        result.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenPictureUrlIsInvalid_ShouldReturnFalse()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Picture = "invalid-url";

        var result = validator.Validate(dto, out var errors);

        result.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("The Picture field contains an invalid URL: invalid-url");
    }

    [Fact]
    public void Validate_WhenThumbnailUrlHasInvalidScheme_ShouldReturnFalse()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Thumbnail = "ftp://example.com/perfume.jpg";

        var result = validator.Validate(dto, out var errors);

        result.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("The Thumbnail field contains an invalid URL: ftp://example.com/perfume.jpg");
    }

    [Fact]
    public void Validate_WhenMultipleErrorsOccur_ShouldReturnFalseAndCollectAllErrors()
    {
        var validator = new PerfumeDtoValidator();
        var dto = CreateValidDto();
        dto.Id = 0;
        dto.Rating = new RawRatingDto { Average = 6.0 };
        dto.Picture = "not-a-valid-url";

        var result = validator.Validate(dto, out var errors);

        result.Should().BeFalse();
        errors.Should().HaveCount(3);
        errors.Should().Contain(new[]
        {
            "Perfume ID must be greater than zero. Received: 0",
            "Perfume rating must be between 0 and 5. Received: 6",
            "The Picture field contains an invalid URL: not-a-valid-url"
        });
    }

    [Fact]
    public void FilterValid_ShouldReturnOnlyValidPerfumes()
    {
        var validator = new PerfumeDtoValidator();
        var list = new List<RawPerfumeDto>
        {
            CreateValidDto(),
            new() { Id = 0, Name = "Invalid Id" },
            new() { Id = 2, Rating = new RawRatingDto { Average = -1.0 } },
            CreateValidDto()
        };

        var validPerfumes = validator.FilterValid(list).ToList();

        validPerfumes.Should().HaveCount(2);
    }
}