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
public class UsersPerPlatformChart
{
    public (List<double> Counts, List<string> Names) Run_Platform_Bar_Chart()
    {
        DataLoader<GlobalMusicStreamingModel> musicLoader = new DataLoader<GlobalMusicStreamingModel>();
        musicLoader.LoadData("CSV-Files/globalMusicStreaming.csv");

        var musicData = musicLoader.data;

        var platform_counts = musicData.GroupBy(p => p.StreamingPlatform)
                                                .Select(p => new
                                                {
                                                    Platform = p.Key,
                                                    Count = p.Count()
                                                })
                                                .OrderByDescending(p => p.Count);

        List<double> counts = platform_counts.Select(p => (double)p.Count).ToList();
        List<string> names = platform_counts.Select(p => p.Platform).ToList()!;
        return (counts, names);
    }

}