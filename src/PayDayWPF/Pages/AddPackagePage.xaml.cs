using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class AddPackagePage : Page
    {
        public AddPackagePage()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<AddPackageViewModel>();
        }
    }
}
