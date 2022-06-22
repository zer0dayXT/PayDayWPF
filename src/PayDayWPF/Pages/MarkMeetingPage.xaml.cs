using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.Infrastructure;
using PayDayWPF.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PayDayWPF.Pages
{
    /// <summary>
    /// Interaction logic for MarkMeetingPage.xaml
    /// </summary>
    public partial class MarkMeetingPage : Page
    {
        private IRepository _repository;

        public ObservableCollection<Package> Packages { get; set; } = new ObservableCollection<Package>();
        public ObservableCollection<Package> HeldMeetings { get; set; } = new ObservableCollection<Package>();
        public ObservableCollection<Package> UnheldMeetings { get; set; } = new ObservableCollection<Package>();


        public MarkMeetingPage()
        {
            _repository = ((MainWindow)Application.Current.MainWindow)
                .ServiceProvider.GetService<IRepository>();
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

        private void OnSelectionChangedRight(object sender, SelectionChangedEventArgs e)
        {
            var firstSelectedPackage = ((object[])e.AddedItems)
                .Select(e => (Package)e)
                .FirstOrDefault();
            if (firstSelectedPackage != null)
            {
                HeldMeetings.Remove(firstSelectedPackage);
            }
        }

        private async void OnMark(object sender, RoutedEventArgs e)
        {
            foreach (var package in HeldMeetings)
            {
                package.MeetingsHeld.Add(calendar.SelectedDate.Value);
                await _repository.UpdateMeetings(package.Id, package.MeetingsHeld);
            }
            HeldMeetings.Clear();
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
