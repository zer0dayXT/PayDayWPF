using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using PayDayWPF.Infrastructure;
using PayDayWPF.Persistence;

namespace PayDayWPF.ViewModels
{
    public class StatisticsTab3ViewModel : ViewModelBase
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

        public StatisticsTab3ViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
            InitData();
        }

        private void Initialize()
        {
            AxesYCollection = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "Count", Foreground = Brushes.Green },
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
                    Title = "Held",
                    Fill = Brushes.Green,
                    Values = new ChartValues<int>(),
                    DataLabels = true,
                },
                new StackedColumnSeries
                {
                    Title = "Remaining",
                    Fill = Brushes.DarkRed,
                    Values = new ChartValues<int>(),
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
            var filteredPackages = MergePackages(packages);
            filteredPackages = filteredPackages
                .Where(e => e.MeetingsHeld.Count != e.MeetingCount)
                .ToList();
            ((List<string>)Labels[0].Labels).AddRange(filteredPackages.Select(e => e.Name));
            SeriesCollection[1].Values.AddRange(filteredPackages.Select(e => (object)(e.MeetingCount - e.MeetingsHeld.Count)));
            SeriesCollection[0].Values.AddRange(filteredPackages.Select(e => (object)e.MeetingsHeld.Count));

            LabelText = $"Held {((ChartValues<int>)SeriesCollection[0].Values).Sum()}  " +
                $"Remaining: {((ChartValues<int>)SeriesCollection[1].Values).Sum()}";
        }

        private List<Package> MergePackages(List<Package> packages)
        {
            var filteredPackages = packages
                .GroupBy(e => e.Name)
                .Select(e => new Package
                {
                    Name = e.Key,
                    MeetingCount = e.Sum(f => f.MeetingCount),
                    MeetingsHeld = e.SelectMany(f => f.MeetingsHeld).ToList(),
                }).ToList();
            return filteredPackages;
        }
    }
}
