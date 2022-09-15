using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PayDayWPF.Infrastructure;
using PayDayWPF.Persistence;

namespace PayDayWPF.ViewModels
{
    public class RemainingMeetingsViewModel : ViewModelBase
    {
        private readonly IRepository _repository;

        private ObservableCollection<RemainingItem> _recentlyCompleted = new ObservableCollection<RemainingItem>();
        public ObservableCollection<RemainingItem> RecentlyCompleted
        {
            get => _recentlyCompleted;
            set
            {
                _recentlyCompleted = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RemainingItem> _remaining = new ObservableCollection<RemainingItem>();
        public ObservableCollection<RemainingItem> Remaining
        {
            get => _remaining;
            set
            {
                _remaining = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RemainingItem> _completed = new ObservableCollection<RemainingItem>();
        public ObservableCollection<RemainingItem> Completed
        {
            get => _completed;
            set
            {
                _completed = value;
                OnPropertyChanged();
            }
        }

        public RemainingMeetingsViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
        }

        private async Task Initialize()
        {
            var packages = await _repository.Load();
            packages = MergePackages(packages);
            HandleLeftList(packages);
            HandleMiddleList(packages);
            HandleRightList(packages);
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
                .Where(e => e.MeetingsHeld.Last() < (DateTime.Now - TimeSpan.FromDays(10)))
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
