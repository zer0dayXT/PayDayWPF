using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.Infrastructure;
using PayDayWPF.Persistence;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PayDayWPF.Pages
{
    /// <summary>
    /// Interaction logic for MarkMeetingPage.xaml
    /// </summary>
    public partial class MarkMeetingPage : Page
    {
        private IRepository _repository;

        public List<Package> Packages { get; set; } = new List<Package>();

        public MarkMeetingPage()
        {
            _repository = ((MainWindow)Application.Current.MainWindow).
                ServiceProvider.GetService<IRepository>();
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Packages.AddRange(await _repository.Load());
        }
    }
}
