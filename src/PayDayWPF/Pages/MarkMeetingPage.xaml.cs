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

        private void OnSelectionChangedRight(object sender, SelectionChangedEventArgs e)
        {
            var firstSelectedPackage = ((object[])e.AddedItems)
                .Select(e => (Package)e)
                .FirstOrDefault();
            (DataContext as MarkMeetingViewModel)?.SelectionChangedRightCommand.Execute(firstSelectedPackage);
        }

        private void OnSelectionChangedLeft(object sender, SelectionChangedEventArgs e)
        {
            var firstSelectedPackage = ((object[])e.AddedItems)
                .Select(e => (Package)e)
                .FirstOrDefault();
            (DataContext as MarkMeetingViewModel)?.SelectionChangedLeftCommand.Execute(firstSelectedPackage);
            (sender as ListBox).SelectedIndex = -1;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
