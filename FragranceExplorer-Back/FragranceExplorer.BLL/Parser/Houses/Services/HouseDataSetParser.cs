using System.Runtime.CompilerServices;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Common;
using FragranceExplorer.BLL.Parser.Common.Interfaces;
using FragranceExplorer.BLL.Parser.Houses.Interfaces;
using FragranceExplorer.BLL.Parser.Houses.Mappers;
using FragranceExplorer.BLL.Parser.Houses.Models;
using FragranceExplorer.BLL.Parser.Houses.Readers;
using FragranceExplorer.BLL.Parser.Houses.Validators;

namespace FragranceExplorer.BLL.Parser.Houses.Services;

public class HouseDataSetParser : IHouseDataSetParser
{
    private readonly IHouseMapper _houseMapper;
    private readonly IDataSetReader<RawHouseDto> _reader;
    private readonly IValidator<RawHouseDto> _validator;
    private readonly ParsingStatistics _statistics;

    public ParsingStatistics Statistics => _statistics;

    public HouseDataSetParser(
        IHouseMapper houseMapper,
        IDataSetReader<RawHouseDto> reader,
        IValidator<RawHouseDto> validator,
        ParsingStatistics statistics)
    {
        _houseMapper = houseMapper ?? throw new ArgumentNullException(nameof(houseMapper));
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _statistics = statistics ?? throw new ArgumentNullException(nameof(statistics));
    }

    public HouseDataSetParser(ParserOptions? options = null)
        : this(new HouseMapper(), new HouseJsonlReader(options), new HouseDtoValidator(), new ParsingStatistics())
    {
    }

    public async IAsyncEnumerable<House> ParseAsync(
        ParserOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        options ??= new ParserOptions { PathToDataset = "house_catalog.jsonl" };
        _statistics.Start();

        long skipped = 0;
        long yielded = 0;

        await foreach (var rawHouse in _reader.ReadAsync(options.PathToDataset, cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!_validator.Validate(rawHouse, out _))
            {
                _statistics.RecordFailure();
                continue;
            }

            House? house;
            try
            {
                house = _houseMapper.MapToDomain(rawHouse);
            }
            catch (Exception)
            {
                _statistics.RecordFailure();
                continue;
            }

            if (house is null)
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
            yield return house;

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