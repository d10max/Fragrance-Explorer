namespace FragranceExplorer.BLL.DataSetParser.Common;

public class ParserOptions
{

    public string PathToDataset { get; set; } = "perfumes_actual.jsonl";
   
    public int? MaxPerfumesToParse {get; set;}

    public int SkipRecords {get; set;}
}

