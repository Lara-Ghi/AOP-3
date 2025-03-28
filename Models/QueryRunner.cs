using System;
using System.Collections.Generic;
using System.Linq;

public class QueryRunner 
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
                                                .OrderByDescending(p => p.Count);
        
        List<double> genrecounts = genre_counts.Select(p => (double)p.Count).ToList();
        List<string> genrenames = genre_counts.Select(p => p.Genre).ToList()!;
        return (genrecounts, genrenames);
    }
}