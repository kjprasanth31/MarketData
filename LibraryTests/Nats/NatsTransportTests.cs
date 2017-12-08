using System;
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
            var yes = true;
            //while (yes)
            {
                _con.ObserveCurrencies()
                    .Distinct()
                    .Subscribe(x =>
                    {
                        System.Diagnostics.Debug.WriteLine(x.ToString());
                    }, () => yes = false);
            }

            _con.Disconnect();

            Assert.IsTrue(true);
        }
    }
}
