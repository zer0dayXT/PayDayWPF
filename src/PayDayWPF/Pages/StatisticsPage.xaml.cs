using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<StatisticsViewModel>();
        }
    }
}
