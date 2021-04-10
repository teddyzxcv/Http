using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Npgsql;

namespace Http
{
    class Program
    {
        private const string ConnectionString = "Host=139.28.223.173;Port=5432;User ID=aaandreev_4;Password=aaandreev_4;Database=aaandreev_4_db;";
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://api.ratesapi.io/api/latest?base=RUB");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            Exchange ex = JsonConvert.DeserializeObject<Exchange>(content);
            Console.WriteLine(JsonConvert.SerializeObject(ex));
            await CreateTable();

        }
        static async Task CreateTable()
        {
            using var db = new NpgsqlConnection(ConnectionString);
            var query =
                @"CREATE TABLE crm.EXCHANGE(
                   ID INT PRIMARY KEY     NOT NULL,
                   DATA           TEXT    NOT NULL,
                   TO_USD            REAL     NOT NULL,
                   TO_JPY        REAL,
                   TO_EUR         REAL
                );";

            await db.QueryAsync(
                new CommandDefinition(
                    query
                ));
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
