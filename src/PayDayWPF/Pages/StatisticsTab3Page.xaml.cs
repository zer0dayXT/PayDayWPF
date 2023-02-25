using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class StatisticsTab3Page : Page
    {
        public StatisticsTab3Page()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<StatisticsViewModel>().StatisticsTab3ViewModel;
        }
    }
}
