﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace csharp_stellar_base.Tests
{
    [TestClass]
    public class NetworkTests
    {
        [TestMethod]
        public void NetworkId()
        {
            Stellar.Network.CurrentNetwork = "ProjectQ";

            string hexed = Chaos.NaCl.CryptoBytes.ToHexStringLower(Stellar.Network.CurrentNetworkId);

            Assert.AreEqual(hexed, "89046661f1e50483904e55469c7131b76bcaca59d0db74e9c0c188b35c67b49a");
        }

    }
}