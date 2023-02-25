using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using PayDayWPF.Infrastructure;
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
                SliderLabel = $"{MonthsScope + 1} Month(s)";
                LossOfProfit();
                LossOfProfitTime();
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

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection1
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _seriesCollection2;
        public SeriesCollection SeriesCollection2
        {
            get => _seriesCollection2;
            set
            {
                _seriesCollection2 = value;
                OnPropertyChanged();
            }
        }

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

        private AxesCollection _axesYCollection1;
        public AxesCollection AxesYCollection1
        {
            get => _axesYCollection1;
            set
            {
                _axesYCollection1 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _labels1;
        public AxesCollection Labels1
        {
            get => _labels1;
            set
            {
                _labels1 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _axesYCollection2;
        public AxesCollection AxesYCollection2
        {
            get => _axesYCollection2;
            set
            {
                _axesYCollection2 = value;
                OnPropertyChanged();
            }
        }

        private AxesCollection _labels2;
        public AxesCollection Labels2
        {
            get => _labels2;
            set
            {
                _labels2 = value;
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

        private string _labelText1b;
        public string LabelText1b
        {
            get => _labelText1b;
            set
            {
                _labelText1b = value;
                OnPropertyChanged();
            }
        }

        private string _labelText1c;
        public string LabelText1c
        {
            get => _labelText1c;
            set
            {
                _labelText1c = value;
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

        public int Offset { get; set; }

        public ICommand PreviousCommand => new RelayCommand(_ =>
        {
            Offset = Offset - 1;
            OverallStatistics();
        });

        public ICommand NextCommand => new RelayCommand(_ =>
        {
            Offset = Offset + 1;
            OverallStatistics();
        });

        public StatisticsTab5ViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
            OverallStatistics();
            PotentialIncome();
            MeetingsBalance();
            LossOfProfit();
            LossOfProfitTime();
        }

        private void Initialize()
        {
            MonthsScope = 0;

            AxesYCollection1 = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "PayDay", Foreground = Brushes.Green },
                new Axis { MinValue = 0, Title = "Time", Foreground = Brushes.DarkRed },
            };

            Labels1 = new AxesCollection
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

            SeriesCollection1 = new SeriesCollection
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

            AxesYCollection2 = new AxesCollection
            {
                new Axis { MinValue = 0, Title = "Income", Foreground = Brushes.Green },
            };

            Labels2 = new AxesCollection
            {
                new Axis
                {
                    Labels = new List<string>(),
                    LabelsRotation = 45,
                    Foreground = Brushes.Black,
                    Separator = new LiveCharts.Wpf.Separator { Step = 1 },
                }
            };

            SeriesCollection2 = new SeriesCollection
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
            SeriesCollection1[0].Values.Clear();
            SeriesCollection1[0].Values.AddRange(MonthlyIncome.Select(e => (object)e));
            SeriesCollection1[1].Values.Clear();
            SeriesCollection1[1].Values.AddRange(MonthlyHours.Select(e => (object)e));

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
                LabelText1b = "No Data";
            }
            else
            {
                LabelText1b = $"PayDay: {monthlyIncomeFull.Sum()} ({(monthlyIncomeFull.Sum() / monthlyIncomeFull.Count(e => e != 0)).ToString("n2")})   " +
                    $"Time - 1: {monthlyHoursFull.Sum()} ({(monthlyHoursFull.Sum() / monthlyHoursFull.Count(e => e != 0)).ToString("n2")})";
            }

            var groupedDuration = activePackages
                .GroupBy(e => (e.Duration, e.MeetingProfit))
                .OrderBy(e => e.Key.Duration)
                .ToList();

            //$"45 - {count}, 60 - {count} total - {duration / 60}"
            var meetingsByDurationCount = new List<string>();

            foreach (var group in groupedDuration)
            {
                meetingsByDurationCount.Add($"{group.Key.Duration}({group.Key.MeetingProfit}): {group.Select(e => e.MeetingsPerWeek).Sum()}");
            }
            var separator = string.Join(" / ", meetingsByDurationCount);
            if (monthlyIncomeFull.Count(e => e != 0) == 0)
            {
                LabelText1c = "No Data";
            }
            else
            {
                LabelText1c = $"Hours: {hours}h   Iph: {(activePackagesIncome / hours):n2}   {separator}";
            }
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

        private async Task PotentialIncome()
        {
            var packages = await _repository.Load();
            packages = packages
                .Where(e => e.MeetingsHeld.Count != e.MeetingCount)
                .ToList();
            var groupedPackages = packages
                .GroupBy(e => e.Name)
                .ToList();
            ((List<string>)Labels2[0].Labels).AddRange(groupedPackages.Select(e => e.Key));
            SeriesCollection2[0].Values.AddRange(groupedPackages.Select(e =>
            {
                var activePackage = e.Where(f => f.MeetingsHeld.Count > 0).FirstOrDefault();
                if (activePackage == null)
                {
                    activePackage = e.FirstOrDefault();
                }
                var activePackageIncome = activePackage.MeetingsHeld.Count * activePackage.MeetingProfit;
                return (object)activePackageIncome;
            }));
            SeriesCollection2[1].Values.AddRange(groupedPackages.Select(e =>
            {
                var totalIncome = 0m;
                foreach (var package in e)
                {
                    var packageIncome = (package.MeetingCount - package.MeetingsHeld.Count) * package.MeetingProfit;
                    totalIncome = totalIncome + packageIncome;
                }
                return (object)totalIncome;
            }));
            if (MonthlyIncome.Count(e => e != 0) == 0)
            {
                LabelText2 = "No Data";
            }
            else
            {
                LabelText2 = $"Income: {((ChartValues<decimal>)SeriesCollection2[0].Values).Sum()}  " +
                    $"Potential Income: {((ChartValues<decimal>)SeriesCollection2[1].Values).Sum()}";
            }
        }

        private async Task MeetingsBalance()
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
            if (MonthlyIncome.Count(e => e != 0) == 0)
            {
                LabelText3 = "No Data";
            }
            else
            {
                LabelText3 = $"Held {((ChartValues<int>)SeriesCollection3[0].Values).Sum()}  " +
                    $"Remaining: {((ChartValues<int>)SeriesCollection3[1].Values).Sum()}";
            }
        }

        private async Task LossOfProfit()
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

        private async Task LossOfProfitTime()
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
            LabelText4 = $"Loss of Profit (Time): {((ChartValues<decimal>)SeriesCollection5[0].Values).Sum()}";

            _sync.Release();
        }
    }
}
