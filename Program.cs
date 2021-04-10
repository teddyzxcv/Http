using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace Http
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://api.ratesapi.io/api/latest?base=USD");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);

        }
    }
}
