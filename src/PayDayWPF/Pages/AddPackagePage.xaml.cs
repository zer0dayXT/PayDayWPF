using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.Infrastructure;
using PayDayWPF.Persistence;
using System.Windows;
using System.Windows.Controls;

namespace PayDayWPF.Pages
{
    /// <summary>
    /// Interaction logic for AddMenu.xaml
    /// </summary>
    public partial class AddPackagePage : Page
    {
        private IRepository _repository;
        
        public AddPackagePage()
        {
            _repository = ((MainWindow)Application.Current.MainWindow).
                ServiceProvider.GetService<IRepository>();
            InitializeComponent();
        }

        private async void OnAdd(object sender, RoutedEventArgs e)
        {
            await _repository.AddPackage(new Package
            {
                Name = NameTextBox.Text,
                Duration = int.Parse(DurationTextBox.Text),
                MeetingProfit = decimal.Parse(ProfitTextBox.Text),
                MeetingCount = int.Parse(CountTextBox.Text)
            });
        }
    }
}
