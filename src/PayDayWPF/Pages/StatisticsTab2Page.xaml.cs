using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class StatisticsTab2Page : Page
    {
        public StatisticsTab2Page()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<StatisticsViewModel>().StatisticsTab2ViewModel;
        }
    }
}
