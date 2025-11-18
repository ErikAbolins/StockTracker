using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
            string QUERY_URL = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=IBM&interval=5min&apikey=(Yourapikey)";
            Uri queryUri = new Uri(QUERY_URL);
            using var client = new AlphaVantageClient("67YXPOPGNIPYTMNW");
            using var stocksClient = client.Stocks();



            var stockTs = await stocksClient.GetTimeSeriesAsync("AAPL", Interval.Min1, OutputSize.Compact, isAdjusted: true);
            var quote = await stocksClient.GetGlobalQuoteAsync("AAPL");
            ICollection<SymbolSearchMatch> searchMatches = await stocksClient.SearchSymbolAsync("BA");


            Console.WriteLine($"AAPL CURRENT: ${quote.Price}");
            foreach (var dataPoint in stockTs.DataPoints)
            {
                Console.WriteLine($"{dataPoint.Time}: ${dataPoint.ClosingPrice}");
            }
        }
    }
}

