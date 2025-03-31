using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Themes;
using System;
using SkiaSharp;
using AOP_3.ViewModels.Charts;
using System.Collections.ObjectModel;
using System.Linq;
using AOP_3.Models;

namespace AOP_3.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string output = String.Empty;
    public ISeries[] PlatformSeries { get; set; } = [];
    public Axis[] PlatformYAxes { get; set; } = [];
    public ObservableCollection<ISeries> GenreSeries { get; } = [];
    public HeatLandSeries[] CountrySeries { get; set; } = [];
    public ObservableCollection<ISeries> SubscriptionSeries { get;} = [];

    public MainWindowViewModel()
    {
        var platformChart = new UsersPerPlatformChart();
        (List<double> counts, List<string> names) = platformChart.Run_Platform_Bar_Chart();
        PlatformSeries = [
            new RowSeries<double>
            {
                Values = counts,
            }
        ];

        PlatformYAxes = [
                    new Axis
            {
                Labels = names,
            }
                ];

        UsersByGenreChart();

        var countryChart = new UsersByCountryChart();
        HeatLandSeries[] countrycounts = countryChart.Run_UsersByCountry_Chart();
        CountrySeries = countrycounts;

        SubscriptionChart();
    }

    private void UsersByGenreChart()
    {
        var musicLoader = new DataLoader<GlobalMusicStreamingModel>();
        musicLoader.LoadData("CSV-Files/GlobalMusicStreaming.csv");

        var musicData = musicLoader.data;

        var genre_counts = musicData
            .GroupBy(m => m.TopGenre)
            .Select(g => new
            {
                Genre = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count).ToList();

        GenreSeries.Clear();
        foreach (var genre in genre_counts)
        {
            GenreSeries.Add(new PieSeries<double>
            {
                Values = new List<double> { genre.Count },
                Name = genre.Genre,
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsSize = 15,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                Fill = new SolidColorPaint(new RandomColour().GetRandomColour()) // Assign random colors
            });
        }
    }

    private void SubscriptionChart()
    {
        var musicLoader = new DataLoader<GlobalMusicStreamingModel>();
        musicLoader.LoadData("CSV-Files/GlobalMusicStreaming.csv");

        var musicData = musicLoader.data;

        var subscription_counts = musicData
            .GroupBy(m => m.SubscriptionType)
            .Select(g => new
            {
                Subscription = g.Key,
                Count = g.Count(),
            })
            .OrderByDescending(g => g.Count).ToList();

        SubscriptionSeries.Clear();
        foreach (var subscription in subscription_counts)
        {
            SubscriptionSeries.Add(new PieSeries<double>
            {
                Values = new List<double> { subscription.Count },
                Name = subscription.Subscription,
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsSize = 15,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                Fill = new SolidColorPaint(new RandomColour().GetRandomColour()) // Assign random colors
            });
        }
    }
}
