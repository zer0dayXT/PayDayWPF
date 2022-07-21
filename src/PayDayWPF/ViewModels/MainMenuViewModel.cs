using System.Windows;
using System.Windows.Input;
using PayDayWPF.Pages;

namespace PayDayWPF.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
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
    }
}
