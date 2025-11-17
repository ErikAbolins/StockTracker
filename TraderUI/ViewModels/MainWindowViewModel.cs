using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AlphaVantage.Net.Common.Intervals;
using AlphaVantage.Net.Common.Size;
using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks.Client;

namespace TraderUI.ViewModels
{
    public class StockDataPoint
    {
        public string Time { get; set; } = "";
        public decimal Close { get; set; }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly string? _apiKey;

        public ObservableCollection<string> Symbols { get; } = new();
        public ObservableCollection<StockDataPoint> TimeSeries { get; } = new();

        string selectedSymbol = "AAPL";
        public string SelectedSymbol
        {
            get => selectedSymbol;
            set
            {
                if (SetField(ref selectedSymbol, value))
                {
                    // fire load in background
                    _ = LoadSymbolAsync(value);
                }
            }
        }

        string newSymbol = "";
        public string NewSymbol
        {
            get => newSymbol;
            set
            {
                if (SetField(ref newSymbol, value))
                {
                    (AddSymbolCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        decimal currentPrice;
        public decimal CurrentPrice
        {
            get => currentPrice;
            set => SetField(ref currentPrice, value);
        }

        string status = "Ready";
        public string Status
        {
            get => status;
            set => SetField(ref status, value);
        }

        public string FooterText => "Baseline UI — connect your data provider to the ViewModel.";

        public ICommand AddSymbolCommand { get; }
        public ICommand RemoveSymbolCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand OpenSymbolDetailsCommand { get; }

        // Optional apiKey allows live requests; omit for offline/sample mode.
        public MainWindowViewModel(string? apiKey = null)
        {
            _apiKey = string.IsNullOrWhiteSpace(apiKey) ? null : apiKey.Trim();

            // Sample/demo data
            Symbols.Add("AAPL");
            Symbols.Add("MSFT");
            Symbols.Add("GOOG");

            AddSymbolCommand = new RelayCommand(_ => AddSymbol(), _ => !string.IsNullOrWhiteSpace(NewSymbol));
            RemoveSymbolCommand = new RelayCommand(p => RemoveSymbol(p as string), p => p is string);
            RefreshCommand = new RelayCommand(async _ => await LoadSymbolAsync(SelectedSymbol));
            OpenSettingsCommand = new RelayCommand(_ => Status = "Settings clicked");
            OpenSymbolDetailsCommand = new RelayCommand(_ => Status = $"Open details for {SelectedSymbol}");

            // Fill sample series
            PopulateSampleSeries();
            CurrentPrice = 172.34m;

            // If API key present, kick off initial load
            if (_apiKey != null)
                _ = LoadSymbolAsync(SelectedSymbol);
        }

        void AddSymbol()
        {
            var s = NewSymbol?.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(s)) return;
            if (!Symbols.Contains(s))
            {
                Symbols.Add(s);
                NewSymbol = "";
                Status = $"{s} added";
            }
            else
            {
                Status = $"{s} already tracked";
            }
        }

        void RemoveSymbol(string symbol)
        {
            if (string.IsNullOrEmpty(symbol)) return;
            Symbols.Remove(symbol);
            if (SelectedSymbol == symbol) SelectedSymbol = Symbols.Count > 0 ? Symbols[0] : "";
            Status = $"{symbol} removed";
        }

        // Loads live quote + intraday series when possible; falls back to sample data when not.
        async Task LoadSymbolAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol)) return;

            try
            {
                Status = $"Loading {symbol}...";

                if (_apiKey == null)
                {
                    // no API key — simulate/rotate sample
                    await Task.Delay(300);
                    var rnd = new Random();
                    CurrentPrice = Math.Round(CurrentPrice + (decimal)(rnd.NextDouble() - 0.5) * 2m, 2);
                    PopulateSampleSeries();
                    Status = $"(sample) Updated {symbol} @ {CurrentPrice:C}";
                    return;
                }

                // Real AlphaVantage calls
                using var client = new AlphaVantageClient(_apiKey);
                using var stocks = client.Stocks();

                var quote = await stocks.GetGlobalQuoteAsync(symbol);
                if (quote != null)
                {
                    CurrentPrice = quote.Price;
                }

                var ts = await stocks.GetTimeSeriesAsync(symbol, Interval.Min5, OutputSize.Compact, isAdjusted: true);
                TimeSeries.Clear();
                if (ts?.DataPoints != null)
                {
                    foreach (var dp in ts.DataPoints
                                         .OrderByDescending(d => d.Time)
                                         .Take(30))
                    {
                        TimeSeries.Add(new StockDataPoint
                        {
                            Time = dp.Time.ToString("yyyy-MM-dd HH:mm"),
                            Close = dp.ClosingPrice
                        });
                    }
                }

                Status = $"Updated {symbol} @ {CurrentPrice:C}";
                OnPropertyChanged(nameof(FooterText));
            }
            catch (Exception ex)
            {
                Status = "Error: " + ex.Message;
            }
        }

        void PopulateSampleSeries()
        {
            TimeSeries.Clear();
            var now = DateTime.Now;
            var rnd = new Random();
            var basePrice = CurrentPrice != 0 ? CurrentPrice : 170m;
            for (int i = 0; i < 30; i++)
            {
                TimeSeries.Add(new StockDataPoint
                {
                    Time = now.AddMinutes(-i * 5).ToString("yyyy-MM-dd HH:mm"),
                    Close = Math.Round(basePrice + (decimal)(rnd.NextDouble() - 0.5) * 4m, 2)
                });
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }
        #endregion
    }

    // Simple ICommand implementation for Avalonia (no WPF CommandManager)
    public class RelayCommand : ICommand
    {
        readonly Action<object?> execute;
        readonly Predicate<object?>? canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;
        public void Execute(object? parameter) => execute(parameter);

        public event EventHandler? CanExecuteChanged;

        // Call this to notify the UI that CanExecute changed
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}