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

namespace AOP_3.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string output = String.Empty;
    public ISeries[] Series { get; set; } = [];
    public ISeries[] GenreSeries { get; set; } = [];
    public Axis[] YAxes { get; set; } = [];
    public Axis[] GenreYAxes { get; set; } = [];
    public MainWindowViewModel()
    {
        QueryRunner queryRunner = new QueryRunner();
        (List<double> counts, List<string> names) = queryRunner.Run_Platform_Bar_Chart();
        Series = [
            new RowSeries<double>
            {
                Values = counts,
            }
        ];

        YAxes = [
            new Axis
            {
                Labels = names,
            }
        ];

        (List<double> genrecounts, List<string> genrenames) = queryRunner.Run_Genre_Bar_Chart();
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
    }
}
