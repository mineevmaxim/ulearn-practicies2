using System.Text;

namespace Delegates.Reports;

public static class ReportMaker
{
    public static string MakeReport(
        string caption,
        Func<IEnumerable<double>, object> makeStatistics,
        Func<string, string?, string?, string> makeMarkup,
        IEnumerable<Measurement> measurements
    )
    {
        var data = measurements.ToList();
        var temperature = makeStatistics(data.Select(m => m.Temperature)).ToString();
        var humidity = makeStatistics(data.Select(m => m.Humidity)).ToString();
        return makeMarkup(caption, temperature, humidity);
    }
}

public static class ReportMakerHelper
{
    public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
        => ReportMaker.MakeReport("Mean and Std",
            MakeMeanAndStdStatistics,
            MakeHtmlMarkup,
            data);

    public static string MedianMarkdownReport(IEnumerable<Measurement> data)
        => ReportMaker.MakeReport("Median",
            MakeMedianStatistics,
            MakeMarkdownMarkup,
            data);

    public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
        => ReportMaker.MakeReport("Mean and Std",
            MakeMeanAndStdStatistics,
            MakeMarkdownMarkup,
            measurements);

    public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
        => ReportMaker.MakeReport("Median",
            MakeMedianStatistics,
            MakeHtmlMarkup,
            measurements);
    
    private static string MakeHtmlMarkup(string caption, string? temperature, string? humidity)
    {
        var markup = new StringBuilder();
        markup.Append($"<h1>{caption}</h1>");
        markup.Append("<ul>");
        markup.Append($"<li><b>Temperature</b>: {temperature}");
        markup.Append($"<li><b>Humidity</b>: {humidity}");
        markup.Append("</ul>");
        return markup.ToString();
    }
    
    private static string MakeMarkdownMarkup(string caption, string? temperature, string? humidity)
    {
        var markup = new StringBuilder();
        markup.Append($"## {caption}\n\n");
        markup.Append($" * **Temperature**: {temperature}\n\n");
        markup.Append($" * **Humidity**: {humidity}\n\n");
        return markup.ToString();
    }

    private static object MakeMedianStatistics(IEnumerable<double> doubles)
    {
        var list = doubles.OrderBy(z => z).ToList();
        if (list.Count % 2 == 0)
            return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

        return list[list.Count / 2];
    }
    
    private static object MakeMeanAndStdStatistics(IEnumerable<double> doubles)
    {
        var doublesList = doubles.ToList();
        var mean = doublesList.Average();
        var std = Math.Sqrt(doublesList.Select(z => Math.Pow(z - mean, 2)).Sum() / (doublesList.Count - 1));

        return new MeanAndStd { Mean = mean, Std = std };
    }
}