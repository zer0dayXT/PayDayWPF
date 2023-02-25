using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using PayDayWPF.Persistence;

namespace PayDayWPF.ViewModels
{
    public class StatisticsTab2ViewModel : ViewModelBase
    {
        private readonly IRepository _repository;

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

        public StatisticsTab2ViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
            InitData();
        }

        private void Initialize()
        {
            AxesYCollection = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "Income", Foreground = Brushes.Green },
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
                new StackedColumnSeries
                {
                    Title = "Income",
                    Fill = Brushes.Green,
                    Values = new ChartValues<decimal>(),
                    DataLabels = true,
                },
                new StackedColumnSeries
                {
                    Title = "Potential Income",
                    Fill = Brushes.DarkRed,
                    Values = new ChartValues<decimal>(),
                    DataLabels = true,
                },
            };
        }

        private async Task InitData()
        {
            var packages = await _repository.Load();
            packages = packages
                .Where(e => e.MeetingsHeld.Count != e.MeetingCount)
                .ToList();
            var groupedPackages = packages
                .GroupBy(e => e.Name)
                .ToList();
            ((List<string>)Labels[0].Labels).AddRange(groupedPackages.Select(e => e.Key));
            SeriesCollection[0].Values.AddRange(groupedPackages.Select(e =>
            {
                var activePackage = e.Where(f => f.MeetingsHeld.Count > 0).FirstOrDefault();
                if (activePackage == null)
                {
                    activePackage = e.FirstOrDefault();
                }
                var activePackageIncome = activePackage.MeetingsHeld.Count * activePackage.MeetingProfit;
                return (object)activePackageIncome;
            }));
            SeriesCollection[1].Values.AddRange(groupedPackages.Select(e =>
            {
                var totalIncome = 0m;
                foreach (var package in e)
                {
                    var packageIncome = (package.MeetingCount - package.MeetingsHeld.Count) * package.MeetingProfit;
                    totalIncome = totalIncome + packageIncome;
                }
                return (object)totalIncome;
            }));

            LabelText = $"Income: {((ChartValues<decimal>)SeriesCollection[0].Values).Sum()}  " +
                $"Potential Income: {((ChartValues<decimal>)SeriesCollection[1].Values).Sum()}";
        }
    }
}
