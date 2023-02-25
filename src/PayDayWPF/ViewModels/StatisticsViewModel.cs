namespace PayDayWPF.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        public StatisticsViewModel(StatisticsTab1ViewModel statisticsTab1ViewModel, StatisticsTab2ViewModel statisticsTab2ViewModel,
            StatisticsTab3ViewModel statisticsTab3ViewModel, StatisticsTab4ViewModel statisticsTab4ViewModel,
            StatisticsTab5ViewModel statisticsTab5ViewModel)
        {
            StatisticsTab1ViewModel = statisticsTab1ViewModel;
            StatisticsTab2ViewModel = statisticsTab2ViewModel;
            StatisticsTab3ViewModel = statisticsTab3ViewModel;
            StatisticsTab4ViewModel = statisticsTab4ViewModel;
            StatisticsTab5ViewModel = statisticsTab5ViewModel;
        }

        public StatisticsTab1ViewModel StatisticsTab1ViewModel { get; }
        public StatisticsTab2ViewModel StatisticsTab2ViewModel { get; }
        public StatisticsTab3ViewModel StatisticsTab3ViewModel { get; }
        public StatisticsTab4ViewModel StatisticsTab4ViewModel { get; }
        public StatisticsTab5ViewModel StatisticsTab5ViewModel { get; }
    }
}
