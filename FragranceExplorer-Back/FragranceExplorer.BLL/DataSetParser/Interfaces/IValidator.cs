namespace FragranceExplorer.BLL.DataSetParser.Interfaces;

public interface IValidator<T> where T : class
{
    bool Validate(T? item, out List<string> validationErrors);

    bool Validate(T? item);

    IEnumerable<T> FilterValid(IEnumerable<T>? items);
}
