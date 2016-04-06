﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stellar;
using Stellar.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_stellar_base.Tests
{
    [TestClass]
    public class TransactionTests
    {
        public TransactionTests()
        {
            Stellar.Network.CurrentNetwork = "";
        }

        public Stellar.Transaction SampleTransaction(string destAddress)
        {
            Stellar.Network.CurrentNetwork = "";

            var master = KeyPair.Master();
            var dest = string.IsNullOrEmpty(destAddress) ? KeyPair.Random() : KeyPair.FromAddress(destAddress);

            var sourceAccount = new Account(master, 1);

            CreateAccountOperation operation =
                new CreateAccountOperation.Builder(dest, 1000)
                //.SetSourceAccount(master)
                .Build();

            Stellar.Transaction transaction =
                new Stellar.Transaction.Builder(sourceAccount)
                .AddOperation(operation)
                .Build();

            return transaction;
        }

        [TestMethod]
        public void SignatureBaseTest()
        {
            var transaction = SampleTransaction("GDICFS3KJ3ZTW4COVPUX7OCOAZKLLNFAM5FIYSN5FKKM7M7QNXLBPCCH");
            var txXdr = transaction.ToXdr();

            var writer = new Stellar.Generated.ByteWriter();
            Stellar.Generated.Transaction.Encode(writer, txXdr);
            string sig64 = Convert.ToBase64String(writer.ToArray());

            string sigSample64 = "AAAAAL6Qe0ushP7lzogR2y3vyb8LKiorvD1U2KIlfs1wRBliAAAAZAAAAAAAAAABAAAAAAAAAAAAAAABAAAAAAAAAAAAAAAA0CLLak7zO3BOq+l/uE4GVLW0oGdKjEm9KpTPs/Bt1hcAAAAAAAAD6AAAAAA=";
            byte[] sigSample = Convert.FromBase64String(sigSample64);

            var reader = new Stellar.Generated.ByteReader(sigSample);
            var sampleTx = Stellar.Generated.Transaction.Decode(reader);

            CollectionAssert.AreEqual(writer.ToArray(), sigSample);

            Assert.AreEqual(sigSample64, sig64);
        }

        [TestMethod]
        public void HashTest()
        {
            var transaction = SampleTransaction("GDICFS3KJ3ZTW4COVPUX7OCOAZKLLNFAM5FIYSN5FKKM7M7QNXLBPCCH");

            byte[] hash = transaction.Hash();
            string hash64 = Convert.ToBase64String(hash);

            string hashSample64 = "bMPzusbyC+LYcRcxTilrKuSSKTwMVE+vbFdN745w1to=";

            Assert.AreEqual(hashSample64, hash64);
        }
    }
}