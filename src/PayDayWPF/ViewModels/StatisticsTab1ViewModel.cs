using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using PayDayWPF.Persistence;

namespace PayDayWPF.ViewModels
{
    public class StatisticsTab1ViewModel : ViewModelBase
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

        private string _labelText1;
        public string LabelText1
        {
            get => _labelText1;
            set
            {
                _labelText1 = value;
                OnPropertyChanged();
            }
        }

        private string _labelText2;
        public string LabelText2
        {
            get => _labelText2;
            set
            {
                _labelText2 = value;
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

        private string _years;
        public string Years
        {
            get => _years;
            set
            {
                _years = value;
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

        public int Offset { get; set; }

        public ICommand PreviousCommand => new RelayCommand(_ =>
        {
            Offset = Offset - 1;
            DataInit();
        });

        public ICommand NextCommand => new RelayCommand(_ =>
        {
            Offset = Offset + 1;
            DataInit();
        });

        public StatisticsTab1ViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
            DataInit();
        }

        private void Initialize()
        {
            AxesYCollection = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "PayDay", Foreground = Brushes.Green },
                new Axis { MinValue = 0, Title = "Time", Foreground = Brushes.DarkRed },
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
                    Separator = new Separator { Step = 1 },
                }
            };

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Money",
                    Fill = Brushes.Green,
                    Values = new ChartValues<decimal>(),
                    ScalesYAt = 0,
                },
                new ColumnSeries
                {
                    Title= "Time",
                    Fill = Brushes.DarkRed,
                    Values = new ChartValues<decimal>(),
                    ScalesYAt = 1
                }
            };
        }

        private async Task DataInit()
        {
            MonthlyIncome = new decimal[12];
            MonthlyHours = new decimal[12];

            var now = DateTime.Now;
            var lastSeptember = new DateTime(now.Year, 9, 1);
            if (lastSeptember > now)
            {
                lastSeptember = lastSeptember.AddYears(-1);
            }
            lastSeptember = lastSeptember.AddYears(Offset);
            Years = $"{lastSeptember.Year}/{lastSeptember.Year+1}";
            var packages = await _repository.Load();
            var filteredPackages = packages
                .Where(e => e.MeetingsHeld.Any(f => f >= lastSeptember))
                .ToList();
            foreach (var package in filteredPackages)
            {
                var filteredMeetingsHeld = package.MeetingsHeld
                    .Where(e => e >= lastSeptember && e < lastSeptember.AddMonths(12));
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

            if (MonthlyIncome.Count(e => e != 0) == 0)
            {
                LabelText1 = "No Data";
            }
            else
            {
                LabelText1 = $"PayDay: {MonthlyIncome.Sum()} ({(MonthlyIncome.Sum() / MonthlyIncome.Count(e => e != 0)).ToString("n2")})   " +
                    $"Time: {MonthlyHours.Sum()} ({(MonthlyHours.Sum() / MonthlyHours.Count(e => e != 0)).ToString("n2")})";
            }
            
            var activePackages = packages
                .Where(e => e.MeetingsHeld.Count != e.MeetingCount)
                .DistinctBy(e => e.Name)
                .ToList();
            var hours = 0.0;
            var activePackagesIncome = 0.0;
            foreach (var package in activePackages)
            {
                hours += package.Duration * package.MeetingsPerWeek / 60;
                activePackagesIncome += (double)package.MeetingProfit * package.MeetingsPerWeek;
            }

            var monthlyIncomeFull = new decimal[12];
            var monthlyHoursFull = new decimal[12];

            Array.Copy(MonthlyIncome, monthlyIncomeFull, MonthlyIncome.Length);
            for (var i = 11; i >= 0; i--)
            {
                if (monthlyIncomeFull[i] > 0)
                {
                    monthlyIncomeFull[i] = 0;
                    break;
                }
            }

            Array.Copy(MonthlyHours, monthlyHoursFull, MonthlyHours.Length);
            for (var i = 11; i >= 0; i--)
            {
                if (monthlyHoursFull[i] > 0)
                {
                    monthlyHoursFull[i] = 0;
                    break;
                }
            }

            if (monthlyIncomeFull.Count(e => e != 0) == 0)
            {
                LabelText2 = "No Data";
            }
            else
            {
                LabelText2 = $"PayDay: {monthlyIncomeFull.Sum()} ({(monthlyIncomeFull.Sum() / monthlyIncomeFull.Count(e => e != 0)).ToString("n2")})   " +
                    $"Time - 1: {monthlyHoursFull.Sum()} ({(monthlyHoursFull.Sum() / monthlyHoursFull.Count(e => e != 0)).ToString("n2")})";
            }

            var groupedDuration = activePackages
                .GroupBy(e => (e.Duration, e.MeetingProfit))
                .OrderBy(e => e.Key.Duration)
                .ToList();

            var meetingsByDurationCount = new List<string>();
            foreach (var group in groupedDuration)
            {
                meetingsByDurationCount.Add($"{group.Key.Duration}({group.Key.MeetingProfit}): {group.Select(e => e.MeetingsPerWeek).Sum()}");
            }
            var separator = string.Join(" / ", meetingsByDurationCount);
            if (monthlyIncomeFull.Count(e => e != 0) == 0)
            {
                LabelText3 = "No Data";
            }
            else
            {
                LabelText3 = $"Hours: {hours}h   Iph: {(activePackagesIncome / hours):n2}   {separator}";
            }
        }
    }
}
