using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace AOP_3.ViewModels.Charts;
public class UsersByCountryChart
{
    //TODO: implement title in UI
    public LabelVisual Title { get; set; } =
    new()
    {
        Text = "Users By Country",
        TextSize = 25,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint(SKColors.WhiteSmoke)
    };

    public HeatLandSeries[] Run_UsersByCountry_Chart()
    {
        DataLoader<GlobalMusicStreamingModel> musicLoader = new DataLoader<GlobalMusicStreamingModel>();
        musicLoader.LoadData("CSV-Files/globalMusicStreaming.csv");

        var musicData = musicLoader.data;

        // Load the country shorthand mapping from the JSON file
        var countryCodeMapping = LoadCountryCodeMapping("Shorthand/word-map-index.json");

        var country_counts = musicData.GroupBy(p => p.Country)
                                      .Select(p => new
                                      {
                                          Country = p.Key,
                                          Count = p.Count()
                                      });

        // Map country_counts to HeatLand objects using the shorthand codes
        var lands = country_counts.Select(c => new HeatLand
        {
            Name = countryCodeMapping.ContainsKey(c.Country!) ? countryCodeMapping[c.Country!] : c.Country!.ToLower(),
            Value = c.Count
        }).ToList();

        // Return a HeatLandSeries with the mapped lands
        return new HeatLandSeries[]
        {
            new HeatLandSeries
            {
                Lands = lands
            }
        };
    }

    private Dictionary<string, string> LoadCountryCodeMapping(string filePath)
    {
        var json = System.IO.File.ReadAllText(filePath);

        // Deserialize the JSON into a dynamic object
        var jsonObject = JsonSerializer.Deserialize<JsonDocument>(json);

        // Extract the "lands" array and map "name" to "shortName"
        var mapping = new Dictionary<string, string>();

        if (jsonObject?.RootElement.TryGetProperty("lands", out var lands) == true)
        {
            foreach (var land in lands.EnumerateArray())
            {
                var name = land.GetProperty("name").GetString();
                var shortName = land.GetProperty("shortName").GetString();

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(shortName))
                {
                    mapping[name] = shortName;
                }
            }
        }

        return mapping;
    }
}
