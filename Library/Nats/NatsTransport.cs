using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using NATS.Client;

namespace Library.Nats
{
    public class NatsTransport<T> : IDisposable where T : INatAdapter<T>, new()
    {
        private readonly string [] _servers = 
            {
                "nats://nats1.polygon.io:30401",
                "nats://nats2.polygon.io:30402",
                "nats://nats3.polygon.io:30403"
            };

        private readonly IConnection _connection;

        public NatsTransport()
        {
            try
            {
                _connection = new ConnectionFactory().CreateConnection(CreateClientOptions("Client"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private Options CreateClientOptions(string clientName)
        {
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.Servers = _servers;
            opts.AllowReconnect = true;
            opts.Name = clientName;
            opts.Secure = false;
            opts.Token = "oAtPi9GbsH_voUmR81bAnu45gJCo7jspm8ADs6";
            opts.MaxReconnect = 3000;
            opts.ReconnectWait = 1500;

            opts.ClosedEventHandler += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("Client {0} CLOSED!.", clientName);
            };

            opts.ReconnectedEventHandler += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("Client {0} Reconnected!.", clientName);
            };

            opts.AsyncErrorEventHandler += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("Client Async Error");
            };

            opts.DisconnectedEventHandler += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("Client disconnected");
            };

            opts.ServerDiscoveredEventHandler += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("Server disconnected" + args.Conn.State);
            };
            return opts;
        }

        public IObservable<T> ObserveCurrency(string pairTopic = "")
        {
            var subject = new Subject<T>();
            const string allTopic = "C.*";
            var topic = !string.IsNullOrEmpty(pairTopic) ? allTopic.Replace("*", pairTopic) : allTopic;

            _connection.SubscribeAsync(topic, (sender, cbArgs) =>
            {
                subject.OnNext(new T().Adapt(cbArgs.Message));
            });
            return subject.AsObservable().Replay().RefCount();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }

    public interface INatAdapter<out T>
    {
        T Adapt(Msg message);
    }

    public class CurrencyPairPrice : CurrencyPair, INatAdapter<CurrencyPairPrice>, IEqualityComparer<CurrencyPairPrice>
    {
        public string AskPrice { get; }
        public string BidPrice { get; }
        public DateTime TimeStamp { get; }

        public CurrencyPairPrice(string currency1, string currency2, string askPrice, string bidPrice, DateTime timeStamp)
            : base(currency1, currency2)
        {
            AskPrice = askPrice;
            BidPrice = bidPrice;
            TimeStamp = timeStamp;
        }

        public CurrencyPairPrice()
        {
        }

        public CurrencyPairPrice Adapt(Msg message)
        {
            var pair = message.Subject.Replace("C.", "").Split('/');
            var fields = Encoding.UTF8.GetString(message.Data)
                .Replace("{", "")
                .Replace("}", "")
                .Replace("\\", "")
                .Replace("\"", "")
                .Split(',');
            return new CurrencyPairPrice(pair[0],
                pair[1],
                fields[1].Split(':')[1],
                fields[2].Split(':')[1],
               new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToInt64(fields[3].Split(':')[1])).ToLocalTime());
        }


        public bool Equals(CurrencyPairPrice x, CurrencyPairPrice y)
        {
            return base.Equals(x, y);
        }

        public int GetHashCode(CurrencyPairPrice pair)
        {
            return base.GetHashCode();
        }
    }

    public class CurrencyPair : IEqualityComparer<CurrencyPair>
    {
        public string Currency1 { get; }
        public string Currency2 { get; }
        public CurrencyPair(string currency1, string currency2)
        {
            Currency1 = currency1;
            Currency2 = currency2;
        }

        public CurrencyPair()
        {
        }

        public override string ToString()
        {
            return this.Currency1 + "/" + this.Currency2;
        }

        public bool Equals(CurrencyPair x, CurrencyPair y)
        {
            if (x == null || y == null)
                return false;
            return x.ToString().Equals(y.ToString());
        }

        public int GetHashCode(CurrencyPair pair)
        {
            return pair.GetHashCode();
        }
    }
}
