namespace RecommendationAlgorithm;

public class CsvProvider
{
    private const char delimiter = ',';
    public CsvData data;

    public CsvProvider(FileInfo csvInfo)
    {
        FileStream stream = new(csvInfo.FullName, FileMode.Open);
        StreamReader reader = new(stream);

        string headerLine = reader.ReadLine() ?? throw new FormatException();
        string[] header = headerLine.Split(delimiter);

        List<string[]> rows = [];
        HashSet<string> classLabels = [];

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] splitLine = line.Split(delimiter);
            classLabels.Add(splitLine.Last());
            rows.Add(splitLine);
        }

        stream.Close();
        reader.Close();

        data = new CsvData(header, [.. rows], [.. classLabels]);
    }

    public record CsvData
    {
        public string[] header;
        public string[][] rows;
        public string[] classLabels;

        public CsvData(string[] header, string[][] rows, string[] classLabels)
        {
            this.header = header;
            this.rows = rows;
            this.classLabels = classLabels;
        }
    }
}
