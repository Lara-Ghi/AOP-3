using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace AOP_3.ViewModels.Charts;
public class UsersByGenreChart
{
    //TODO: implement title in UI
    // Not implemented in the UI, since it was replaced by the pie chart.
    public LabelVisual Title { get; set; } =
    new()
    {
        Text = "Users By Genre",
        TextSize = 25,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint(SKColors.WhiteSmoke)
    };

    public (List<double> GenreCounts, List<string> GenreNames) Run_Genre_Bar_Chart()
    {
        DataLoader<GlobalMusicStreamingModel> musicLoader = new DataLoader<GlobalMusicStreamingModel>();
        musicLoader.LoadData("CSV-Files/GlobalMusicStreaming.csv");

        var musicData = musicLoader.data;

        var genre_counts = musicData.GroupBy(p => p.TopGenre)
                                                .Select(p => new
                                                {
                                                    Genre = p.Key,
                                                    Count = p.Count()
                                                })
                                                .OrderByDescending(p => p.Count).ToList();

        List<double> genrecounts = genre_counts.Select(p => (double)p.Count).ToList();
        List<string> genrenames = genre_counts.Select(p => p.Genre).ToList()!;
        return (genrecounts, genrenames);
    }
}