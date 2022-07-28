using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class RemainingMeetingsPage : Page
    {
        public RemainingMeetingsPage()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<RemainingMeetingsViewModel>();
        }
    }
}
