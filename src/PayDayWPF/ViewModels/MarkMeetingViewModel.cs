using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PayDayWPF.Infrastructure;
using PayDayWPF.Persistence;

namespace PayDayWPF.ViewModels
{
    public class MarkMeetingViewModel : ViewModelBase
    {
        private readonly IRepository _repository;

        private ObservableCollection<Package> _packages = new ObservableCollection<Package>();
        public ObservableCollection<Package> Packages
        {
            get => _packages;
            set
            {
                _packages = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Package> _heldMeetings = new ObservableCollection<Package>();
        public ObservableCollection<Package> HeldMeetings
        {
            get => _heldMeetings;
            set
            {
                _heldMeetings = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Package> _unheldMeetings = new ObservableCollection<Package>();
        public ObservableCollection<Package> UnheldMeetings
        {
            get => _unheldMeetings;
            set
            {
                _unheldMeetings = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }


        public MarkMeetingViewModel(IRepository repository)
        {
            _repository = repository;
            Initialize();
        }

        private async Task Initialize()
        {
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

        public ICommand SelectionChangedRightCommand => new RelayCommand(param =>
        {
            var package = param as Package;
            if (package != null)
            {
                HeldMeetings.Remove(package);
            }
        });

        public ICommand MarkCommand => new RelayCommand(async _ =>
        {
            foreach (var package in HeldMeetings)
            {
                package.MeetingsHeld.Add(SelectedDate);
                await _repository.UpdateMeetings(package.Id, package.MeetingsHeld);
            }
            HeldMeetings.Clear();
        });
    }
}
