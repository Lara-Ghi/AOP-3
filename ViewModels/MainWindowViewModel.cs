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

namespace AOP_3.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string output = String.Empty;
    public ISeries[] Series { get; set; } = [];
    public Axis[] YAxes { get; set; } = [];
    public MainWindowViewModel()
    {
        QueryRunner queryRunner = new QueryRunner();
        (List<double> counts, List<string> names) = queryRunner.Run_Generic_Bar_Chart();
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

    }
}
