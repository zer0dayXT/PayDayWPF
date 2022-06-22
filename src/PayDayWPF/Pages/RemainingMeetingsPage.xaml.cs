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
    /// Interaction logic for RemainingMeetingsPage.xaml
    /// </summary>
    public partial class RemainingMeetingsPage : Page
    {
        private IRepository _repository;
        public ObservableCollection<RemainingItem> RecentlyCompleted { get; set; } = new ObservableCollection<RemainingItem>();
        public ObservableCollection<RemainingItem> Remaining { get; set; } = new ObservableCollection<RemainingItem>();
        public ObservableCollection<RemainingItem> Completed { get; set; } = new ObservableCollection<RemainingItem>();
        
        public RemainingMeetingsPage()
        {
            _repository = ((MainWindow)Application.Current.MainWindow).
                ServiceProvider.GetService<IRepository>();
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            var packages = await _repository.Load();
            HandleLeftList(packages);
            HandleMiddleList(packages);
            HandleRightList(packages);
        }

        private void HandleLeftList(List<Package> packages)
        {
            var filteredPackages = packages
                .Where(e => e.MeetingsHeld.Count == e.MeetingCount)
                .Where(e => e.MeetingsHeld.Last() >= (DateTime.Now - TimeSpan.FromDays(10)))
                .DistinctBy(e => e.Name)
                .OrderBy(e => e.Name);

            foreach (var package in filteredPackages)
            {
                var color = "#FF0000";
                RecentlyCompleted.Add(new RemainingItem
                {
                    Name = package.Name,
                    Color = color
                });
            }
        }

        private void HandleMiddleList(List<Package> packages)
        {
            var filteredPackages = packages
                .Where(e => e.MeetingsHeld.Count != e.MeetingCount);
            filteredPackages = filteredPackages
                .OrderBy(e => e.MeetingCount - e.MeetingsHeld.Count);
            foreach (var package in filteredPackages)
            {
                var color = "#00FF00";
                if (package.MeetingCount - package.MeetingsHeld.Count == 1)
                {
                    color = "#FF0000";
                }
                else if (package.MeetingCount - package.MeetingsHeld.Count == 2)
                {
                    color = "#FF9900";
                }
                Remaining.Add(new RemainingItem
                {
                    Name = package.Name,
                    Remaining = package.MeetingCount - package.MeetingsHeld.Count,
                    Color = color
                });
            }
        }

        private void HandleRightList(List<Package> packages)
        {
            var filteredPackages = packages
                .Where(e => e.MeetingsHeld.Count == e.MeetingCount)
                .DistinctBy(e => e.Name)
                .OrderBy(e => e.Name);

            foreach (var package in filteredPackages)
            {
                var color = "#FF0000";
                Completed.Add(new RemainingItem
                {
                    Name = package.Name,
                    Color = color
                });
            }
        }

    }
}
