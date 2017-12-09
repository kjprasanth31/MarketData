using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using NATS.Client;

namespace Library.Nats
{
    public class NatsTransport<T> where T : INatAdapter<T>, new()
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
                _connection = new ConnectionFactory().CreateConnection(CreateClientOptions("Client2"));
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

        public IObservable<T> ObserveCurrencies()
        {
            var subject = new Subject<T>();

            var c = _connection.SubscribeAsync("C.*", (sender, cbArgs) =>
            {
                subject.OnNext(new T().Adapt(cbArgs.Message.Subject));
            });
            c.Start();
            return subject.AsObservable();
        }

        public void REquest()
        {
            EventHandler<MsgHandlerEventArgs> msgHandler = (sender, cbArgs) =>
            {
                var l = cbArgs.Message;
            };

            using (IAsyncSubscription s = _connection.SubscribeAsync("C.*", msgHandler))
            {
                // Go ahead and keep a thread up for stressing the system a bit.
                while (true)
                {
                    // always up
                    Thread.Sleep(250);
                }
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }

    public interface INatAdapter<out T>
    {
        T Adapt(string data);
    }

    public class CurrencyPair : INatAdapter<CurrencyPair>
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

        public CurrencyPair Adapt(string data)
        {
            var pair = data.Replace("C.", "").Split('/');

            return new CurrencyPair(pair[0], pair[1]);
        }

        public override string ToString()
        {
            return this.Currency1 + "/" +  this.Currency2;
        }
    }
}
