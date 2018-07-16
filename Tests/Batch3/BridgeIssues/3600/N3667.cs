using Bridge.Html5;
using Bridge.Test.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bridge.ClientTest.Batch3.BridgeIssues
{
    [TestFixture(TestNameFormat = "#3667 - {0}")]
    public class Bridge3667
    {
        [Test]
        public static void TestNullableTuple()
        {
            (string Prop1, string Prop2)? val = ("test1", "test2");

            Assert.AreEqual("test1", val.Value.Prop1);
            Assert.AreEqual("test1", val?.Prop1);
        }
    }
}