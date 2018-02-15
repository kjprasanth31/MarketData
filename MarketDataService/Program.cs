using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketDataService
{
    class Program
    {
        private static MongoClient _client;

        static void Main(string[] args)
        {
            var db = "";
            var userName = ""
            var passWd = ""

            var settings = new MongoClientSettings();
            settings.ConnectTimeout = TimeSpan.FromMinutes(2);
            settings.ConnectionMode = ConnectionMode.Automatic;
            settings.HeartbeatInterval = TimeSpan.FromSeconds(1);
            settings.Credentials = new List<MongoCredential> {MongoCredential.CreatePlainCredential(db, userName, passWd)};
            settings.Servers = new List<MongoServerAddress>() {
                new MongoServerAddress("cluster1-shard-00-00-rys2d.mongodb.net", 27017),
                new MongoServerAddress("cluster1-shard-00-01-rys2d.mongodb.net", 27017),
                new MongoServerAddress("cluster1-shard-00-02-rys2d.mongodb.net", 27017)};

            _client = new MongoClient(settings);

            UpdateCcy("USD", "GBP");
            UpdateCcy("GBP", "JPY");

            Task.Run(async () => await SomeMethodAsync());

            //Console.ReadKey();
        }

       static  async Task SomeMethodAsync()
        {
            var db = _client.GetDatabase("MarketDataDB");
            var fxDB = db.GetCollection<BsonDocument>("FX");

            using (var cursor = await fxDB.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var document in cursor.Current)
                    {
                        var k = document;
                    }
                }
            }
        }

        private static void UpdateCcy(string cur1, string cur2)
        {
            var db = _client.GetDatabase("MarketDataDB");
            var fxDB = db.GetCollection<BsonDocument>("FX");

            var document = new BsonDocument
            {
                { "Currency1", cur1},
                { "Currency2", cur2},
                { "CurrencyExchange", new BsonArray{
                    new BsonDocument { {"Exchange", cur1+cur2 } },
                    new BsonDocument { {"Exchange", cur2+cur1 } } }
                }
            };

            fxDB.InsertOne(document);
        }
    }
}
