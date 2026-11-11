using System.Runtime.CompilerServices;
using FragranceExplorer.BLL.Parser.Common;
using FragranceExplorer.BLL.Parser.Common.Interfaces;
using FragranceExplorer.BLL.Parser.Perfumes.Interfaces;
using FragranceExplorer.BLL.Parser.Perfumes.Mappers;
using FragranceExplorer.BLL.Parser.Perfumes.Models;
using FragranceExplorer.BLL.Parser.Perfumes.Readers;
using FragranceExplorer.BLL.Parser.Perfumes.Validators;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Parser.Perfumes.Services;

public class PerfumeDataSetParser : IPerfumeDataSetParser
{
    private readonly IPerfumeMapper _perfumeMapper;
    private readonly IDataSetReader<RawPerfumeDto> _reader;
    private readonly IValidator<RawPerfumeDto> _validator;
    private readonly ParsingStatistics _statistics;

    public ParsingStatistics Statistics => _statistics;

    public PerfumeDataSetParser(
        IPerfumeMapper perfumeMapper,
        IDataSetReader<RawPerfumeDto> reader,
        IValidator<RawPerfumeDto> validator,
        ParsingStatistics statistics)
    {
        _perfumeMapper = perfumeMapper ?? throw new ArgumentNullException(nameof(perfumeMapper));
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _statistics = statistics ?? throw new ArgumentNullException(nameof(statistics));
    }

    public PerfumeDataSetParser(ParserOptions? options = null) : this(new PerfumeMapper(), new PerfumeJsonlReader(options), new PerfumeDtoValidator(), new ParsingStatistics())
    {

    }

    public async IAsyncEnumerable<Perfume> ParseAsync(ParserOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        options ??= new ParserOptions();
        _statistics.Start();

        long skipped = 0;
        long yielded = 0;

        await foreach (var rawPerfume in _reader.ReadAsync(options.PathToDataset, cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!_validator.Validate(rawPerfume, out _))
            {
                _statistics.RecordFailure();
                continue;
            }

            Perfume? perfume;
            try
            {
                perfume = _perfumeMapper.MapToDomain(rawPerfume);
            }
            catch (Exception)
            {
                _statistics.RecordFailure();
                continue;
            }

            if (perfume is null)
            {
                _statistics.RecordFailure();
                continue;
            }

            if (skipped < options.SkipRecords)
            {
                skipped++;
                continue;
            }

            _statistics.RecordSuccess();
            yielded++;
            yield return perfume;

            if (options.MaxPerfumesToParse.HasValue && yielded >= options.MaxPerfumesToParse.Value)
            {
                yield break;
            }
        }

        _statistics.Stop();
    }

    public string GetStatisticsReport()
    {
        return _statistics.GetSummary();
    }
}
