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

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _axesYCollection;
        public AxesCollection AxesYCollection
        {
            get => _axesYCollection;
            set
            {
                _axesYCollection = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _labels;
        public AxesCollection Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged();
            }
        }

        private string _labelText;
        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
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
            AxesYCollection = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "Lost Profit (Time)", Foreground = Brushes.Black, LabelFormatter = e => e.ToString("0.##") },
            };

            Labels = new AxesCollection
            {
                new Axis
                {
                    Labels = new List<string>(),
                    LabelsRotation = 45,
                    Foreground = Brushes.Black,
                    Separator = new Separator { Step = 1 },
                }
            };

            SeriesCollection = new SeriesCollection
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

            ((List<string>)Labels?[0].Labels)?.Clear();
            SeriesCollection?[0].Values?.Clear();

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
            ((List<string>)Labels[0].Labels).AddRange(groupedPackages.Select(e => e.Key));
            SeriesCollection[0].Values.AddRange(groupedPackages.Select(e =>
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
            LabelText = $"Loss of Profit (Time): {((ChartValues<decimal>)SeriesCollection[0].Values).Sum()}";

            _sync.Release();
        }
    }
}
