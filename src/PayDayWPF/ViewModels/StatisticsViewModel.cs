using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using PayDayWPF.Persistence;

namespace PayDayWPF.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
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

        private decimal[] _monthlyIncome;
        public decimal[] MonthlyIncome
        {
            get => _monthlyIncome;
            set
            {
                _monthlyIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal[] _monthlyHours;
        public decimal[] MonthlyHours
        {
            get => _monthlyHours;
            set
            {
                _monthlyHours = value;
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

        public StatisticsViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
            OverallStatistics();
        }

        private void Initialize()
        {
            AxesYCollection = new AxesCollection
            {
                new Axis { Title = "PayDay", Foreground = Brushes.Green },
                new Axis { Title = "Time", Foreground = Brushes.DarkRed },
            };

            Labels = new AxesCollection
            {
                new Axis
                {
                    Labels = new string[]
                    {
                        "September", "October", "November", "December",
                        "January", "February", "March", "April", "May", "June", "July", "August"
                    },
                    Foreground = Brushes.Black,
                    Separator = new LiveCharts.Wpf.Separator { Step = 1 },
                }
            };

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Money",
                    Fill = Brushes.Green,
                    Values = new ChartValues<decimal> { 5, 6, 2, 7 },
                    ScalesYAt = 0
                },
                new ColumnSeries
                {
                    Title= "Time",
                    Fill = Brushes.DarkRed,
                    Values = new ChartValues<decimal> { 3000, 2000, 1000, 9000 },
                    ScalesYAt = 1
                }
            };
        }

        private async Task OverallStatistics()
        {
            MonthlyIncome = new decimal[12];
            MonthlyHours = new decimal[12];

            var now = DateTime.Now;
            var lastSeptember = new DateTime(now.Year, 9, 1);
            if (lastSeptember > now)
            {
                lastSeptember = lastSeptember.AddYears(-1);
            }
            var packages = await _repository.Load();
            var filteredPackages = packages
                .Where(e => e.MeetingsHeld.Any(f => f >= lastSeptember))
                .ToList();
            foreach (var package in filteredPackages)
            {
                var filteredMeetingsHeld = package.MeetingsHeld
                    .Where(e => e >= lastSeptember);
                foreach (var meetingsHeld in filteredMeetingsHeld)
                {
                    MonthlyIncome[(meetingsHeld.Month + 3) % 12] += package.MeetingProfit;
                    MonthlyHours[(meetingsHeld.Month + 3) % 12] += ((decimal)package.Duration) / 60;
                }
            }
            SeriesCollection[0].Values.Clear();
            SeriesCollection[0].Values.AddRange(MonthlyIncome.Select(e => (object)e));
            SeriesCollection[1].Values.Clear();
            SeriesCollection[1].Values.AddRange(MonthlyHours.Select(e => (object)e));
            LabelText = "fdasfsadas";
        }
    }
}
