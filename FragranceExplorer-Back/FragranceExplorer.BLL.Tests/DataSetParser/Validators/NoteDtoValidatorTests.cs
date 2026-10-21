using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.DataSetParser.Validators;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.DataSetParser.Validators;

public class NoteDtoValidatorTests
{
    [Fact]
    public void Validate_WhenNoteDtoIsValid_ShouldReturnTrueAndNoErrors()
    {
        var validator = new NoteDtoValidator();
        var dto = new RawNoteDto
        {
            Name = "Bergamot",
            Slug = "bergamot",
            Weight = 80
        };

        bool isValid = validator.Validate(dto, out var errors);

        isValid.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenNoteDtoIsNull_ShouldReturnFalseAndAddError() {
        var validator = new NoteDtoValidator();
        bool isValid = validator.Validate(null, out var errors);

        isValid.Should().BeFalse();
        
        errors.Should().ContainSingle().Which.Should().Be("Note DTO cannot be null.");
    }

    [Fact]
    public void Validate_WhenNoteDtoHasNoName_ShouldReturnFalseAndAddError() {
        var validator = new NoteDtoValidator();
        var dto = new RawNoteDto
        {
            Name = "",
            Slug = "bergamot",
            Weight = 80
        };

        bool isValid = validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Note name cannot be empty.");
    }

    [Fact]
    public void Validate_WhenWeightLessThenZero_ShouldReturnFalseAndAddError(){
        var validator = new NoteDtoValidator();
        var dto = new RawNoteDto{
            Name = "Layton",
            Slug = "vanila",
            Weight = -1
        };

        bool isValid = validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Note weight must be between 0 and 100. Received: -1");
    }

    [Fact]
    public void Validate_WhenWeightMoreThen100_ShouldReturnFalseAndAddError(){
        var validator = new NoteDtoValidator();
        var dto = new RawNoteDto{
            Name = "Layton",
            Slug = "vanila",
            Weight = 101
        };

        bool isValid = validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Note weight must be between 0 and 100. Received: 101");
    }

    [Fact]
    public void Validate_WhenNoteDtoHasMultipleErrors_ShouldReturnFalseAndAddAllErrors(){
        var validator = new NoteDtoValidator();
        var dto = new RawNoteDto{
            Name = "",
            Slug = "vanila",
            Weight = -1
        };

        bool isValid = validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().HaveCount(2);
        errors.Should().Contain(new string[] {
            "Note name cannot be empty.",
            "Note weight must be between 0 and 100. Received: -1"
        });
    }

    [Fact]
    public void Validate_WhenNoteNameIsWhitespace_ShouldReturnFalseAndAddError()
    {
        var validator = new NoteDtoValidator();
        var dto = new RawNoteDto { Name = "   ", Weight = 50 };

        bool isValid = validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be("Note name cannot be empty.");
    }

    [Fact]
    public void FilterValid_ShouldReturnOnlyValidNotes(){
        var validator = new NoteDtoValidator();
        var list = new List<RawNoteDto>
        {
        new() { Name = "Rose", Weight = 50 },      // Valid
        new() { Name = "", Weight = 50 },          // Invalid (no name)
        new() { Name = "Amber", Weight = -1 },     // Invalid (weight < 0)
        new() { Name = "Bergamot", Weight = 100 }  // Valid
    };

        var validNotes = validator.FilterValid(list).ToList();

        validNotes.Should().HaveCount(2);
        validNotes.Select(n => n.Name).Should().Contain(new[] { "Rose", "Bergamot" });
    }


}
