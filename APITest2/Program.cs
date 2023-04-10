using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace APITest2
{
    public partial class CoinDesk
    {
        [JsonPropertyName("time")]
        public Time? Time { get; set; }


        [JsonPropertyName("chartName")]
        public string? ChartName { get; set; }

        [JsonPropertyName("bpi")]
        public Bpi? Bpi { get; set; }
    }

    public partial class Bpi
    {
        [JsonPropertyName("USD")]
        public Usd? Usd { get; set; }

        [JsonPropertyName("GBP")]
        public Gbp? Gbp { get; set; }

        [JsonPropertyName("EUR")]
        public Eur? Eur { get; set; }

       
    }
    public partial class Usd
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("rate")]
        public string? Rate { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("rate_float")]
        public double? RateFloat { get; set; }

    }
    public partial class Gbp
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("rate")]
        public string? Rate { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("rate_float")]
        public double? RateFloat { get; set; }

    }
    public partial class Eur
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("rate")]
        public string? Rate { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("rate_float")]
        public double? RateFloat { get; set; }

        
    }

    public partial class Time
    {
        [JsonPropertyName("updated")]
        public string? Updated { get; set; }
    }

    public class CatFacts
    {
        [JsonPropertyName("fact")]
        public string? Fact { get; set; }
    }

    public class Activity
    {
        [JsonPropertyName("activity")]
        public string? ToDo { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }
    }


    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string urlBitcoin = "https://api.coindesk.com/v1/bpi/currentprice.json";
        private static string urlFactCat = "https://catfact.ninja/fact";
        private static string urlToDo = "https://www.boredapi.com/api/activity";


        async static Task GetBitcoin()
        {
            var responseString = await client.GetStringAsync(urlBitcoin);
            CoinDesk? coinDest =
                JsonSerializer.Deserialize<CoinDesk>(responseString);
            decimal rateValueUsd = decimal.Parse(coinDest?.Bpi?.Usd?.Rate.Replace(",","").Replace(".", ","));
            string FormattedRateUsd = string.Format("{0:f2}", rateValueUsd);
            decimal rateValueGbp = decimal.Parse(coinDest?.Bpi?.Gbp?.Rate.Replace(",", "").Replace(".", ","));
            string FormattedRateGbp = string.Format("{0:f2}", rateValueGbp);
            decimal rateValueEur = decimal.Parse(coinDest?.Bpi?.Eur?.Rate.Replace(",", "").Replace(".", ","));
            string FormattedRateEur = string.Format("{0:f2}", rateValueEur);
            Console.Clear();
            Console.WriteLine($"On {coinDest?.Time?.Updated} {coinDest?.ChartName} exchange rate:");
            Console.WriteLine($"  {coinDest?.Bpi?.Usd?.Description}");
            Console.WriteLine($"     {coinDest?.Bpi?.Usd?.Code}: {FormattedRateUsd}{WebUtility.HtmlDecode(coinDest?.Bpi?.Usd?.Symbol)}");
            Console.WriteLine();

            Console.WriteLine($"  {coinDest?.Bpi?.Gbp?.Description}");
            Console.WriteLine($"     {coinDest?.Bpi?.Gbp?.Code}: {FormattedRateGbp}{WebUtility.HtmlDecode(coinDest?.Bpi?.Gbp?.Symbol)}");
            Console.WriteLine();
            Console.WriteLine($"  {coinDest?.Bpi?.Eur?.Description}");
            Console.WriteLine($"     {coinDest?.Bpi?.Eur?.Code}: {FormattedRateEur}{WebUtility.HtmlDecode(coinDest?.Bpi?.Eur?.Symbol)}");
            Console.WriteLine();
            Console.WriteLine("0. Enter to exit to the previous menu");
            Console.WriteLine();
            
            await Previous();
            
        }
        async static Task GetFactCat()
        {
            var responseString = await client.GetStringAsync(urlFactCat);
            CatFacts? catFact =
                JsonSerializer.Deserialize<CatFacts>(responseString);
            Console.Clear();
            Console.WriteLine("Did you know this fact about cats?:");
            Console.WriteLine();
            Console.WriteLine($"{catFact?.Fact}");
            Console.WriteLine();
            Console.WriteLine("0. Enter to exit to the previous menu");
            Console.WriteLine();

            await Previous();
        }
        async static Task GetToDo()
        {
            var responseString = await client.GetStringAsync(urlToDo);
            Activity? activity =
                    JsonSerializer.Deserialize<Activity>(responseString);
            Console.Clear();
            Console.WriteLine("The type of activity offered to you:");
            Console.WriteLine($"   {activity?.Type}");
            Console.WriteLine();
            Console.WriteLine("What we advise you to do today:");
            Console.WriteLine($"   {activity?.ToDo}");
            Console.WriteLine();
            Console.WriteLine("Useful links for you:");
            /*if(activity.Link == "")
            {
                Console.WriteLine("   Unfortunately, there are no links");
            }
            else Console.WriteLine($"   {activity?.Link}");*/
            Console.WriteLine($"   {(string.IsNullOrEmpty(activity?.Link) ? "Unfortunately, there are no links" : activity?.Link)}");
            // Console.WriteLine($"   {activity?.Link ?? "Unfortunately, there are no links"}"); - Мало працювати, але - але...
            Console.WriteLine();
            Console.WriteLine("0. Enter to exit to the previous menu");
            Console.WriteLine();

            await Previous();
        }

        
        async static Task Start()
        {
            Console.Clear();
            Console.WriteLine("Please enter a number between 1 and 3:");
            Console.WriteLine("1. Display current Bitcoin price.");
            Console.WriteLine("2. Display a random cat fact.");
            Console.WriteLine("3. Display a random activity to do.");
            Console.WriteLine("0. Enter to exit to the program");
            Console.WriteLine();

            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    await GetBitcoin();
                    break;
                case 2:
                    await GetFactCat();
                    break;
                case 3:
                    await GetToDo();
                    break;
                case 0:
                    await Exit();
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }
        }
        async static Task Previous()
        {
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 0:
                    await Start();
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }
        }

        async static Task Exit()
        {
            Console.Clear();
            Console.WriteLine("Thank you for using this program!");
            Console.WriteLine();
            Environment.Exit(0);
        }
        async static Task Main(string[] args)
        {

            await Start();
            
        }
    }
}
