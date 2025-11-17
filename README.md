# StockTracker

A modern, cross-platform desktop application for real-time stock market tracking and analysis, built with .NET 8 and Avalonia UI.

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Avalonia](https://img.shields.io/badge/Avalonia-11.3-8B44AC)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)

## 🚀 Features

- **Real-Time Stock Data**: Live quotes and intraday time series via AlphaVantage API
- **Multi-Symbol Tracking**: Monitor multiple stocks simultaneously with an intuitive watchlist
- **Interactive UI**: Clean, responsive interface with master-detail layout
- **Cross-Platform**: Runs on Windows, macOS, and Linux thanks to Avalonia framework
- **MVVM Architecture**: Well-structured codebase following industry best practices
- **Offline Mode**: Built-in sample data generation for development without API limits

## 🛠️ Tech Stack

- **Framework**: .NET 8.0
- **UI**: Avalonia 11.3 (cross-platform XAML-based UI)
- **Architecture**: MVVM (Model-View-ViewModel)
- **API Integration**: AlphaVantage.Net for financial data
- **Design Pattern**: Command pattern with INotifyPropertyChanged for reactive UI

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- AlphaVantage API Key (free tier available at [alphavantage.co](https://www.alphavantage.co/))

## 🔧 Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/StockTracker.git
   cd StockTracker
   ```

2. **Set your API key** (optional - app works in demo mode without it)
   
   Windows (PowerShell):
   ```powershell
   $env:ALPHAVANTAGE_API_KEY="your_api_key_here"
   ```
   
   macOS/Linux:
   ```bash
   export ALPHAVANTAGE_API_KEY="your_api_key_here"
   ```

3. **Build and run**
   ```bash
   cd TraderUI
   dotnet build
   dotnet run
   ```

## 🎯 Usage

### Adding Symbols
1. Enter a stock symbol (e.g., "AAPL", "MSFT") in the text box
2. Click "Add" to add it to your watchlist
3. Click on any symbol to view its details and price history

### Viewing Data
- **Current Price**: Displays the latest trading price
- **Time Series**: Shows recent 5-minute interval data points
- **Refresh**: Manually update data for the selected symbol

### Managing Your Watchlist
- Click "Remove" next to any symbol to remove it from tracking
- Symbols persist during the session

## 🏗️ Project Structure

```
StockTracker/
├── StockTracker/          # Original console application (prototyping)
│   └── Program.cs
└── TraderUI/              # Main Avalonia UI application
    ├── MainWindow.axaml    # UI layout (XAML)
    ├── MainWindow.axaml.cs # Code-behind
    ├── ViewModels/
    │   └── MainWindowViewModel.cs  # Business logic & data binding
    ├── Program.cs          # Application entry point
    └── App.axaml          # Application resources
```

## 🔑 Key Design Decisions

### MVVM Pattern
The application strictly follows the Model-View-ViewModel pattern:
- **View** (XAML): Pure presentation layer with declarative UI
- **ViewModel**: Business logic, data binding, and command handling
- **Model**: Data structures (StockDataPoint) and API integration

### Reactive UI
All UI updates are driven by `INotifyPropertyChanged`, ensuring smooth, automatic updates when data changes. Commands use `ICommand` interface for clean separation between UI and logic.

### API Rate Limiting
The app intelligently handles AlphaVantage's rate limits:
- Falls back to sample data when no API key is present
- Reduces unnecessary API calls with manual refresh control
- Provides clear status feedback during data loading

## 🚧 Future Enhancements

- [ ] Historical data charting with interactive graphs
- [ ] Portfolio tracking with cost basis and P&L calculations
- [ ] Price alerts and notifications
- [ ] Multiple data provider support (Yahoo Finance, IEX Cloud)
- [ ] Export data to CSV/Excel
- [ ] Dark/Light theme toggle
- [ ] Settings persistence (user preferences, API keys)

## 📝 License

This project is available under the MIT License.

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the [issues page](https://github.com/yourusername/StockTracker/issues).

## 📧 Contact

Email: erikabolins@proton.me 
(Yes i use proton. I like my security)

---

Built with ❤️ using .NET and Avalonia
