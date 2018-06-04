using Bridge.Html5;
using Bridge.Test.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bridge.ClientTest.Batch3.BridgeIssues
{
    [TestFixture(TestNameFormat = "#3612 - {0}")]
    [Rules(Boxing = BoxingRule.Managed)]
    public class Bridge3612
    {
        public enum Mode
        {
            Slow,
            Medium,
            Fast
        }

        public static void SetModeStronglyTyped(Mode? mode)
        {
            SetMode(mode);
        }

        public static void SetMode(object mode)
        {
            Assert.AreEqual("Null", mode ?? "Null");
        }

        [Test]
        public static void TestEnumNullable()
        {
            SetModeStronglyTyped(null);
        }
    }
}