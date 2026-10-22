using FragranceExplorer.BLL.DataSetParser.Mappers;
using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Enums;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.DataSetParser.Mappers;

public class NoteMapperTests
{
    private readonly NoteMapper _mapper = new();

    [Fact]
    public void Map_WhenDtoIsValid_ShouldReturnPerfumeNoteWithNormalizedIntensityAndLayer()
    {
        var dto = new RawNoteDto
        {
            Name = "Bergamot",
            Weight = 80
        };

        var result = _mapper.Map(dto, NoteLayer.Top);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Bergamot");
        result.Intensity.Should().BeApproximately(0.8, precision: 1e-5);
        result.NoteLayer.Should().Be(NoteLayer.Top);
    }

    [Fact]
    public void Map_WhenNameHasWhitespace_ShouldTrimName()
    {
        var dto = new RawNoteDto
        {
            Name = "   Rose   ",
            Weight = 60
        };

        var result = _mapper.Map(dto, NoteLayer.Middle);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Rose");
        result.Intensity.Should().BeApproximately(0.6, precision: 1e-5);
        result.NoteLayer.Should().Be(NoteLayer.Middle);
    }

    [Fact]
    public void Map_WhenDtoIsNull_ShouldReturnNull()
    {
        var result = _mapper.Map(null, NoteLayer.Base);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Map_WhenNameIsNullOrWhitespace_ShouldReturnNull(string? invalidName)
    {
        var dto = new RawNoteDto
        {
            Name = invalidName!,
            Weight = 50
        };

        var result = _mapper.Map(dto, NoteLayer.Top);

        result.Should().BeNull();
    }

    [Fact]
    public void Map_WhenWeightExceeds100_ShouldClampIntensityToOne()
    {
        var dto = new RawNoteDto
        {
            Name = "Amber",
            Weight = 140
        };

        var result = _mapper.Map(dto, NoteLayer.Base);

        result.Should().NotBeNull();
        result!.Intensity.Should().Be(1.0);
    }

    [Fact]
    public void Map_WhenWeightIsNegative_ShouldClampIntensityToZero()
    {
        var dto = new RawNoteDto
        {
            Name = "Patchouli",
            Weight = -30
        };

        var result = _mapper.Map(dto, NoteLayer.Base);

        result.Should().NotBeNull();
        result!.Intensity.Should().Be(0.0);
    }

    [Fact]
    public void Map_WhenNoteLayerIsInvalid_ShouldThrowArgumentOutOfRangeException()
    {
        var dto = new RawNoteDto
        {
            Name = "Vanilla",
            Weight = 50
        };

        var act = () => _mapper.Map(dto, (NoteLayer)999);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MapNotes_WhenDtoIsNull_ShouldReturnEmptySequence()
    {
        var result = _mapper.MapNotes(null);

        result.Should().BeEmpty();
    }

    [Fact]
    public void MapNotes_WhenTieredNotesProvided_ShouldMapTopMiddleAndBaseWithCorrectLayers()
    {
        var dto = new RawNotesDto
        {
            Tiered = new RawNotesTieredDto
            {
                Top = new List<RawNoteDto>
                {
                    new() { Name = "Lemon", Weight = 80 },
                    new() { Name = "Mint", Weight = 70 }
                },
                Middle = new List<RawNoteDto>
                {
                    new() { Name = "Lavender", Weight = 60 }
                },
                Base = new List<RawNoteDto>
                {
                    new() { Name = "Cedar", Weight = 90 }
                }
            }
        };

        var result = _mapper.MapNotes(dto).ToList();

        result.Should().HaveCount(4);

        var topNotes = result.Where(n => n.NoteLayer == NoteLayer.Top).ToList();
        topNotes.Should().HaveCount(2);
        topNotes.Select(n => n.Name).Should().Contain(new[] { "Lemon", "Mint" });

        var middleNotes = result.Where(n => n.NoteLayer == NoteLayer.Middle).ToList();
        middleNotes.Should().ContainSingle().Which.Name.Should().Be("Lavender");

        var baseNotes = result.Where(n => n.NoteLayer == NoteLayer.Base).ToList();
        baseNotes.Should().ContainSingle().Which.Name.Should().Be("Cedar");
    }

    [Fact]
    public void MapNotes_WhenFlatNotesProvided_ShouldMapWithFlatLayer()
    {
        var dto = new RawNotesDto
        {
            Flat = new List<RawNoteDto>
            {
                new() { Name = "Musk", Weight = 50 },
                new() { Name = "Sandalwood", Weight = 85 }
            }
        };

        var result = _mapper.MapNotes(dto).ToList();

        result.Should().HaveCount(2);
        result.Should().AllSatisfy(n => n.NoteLayer.Should().Be(NoteLayer.Flat));
        result.Select(n => n.Name).Should().Contain(new[] { "Musk", "Sandalwood" });
    }

    [Fact]
    public void MapNotes_WhenBothTieredAndFlatProvided_ShouldMapAllNotes()
    {
        var dto = new RawNotesDto
        {
            Tiered = new RawNotesTieredDto
            {
                Top = new List<RawNoteDto> { new() { Name = "Orange", Weight = 60 } },
                Middle = new List<RawNoteDto> { new() { Name = "Jasmine", Weight = 75 } },
                Base = new List<RawNoteDto> { new() { Name = "Vanilla", Weight = 90 } }
            },
            Flat = new List<RawNoteDto>
            {
                new() { Name = "Sea Salt", Weight = 40 }
            }
        };

        var result = _mapper.MapNotes(dto).ToList();

        result.Should().HaveCount(4);
        result.Select(n => n.NoteLayer).Should().Contain(new[]
        {
            NoteLayer.Top,
            NoteLayer.Middle,
            NoteLayer.Base,
            NoteLayer.Flat
        });
    }

    [Fact]
    public void MapNotes_WhenListsContainInvalidOrNullNotes_ShouldSkipInvalidAndReturnOnlyValid()
    {
        var dto = new RawNotesDto
        {
            Tiered = new RawNotesTieredDto
            {
                Top = new List<RawNoteDto?>
                {
                    new() { Name = "Valid Top", Weight = 50 },
                    null,
                    new() { Name = "   ", Weight = 50 }
                }!,
                Middle = null,
                Base = new List<RawNoteDto?>
                {
                    new() { Name = "Valid Base", Weight = 70 }
                }!
            }
        };

        var result = _mapper.MapNotes(dto).ToList();

        result.Should().HaveCount(2);
        result.Select(n => n.Name).Should().Contain(new[] { "Valid Top", "Valid Base" });
    }
}
