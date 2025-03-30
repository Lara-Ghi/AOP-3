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

namespace AOP_3.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string output = String.Empty;
    public ISeries[] PlatformSeries { get; set; } = [];
    public ISeries[] GenreSeries { get; set; } = [];
    public HeatLandSeries[] CountrySeries { get; set; } = [];
    public Axis[] PlatformYAxes { get; set; } = [];
    public Axis[] GenreYAxes { get; set; } = [];
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

        var genreChart = new UsersByGenreChart();
        (List<double> genrecounts, List<string> genrenames) = genreChart.Run_Genre_Bar_Chart();
        GenreSeries = [
            new RowSeries<double>
            {
                Values = genrecounts,
            }
        ];

        GenreYAxes = [
            new Axis
            {
                Labels = genrenames,
            }
        ];

        var countryChart = new UsersByCountryChart();
        HeatLandSeries[] countrycounts = countryChart.Run_UsersByCountry_Chart();
        CountrySeries = countrycounts;
    }
}
