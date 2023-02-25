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
    public class StatisticsTab4ViewModel : ViewModelBase
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
                SliderLabel = $"{MonthsScope + 1} Month(s)";
                InitData();
            }
        }

        private string _sliderLabel;
        public string SliderLabel
        {
            get => _sliderLabel;
            set
            {
                _sliderLabel = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _seriesCollection4;
        public SeriesCollection SeriesCollection4
        {
            get => _seriesCollection4;
            set
            {
                _seriesCollection4 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _axesYCollection4;
        public AxesCollection AxesYCollection4
        {
            get => _axesYCollection4;
            set
            {
                _axesYCollection4 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _labels4;
        public AxesCollection Labels4
        {
            get => _labels4;
            set
            {
                _labels4 = value;
                OnPropertyChanged();
            }
        }

        private string _labelText4;
        public string LabelText4
        {
            get => _labelText4;
            set
            {
                _labelText4 = value;
                OnPropertyChanged();
            }
        }

        public StatisticsTab4ViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
            InitData();
        }

        private void Initialize()
        {
            AxesYCollection4 = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "Lost Profit", Foreground = Brushes.Black, LabelFormatter = e => e.ToString("0.##") },
            };

            Labels4 = new AxesCollection
            {
                new Axis
                {
                    Labels = new List<string>(),
                    LabelsRotation = 45,
                    Foreground = Brushes.Black,
                    Separator = new LiveCharts.Wpf.Separator { Step = 1 },
                }
            };

            SeriesCollection4 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Lost Profit",
                    Fill = Brushes.Black,
                    Values = new ChartValues<decimal>(),
                    DataLabels = true,
                },
            };
        }

        private async Task InitData()
        {
            await _sync.WaitAsync();

            ((List<string>)Labels4?[0].Labels)?.Clear();
            SeriesCollection4?[0].Values?.Clear();

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
            ((List<string>)Labels4[0].Labels).AddRange(groupedPackages.Select(e => e.Key));
            SeriesCollection4[0].Values.AddRange(groupedPackages.Select(e =>
            {
                var lossOfProfit = 0m;
                foreach (var package in e)
                {
                    var scope = package.MeetingsUnheld
                        .Where(e => e > DateTime.Now - TimeSpan.FromDays(30) * (MonthsScope + 1))
                        .ToList();
                    lossOfProfit += scope.Count * package.MeetingProfit;
                }
                return (object)lossOfProfit;
            }));
            LabelText4 = $"Loss of Profit: {((ChartValues<decimal>)SeriesCollection4[0].Values).Sum()}";

            _sync.Release();
        }
    }
}
