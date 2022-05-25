using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.Infrastructure;
using PayDayWPF.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Package> Packages { get; set; } = new ObservableCollection<Package>();
        public ObservableCollection<Package> SelectedPackages { get; set; } = new ObservableCollection<Package>();

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
            foreach (var filteredPackage in filteredPackages)
            {
                Packages.Add(filteredPackage);
            }
        }

        private void OnSelectionChangedLeft(object sender, SelectionChangedEventArgs e)
        {
            var firstSelectedPackage = ((object[])e.AddedItems)
                .Select(e => (Package)e)
                .First();
            SelectedPackages.Add(firstSelectedPackage);
        }

        private void OnSelectionChangedRight(object sender, SelectionChangedEventArgs e)
        {
            var firstSelectedPackage = ((object[])e.AddedItems)
                .Select(e => (Package)e)
                .FirstOrDefault();
            if (firstSelectedPackage != null)
            {
                SelectedPackages.Remove(firstSelectedPackage);
            }
        }

        private void OnMark(object sender, RoutedEventArgs e)
        {
            foreach (var package in Packages)
            {
                package.MeetingsHeld.Add(calendar.DisplayDate);
            }
        }
    }
}
