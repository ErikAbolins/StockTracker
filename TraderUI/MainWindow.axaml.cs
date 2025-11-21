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

            var apiKey = Environment.GetEnvironmentVariable("YourApiKey");
            DataContext = new MainWindowViewModel(apiKey);
        }
    }
}   
