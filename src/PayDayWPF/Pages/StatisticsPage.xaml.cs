using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
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

        public StatisticsPage()
        {
            _repository = ((MainWindow)Application.Current.MainWindow)
                .ServiceProvider.GetService<IRepository>();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> { 3, 5, 7, 3 }
                },
                new ColumnSeries
                {
                    Values = new ChartValues<decimal> { 5, 6, 2, 7 }
                }
            };
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            var packages = await _repository.Load();
            var filteredPackages = packages
                .Where(e => e.MeetingsHeld.Any(f =>
                {
                    var now = DateTime.Now;
                    var lastSeptember = new DateTime(now.Year, 9, 1);
                    if (lastSeptember > now)
                    {
                        lastSeptember = lastSeptember.AddYears(-1);
                    }
                    return f >= lastSeptember;
                }))
                .ToList();
        }
    }
}
