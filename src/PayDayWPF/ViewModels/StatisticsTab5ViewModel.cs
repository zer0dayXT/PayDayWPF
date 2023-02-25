using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using PayDayWPF.Persistence;

namespace PayDayWPF.ViewModels
{
    public class StatisticsTab5ViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        private SemaphoreSlim _sync = new SemaphoreSlim(1, 1);

        private int _monthsScope;
        public int MonthsScope
        {
            get => _monthsScope;
            set
            {
                _monthsScope = value;
                //SliderLabel = $"{MonthsScope + 1} Month(s)";
                InitData();
            }
        }

        private SeriesCollection _seriesCollection5;
        public SeriesCollection SeriesCollection5
        {
            get => _seriesCollection5;
            set
            {
                _seriesCollection5 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _axesYCollection5;
        public AxesCollection AxesYCollection5
        {
            get => _axesYCollection5;
            set
            {
                _axesYCollection5 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _labels5;
        public AxesCollection Labels5
        {
            get => _labels5;
            set
            {
                _labels5 = value;
                OnPropertyChanged();
            }
        }

        private string _labelText5;
        public string LabelText5
        {
            get => _labelText5;
            set
            {
                _labelText5 = value;
                OnPropertyChanged();
            }
        }

        public StatisticsTab5ViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
            InitData();
        }

        private void Initialize()
        {
            AxesYCollection5 = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "Lost Profit (Time)", Foreground = Brushes.Black, LabelFormatter = e => e.ToString("0.##") },
            };

            Labels5 = new AxesCollection
            {
                new Axis
                {
                    Labels = new List<string>(),
                    LabelsRotation = 45,
                    Foreground = Brushes.Black,
                    Separator = new LiveCharts.Wpf.Separator { Step = 1 },
                }
            };

            SeriesCollection5 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Lost Profit (Time)",
                    Fill = Brushes.Black,
                    Values = new ChartValues<decimal>(),
                    DataLabels = true,
                },
            };
        }

        private async Task InitData()
        {
            await _sync.WaitAsync();

            ((List<string>)Labels5?[0].Labels)?.Clear();
            SeriesCollection5?[0].Values?.Clear();

            var packages = await _repository.Load();
            var activeUserNames = packages
                .Where(e => e.MeetingsHeld.Count != e.MeetingCount)
                .Select(e => e.Name)
                .Distinct()
                .ToList();
            var activeUserPackages = packages
                .Where(e => activeUserNames.Contains(e.Name))
                .ToList();
            var groupedPackages = activeUserPackages
                .GroupBy(e => e.Name)
                .ToList();
            ((List<string>)Labels5[0].Labels).AddRange(groupedPackages.Select(e => e.Key));
            SeriesCollection5[0].Values.AddRange(groupedPackages.Select(e =>
            {
                var lossOfProfitTime = 0m;
                foreach (var package in e)
                {
                    var scope = package.MeetingsUnheld
                        .Where(e => e > DateTime.Now - TimeSpan.FromDays(30) * (MonthsScope + 1))
                        .ToList();
                    lossOfProfitTime += (decimal)scope.Count * package.Duration / 60;
                }
                return (object)lossOfProfitTime;
            }));
            LabelText5 = $"Loss of Profit (Time): {((ChartValues<decimal>)SeriesCollection5[0].Values).Sum()}";

            _sync.Release();
        }
    }
}
