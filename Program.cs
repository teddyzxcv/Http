using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Npgsql;
using Flurl;
using Flurl.Http;

namespace Http
{
    class Program
    {
        private const string ConnectionString = "Host=139.28.223.173;Port=5432;User ID=aaandreev_4;Password=aaandreev_4;Database=aaandreev_4_db;";
        static async Task Main(string[] args)
        {
            using var db = new NpgsqlConnection(ConnectionString);
            for (DateTime i = new DateTime(2020, 1, 1); i <= DateTime.Now; i = i.AddDays(1))
            {
                string sth = i.ToString().Split(' ')[0].Replace("/", "-");
                var res = await "https://api.ratesapi.io"
                    .AppendPathSegment("api")
                    .AppendPathSegment(sth)
                    .SetQueryParams(new { @base = "RUB" })
                    .GetJsonAsync<Exchange>();
                Exchange ex = res;
                var a = await db.QueryAsync($"insert into crm.EXCHANGE (to_eur,to_usd,to_jpy,date) values (@eur,@usd,@jpy,@data);",
                    new { eur = ex.rates["EUR"], usd = ex.rates["USD"], jpy = ex.rates["JPY"], data = ex.date });
            }

        }
        static async Task CreateTable()
        {
            using var db = new NpgsqlConnection(ConnectionString);
            var query =
                @"CREATE TABLE crm.EXCHANGE(
                   ID SERIAL PRIMARY KEY     NOT NULL,
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
        public DateTime date;

    }
}
