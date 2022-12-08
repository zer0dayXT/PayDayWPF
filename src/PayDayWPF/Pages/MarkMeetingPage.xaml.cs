using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.Infrastructure;
using System.Linq;
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

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MarkMeetingViewModel)?.AddHeldMeetingCommand.Execute(
                (sender as TextBlock).DataContext);
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MarkMeetingViewModel)?.AddUnheldMeetingCommand.Execute(
                (sender as TextBlock).DataContext);
        }
    }
}
