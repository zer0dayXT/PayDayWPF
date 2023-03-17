using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class StatisticsTab5Page : Page
    {
        public StatisticsTab5Page()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<StatisticsViewModel>().StatisticsTab5ViewModel;
        }
    }
}
