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

        private SeriesCollection _seriesCollection3;
        public SeriesCollection SeriesCollection3
        {
            get => _seriesCollection3;
            set
            {
                _seriesCollection3 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _axesYCollection3;
        public AxesCollection AxesYCollection3
        {
            get => _axesYCollection3;
            set
            {
                _axesYCollection3 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _labels3;
        public AxesCollection Labels3
        {
            get => _labels3;
            set
            {
                _labels3 = value;
                OnPropertyChanged();
            }
        }

        private string _labelText3;
        public string LabelText3
        {
            get => _labelText3;
            set
            {
                _labelText3 = value;
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
            AxesYCollection3 = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "Count", Foreground = Brushes.Green },
            };

            Labels3 = new AxesCollection
            {
                new Axis
                {
                    Labels = new List<string>(),
                    LabelsRotation = 45,
                    Foreground = Brushes.Black,
                    Separator = new LiveCharts.Wpf.Separator { Step = 1 },
                }
            };

            SeriesCollection3 = new SeriesCollection
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
            ((List<string>)Labels3[0].Labels).AddRange(filteredPackages.Select(e => e.Name));
            SeriesCollection3[1].Values.AddRange(filteredPackages.Select(e => (object)(e.MeetingCount - e.MeetingsHeld.Count)));
            SeriesCollection3[0].Values.AddRange(filteredPackages.Select(e => (object)e.MeetingsHeld.Count));

            LabelText3 = $"Held {((ChartValues<int>)SeriesCollection3[0].Values).Sum()}  " +
                $"Remaining: {((ChartValues<int>)SeriesCollection3[1].Values).Sum()}";
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
