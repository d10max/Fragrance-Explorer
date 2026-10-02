namespace FragranceExplorer.BLL.DataSetParser.Interfaces;

public interface IDataSetReader<T> where T : class
{
    IAsyncEnumerable<T> ReadAsync(string filePath, CancellationToken cancellationToken = default);
    
    double ProgressPercentage { get; }
    
    long TotalBytesRead { get; }
}
