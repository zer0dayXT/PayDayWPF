using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.Infrastructure;
using PayDayWPF.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PayDayWPF.Pages
{
    /// <summary>
    /// Interaction logic for MarkMeetingPage.xaml
    /// </summary>
    public partial class MarkMeetingPage : Page
    {
        private IRepository _repository;

        public List<Package> Packages { get; set; } = new List<Package>();
        public List<Package> SelectedPackages { get; set; } = new List<Package>();

        public MarkMeetingPage()
        {
            _repository = ((MainWindow)Application.Current.MainWindow).
                ServiceProvider.GetService<IRepository>();
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            var packages = await _repository.Load();
            var filteredPackages = packages
                .Where(e => e.MeetingsHeld.Count < e.MeetingCount)
                .GroupBy(e => e.Name)
                .Select(e => e.First())
                .ToList();
            Packages.AddRange(filteredPackages);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var firstSelectedPackage = ((object[])e.AddedItems)
                .Select(e => (Package)e)
                .First();
            SelectedPackages.Add(firstSelectedPackage);
        }
    }
}
