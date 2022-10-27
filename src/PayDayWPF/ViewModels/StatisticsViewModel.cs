﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using PayDayWPF.Infrastructure;
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

        public StatisticsViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
            OverallStatistics();
            PotentialIncome();
            MeetingsBalance();
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
                    Separator = new LiveCharts.Wpf.Separator { Step = 1 },
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
                    Values = new ChartValues<decimal>(),
                    DataLabels = true,
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<decimal>(),
                    DataLabels = true,
                },
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
                    Values = new ChartValues<int>(),
                    DataLabels = true,
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<int>(),
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
            SeriesCollection[0].Values.Clear();
            SeriesCollection[0].Values.AddRange(MonthlyIncome.Select(e => (object)e));
            SeriesCollection[1].Values.Clear();
            SeriesCollection[1].Values.AddRange(MonthlyHours.Select(e => (object)e));
            if (MonthlyIncome.Count(e => e != 0) == 0)
            {
                LabelText = "No Data";
            }
            else
            {
                LabelText = $"PayDay: {MonthlyIncome.Sum()} ({(MonthlyIncome.Sum() / MonthlyIncome.Count(e => e != 0)).ToString("n2")})   " +
                    $"Time: {MonthlyHours.Sum()} ({(MonthlyHours.Sum() / MonthlyHours.Count(e => e != 0)).ToString("n2")})";
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
        }
    }
}
