using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using Library.Nats;
using NUnit.Framework;

namespace LibraryTests.Nats
{
    [TestFixture]
    public class NatsTransportTests
    {
        private NatsTransport<CurrencyPair> _con;

        [SetUp]
        public void SetUp()
        {
            _con = new NatsTransport<CurrencyPair>();
        }

        [Test]
        public void TestSomething()
        {
            var dict = new ConcurrentDictionary<string, int>();
            var done = false;

            //while (true)
            //{
            //    if (!done)
            //    {
            //        done = true;
            //        _con.ObserveCurrencies()
            //            .Subscribe(x =>
            //            {
            //                System.Diagnostics.Debug.WriteLine(x.ToString());
            //                //dict.AddOrUpdate(x.ToString(), key => 1, (key, count) => count + 1);
            //                //if (dict.TryGetValue(x.ToString(), out int counts) && counts > 1)
            //                //{
            //                //    System.Diagnostics.Debug.WriteLine(x.ToString() + $" came down more than once - {counts}");
            //                //}
            //            });
            //    }
            //}

            _con.REquest();

            _con.Disconnect();
            Assert.IsTrue(true);
        }
    }
}
