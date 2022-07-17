using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.Persistence;

namespace PayDayWPF.Pages
{
    /// <summary>
    /// Interaction logic for StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        private IRepository _repository;

        public SeriesCollection SeriesCollection { get; set; }
        public AxesCollection AxesYCollection { get; set; }
        public AxesCollection Labels { get; set; }
        public decimal[] MonthlyIncome { get; set; }
        public string LabelText { get; set; }

        public StatisticsPage()
        {
            _repository = ((MainWindow)Application.Current.MainWindow)
                .ServiceProvider.GetService<IRepository>();

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
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await OverallStatistics();
        }

        private async Task OverallStatistics()
        {
            MonthlyIncome = new decimal[12];
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
                    MonthlyIncome[(meetingsHeld.Month+3)%12] += package.MeetingProfit;
                }
            }
            SeriesCollection[0].Values.Clear();
            SeriesCollection[0].Values.AddRange(MonthlyIncome.Select(e => (object)e));
            LabelText = "fdasfsadas";
        }
    }
}
