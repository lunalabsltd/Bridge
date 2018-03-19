using Bridge.Test.NUnit;
using System;

namespace Bridge.ClientTest.Batch3.BridgeIssues
{
    /// <summary>
    /// The test here consists in ensuring overrides to ToString() method
    /// works in some situations.
    /// </summary>
    [Category(Constants.MODULE_ISSUES)]
    [TestFixture(TestNameFormat = "#3440 - {0}")]
    public class Bridge3440
    {
        /// <summary>
        /// This is a simple class with no convention, overriding the
        /// ToString() method.
        /// </summary>
        public class Class1
        {
            public override string ToString()
            {
                return "test1";
            }
        }

        /// <summary>
        /// A class implementing the convention attribute and similarly
        /// overriding the ToString() method.
        /// </summary>
        [Convention]
        public class Class2
        {
            public override string ToString()
            {
                return "test2";
            }
        }

        /// <summary>
        /// Test the custom classes above and also System.IO's
        /// [String/Text]Writer's ToString() call.
        /// </summary>
        [Test]
        public static void TestRulesForOverride()
        {
            System.IO.TextWriter writer = new System.IO.StringWriter();
            Assert.AreEqual("", writer.ToString(), "Casting to TextWriter works.");

            System.IO.StringWriter writer1 = new System.IO.StringWriter();
            Assert.AreEqual("", writer1.ToString(), "Without cast works.");

            object o = new Class1();
            Assert.AreEqual("test1", o.ToString(), "Conventionless class' ToString() works.");

            o = new Class2();
            Assert.AreEqual("test2", o.ToString(), "Convention-specified class ToString() works.");
        }
    }
}