using System.Security;
using System.Windows;
using System.Windows.Input;
using PayDayWPF.Pages;
using PayDayWPF.Persistence;

namespace PayDayWPF.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private readonly IRepository _repository;

        public MainMenuViewModel(IRepository repository)
        {
            _repository = repository;
        }

        public string Password { get; set; }
        
        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand QuitCommand => new RelayCommand(_ =>
        {
            Application.Current.Shutdown();
        });

        public ICommand NavigateToAddCommand => new RelayCommand(_ =>
        {
            Navigate(new AddPackagePage());
        });

        public ICommand NavigateToMarkCommand => new RelayCommand(_ =>
        {
            Navigate(new MarkMeetingPage());
        });

        public ICommand NavigateToRemainingCommand => new RelayCommand(_ =>
        {
            Navigate(new RemainingMeetingsPage());
        });

        public ICommand NavigateToStatisticsPage => new RelayCommand(_ =>
        {
            Navigate(new StatisticsPage());
        });

        public void PasswordBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Visibility = Visibility.Hidden;
                _repository.SetPassword(Password);
            }
        }
    }
}
