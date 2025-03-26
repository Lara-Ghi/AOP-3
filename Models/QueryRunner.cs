using System;
using System.Collections.Generic;
using System.Linq;

public class QueryRunner 
{
    public (List<double> Counts, List<string> Names)   Run_Generic_Bar_Chart() 
    {
        DataLoader<GlobalMusicStreamingModel> musicLoader = new DataLoader<GlobalMusicStreamingModel>();
        musicLoader.LoadData("CSV-Files/global_music_streaming.csv");

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