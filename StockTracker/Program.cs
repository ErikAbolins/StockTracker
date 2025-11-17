using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AlphaVantage.Net.Common.Intervals;
using AlphaVantage.Net.Common.Size;
using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;

namespace ConsoleTests
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            string QUERY_URL = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=IBM&interval=5min&apikey=67YXPOPGNIPYTMNW";
            Uri queryUri = new Uri(QUERY_URL);
            using var Aclient = new AlphaVantageClient("67YXPOPGNIPYTMNW");
            using var stocksClient = Aclient.Stocks();


            using (WebClient client = new WebClient())
            {
                dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));
            }


            StockTimeSeries stockTs = await stocksClient.GetTimeSeriesAsync("AAPL", Interval.Min1, OutputSize.Compact, isAdjusted: true);
            GlobalQuote globalQuote = await stocksClient.GetGlobalQuoteAsync("AAPL");
            ICollection<SymbolSearchMatch> searchMatches = await stocksClient.SearchSymbolAsync("BA");

        }
    }
}

