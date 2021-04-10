using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Http
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://api.ratesapi.io/api/latest?base=RUB");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            Exchange ex = JsonConvert.DeserializeObject<Exchange>(content);
            Console.WriteLine(JsonConvert.SerializeObject(ex));

        }
    }
    class Exchange
    {
        [JsonProperty("base")]
        public static string @base = "RUB";

        public Dictionary<string, double> rates = new Dictionary<string, double>();
        public string date;

    }
}
