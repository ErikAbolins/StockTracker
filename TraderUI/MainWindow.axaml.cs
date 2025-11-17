using System;
using Avalonia.Controls;
using TraderUI.ViewModels;

namespace TraderUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Prefer setting ALPHAVANTAGE_API_KEY in environment or replace with your key here.
            var apiKey = Environment.GetEnvironmentVariable("ALPHAVANTAGE_API_KEY");
            DataContext = new MainWindowViewModel(apiKey);
        }
    }
}   