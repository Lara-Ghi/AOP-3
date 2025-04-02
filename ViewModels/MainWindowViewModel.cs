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
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Metadata;
using Avalonia.Controls.Documents;

namespace AOP_3.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string output = String.Empty;
    public ISeries[] PlatformSeries { get; set; } = [];
    public Axis[] PlatformYAxes { get; set; } = [];
    public ObservableCollection<ISeries> GenreSeries { get; } = [];
    public HeatLandSeries[] CountrySeries { get; set; } = [];
    public ObservableCollection<ISeries> SubscriptionSeries { get; } = [];
    public ISeries[] ListeningTimeSeries { get; set; } = [];
    public Axis[] ListeningTimeXAxes { get; set; } = [];
    public Axis[] ListeningTimeYAxes { get; set; } = [];

    public ObservableCollection<ISeries> PlatformSubscriptionSeries { get; } = [];

    [ObservableProperty]
    public bool isListeningTimeChartVisible = false;

    [ObservableProperty]
    public bool isPlatformSubscriptionChartVisible = false;

    [ObservableProperty]
    public bool isPlatformChartVisible = false;

    [ObservableProperty]
    public bool isGenreChartVisible = false;

    [ObservableProperty]
    public bool isCountryChartVisible = false;

    [ObservableProperty]
    public bool isSubscriptionChartVisible = false;

    public MainWindowViewModel()
    {
        ListeningTimeChart();
        PlatformSubscriptionChart();
        TopPlatformChart();
        UsersByGenreChart();
        UsersByCountryChart();
        SubscriptionChart();
    }

    private void ListeningTimeChart()
    {
        var musicLoader = new DataLoader<GlobalMusicStreamingModel>();
        musicLoader.LoadData("CSV-Files/GlobalMusicStreaming.csv");

        var musicData = musicLoader.data;

        var morningData = musicData.Where(m => m.ListeningTime == "Morning").ToList();
        var afternoonData = musicData.Where(m => m.ListeningTime == "Afternoon").ToList();
        var nightData = musicData.Where(m => m.ListeningTime == "Night").ToList();

        var morningValues = morningData.Select(m => (double)m.MinutesStreamedPerDay).ToList();
        var afternoonValues = afternoonData.Select(m => (double)m.MinutesStreamedPerDay).ToList();
        var nightValues = nightData.Select(m => (double)m.MinutesStreamedPerDay).ToList();

        var weeklyMorningValues = morningValues
            .Select((value, index) => new { value, index })
            .GroupBy(x => x.index / 7)
            .Select(g => g.Sum(x => x.value))
            .ToList();

        var weeklyAfternoonValues = afternoonValues
            .Select((value, index) => new { value, index })
            .GroupBy(x => x.index / 7)
            .Select(g => g.Sum(x => x.value))
            .ToList();

        var weeklyNightValues = nightValues
            .Select((value, index) => new { value, index })
            .GroupBy(x => x.index / 7)
            .Select(g => g.Sum(x => x.value))
            .ToList();

        ListeningTimeSeries = new ISeries[]
        {
            new StackedAreaSeries<double>
            {
                Values = weeklyMorningValues,
                Name = "Morning",
                Fill = new SolidColorPaint(SKColors.LightBlue)
            },
            new StackedAreaSeries<double>
            {
                Values = weeklyAfternoonValues,
                Name = "Afternoon",
                Fill = new SolidColorPaint(SKColors.LightGreen)
            },
            new StackedAreaSeries<double>
            {
                Values = weeklyNightValues,
                Name = "Night",
                Fill = new SolidColorPaint(SKColors.LightCoral)
            }
        };

        ListeningTimeXAxes = new Axis[]
        {
            new Axis
            {
                // Grouped into weeks
                Labels = Enumerable.Range(1, weeklyMorningValues.Count).Select(i => $"Week {i}").ToList()
            }
        };

        ListeningTimeYAxes = new Axis[]
        {
            new Axis
            {
                Name = "Minutes Streamed Per Week"
            }
        };
    }

    private void PlatformSubscriptionChart()
    {
        var musicLoader = new DataLoader<GlobalMusicStreamingModel>();
        musicLoader.LoadData("CSV-Files/GlobalMusicStreaming.csv");

        var musicData = musicLoader.data;

        var groupedData = musicData
            .GroupBy(m => new { m.StreamingPlatform, m.SubscriptionType })
            .Select(g => new
            {
                Platform = g.Key.StreamingPlatform,
                Subscription = g.Key.SubscriptionType,
                Count = g.Count()
            })
            .ToList();

        PlatformSubscriptionSeries.Clear();
        foreach (var group in groupedData)
        {
            PlatformSubscriptionSeries.Add(new RowSeries<double>
            {
                Values = new List<double> { group.Count },
                Name = $"{group.Platform} ({group.Subscription})",
                Fill = new SolidColorPaint(new RandomColour().GetRandomColour())
            });
        }
    }

    private void TopPlatformChart()
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
    }

    public LabelVisual ListeningTimeTitle { get; set; } = new LabelVisual
    {
        Text = "Listening Time (Morning, Afternoon, Night)",
        TextSize = 20,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint(SKColors.RoyalBlue)
    };

    public LabelVisual PlatformSubscriptionTitle { get; set; } = new LabelVisual
    {
        Text = "Platform Subscription Types",
        TextSize = 20,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint(SKColors.RoyalBlue)
    };

    public LabelVisual PlatformTitle { get; set; } = new LabelVisual
    {
        Text = "Top Streaming Platforms",
        TextSize = 20,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint(SKColors.RoyalBlue),
    };

    private void UsersByCountryChart()
    {
        var countryChart = new UsersByCountryChart();
        HeatLandSeries[] countrycounts = countryChart.Run_UsersByCountry_Chart();
        CountrySeries = countrycounts;
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

    public LabelVisual TopGenreTitle { get; set; } = new LabelVisual
    {
        Text = "Top Genre Streamed",
        TextSize = 20,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint(SKColors.RoyalBlue),
    };

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

    public LabelVisual SubscriptionTitle { get; set; } = new LabelVisual
    {
        Text = "Subscription Type By Users",
        TextSize = 20,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint(SKColors.RoyalBlue),
    };

    //TODO: implement the delete funtion for all of the charts

    [RelayCommand]
    private void DeletePlatformChart()
    {

    }
}
