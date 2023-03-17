using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<MainMenuViewModel>();
        }

        private void PasswordBox_OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((MainMenuViewModel)DataContext).Password = PasswordBox.Password;
            ((MainMenuViewModel)DataContext).PasswordBox_OnKeyDown(sender, e);
        }
    }
}
