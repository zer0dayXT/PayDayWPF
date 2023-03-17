using System.Security;
using System.Windows;
using System.Windows.Input;
using PayDayWPF.Pages;

namespace PayDayWPF.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        public string Password { get; set; }

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

            }
        }
    }
}
