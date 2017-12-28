using System;
using System.Reactive.Linq;
using System.Threading;
using Library.Nats;
using NUnit.Framework;

namespace LibraryTests.Nats
{
    [TestFixture]
    public class NatsTransportTests
    {
        private NatsTransport<CurrencyPairPrice> _con;
        private ManualResetEventSlim _resetEvent;

        [SetUp]
        public void SetUp()
        {
            _con = new NatsTransport<CurrencyPairPrice>();
            _resetEvent = new ManualResetEventSlim();
        }

        [Test]
        public void TestSomething()
        {
            _con.ObserveCurrency()
                .Distinct(x => x.ToString())
                .Subscribe(x =>
                {
                    System.Diagnostics.Debug.WriteLine("List   : " + x.ToString()+ " "+x.AskPrice + " " + x.BidPrice + " " + x.TimeStamp);
                }, () => System.Diagnostics.Debug.WriteLine("Completed"));

            _con.ObserveCurrency("C.GBP/USD")
                .Subscribe(x =>
                {
                    System.Diagnostics.Debug.WriteLine("All   : " + x.ToString() + " " + x.AskPrice + " " + x.BidPrice + " " + x.TimeStamp);
                }, () => System.Diagnostics.Debug.WriteLine("Completed"));
            _resetEvent.Wait(TimeSpan.FromMinutes(10));
            _con.Disconnect();
            Assert.IsTrue(true);
        }
    }
}
