using Bridge.Test.NUnit;
using System;

namespace Bridge.ClientTest.Batch3.BridgeIssues
{
    [Category(Constants.MODULE_ISSUES)]
    [TestFixture(TestNameFormat = "#3440 - {0}")]
    public class Bridge3440
    {
        public class Class1
        {
            public override string ToString()
            {
                return "test1";
            }
        }

        [Convention]
        public class Class2
        {
            public override string ToString()
            {
                return "test2";
            }
        }

        [Test]
        public static void TestRulesForOverride()
        {
            System.IO.TextWriter writer = new System.IO.StringWriter();
            Assert.AreEqual("", writer.ToString());

            System.IO.StringWriter writer1 = new System.IO.StringWriter();
            Assert.AreEqual("", writer1.ToString());

            object o = new Class1();
            Assert.AreEqual("test1", o.ToString());

            o = new Class2();
            Assert.AreEqual("test2", o.ToString());
        }
    }
}