using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using PayDayWPF.ViewModels;
using System.Windows.Input;

namespace PayDayWPF.Pages
{
    public partial class MarkMeetingPage : Page
    {
        public MarkMeetingPage()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<MarkMeetingViewModel>();
        }

        private void OnPackagesMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MarkMeetingViewModel)?.AddHeldMeetingCommand.Execute(
                (sender as TextBlock).DataContext);
        }

        private void OnPackagesMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MarkMeetingViewModel)?.AddUnheldMeetingCommand.Execute(
                (sender as TextBlock).DataContext);
        }

        private void OnHeldMeetingsMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MarkMeetingViewModel)?.RemoveHeldMeetingCommand.Execute(
                (sender as TextBlock).DataContext);
        }

        private void OnUnheldMeetingsMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MarkMeetingViewModel)?.RemoveUnheldMeetingCommand.Execute(
                (sender as TextBlock).DataContext);
        }
    }
}
