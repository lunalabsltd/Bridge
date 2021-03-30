using Bridge.Test.NUnit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bridge.ClientTest.Batch1.Collections.Native
{
    [Category(Constants.MODULE_LIST)]
    [TestFixture(TestNameFormat = "Hashtable - {0}")]
    public class HashtableTests
    {
        [Test]
        public void TypePropertiesAreCorrect()
        {
            // #1294
            Assert.AreEqual("System.Collections.Generic.List`1[[System.Int32, mscorlib]]", typeof(List<int>).FullName, "FullName");
            Assert.True(typeof(Hashtable).IsClass, "IsClass should be true");
            object hashtable = new Hashtable();
            Assert.True(hashtable is Hashtable, "is Hashtable should be true");
            Assert.True(hashtable is IDictionary, "is IDictionary should be true");
            Assert.True(hashtable is ICollection, "is ICollection should be true");
            Assert.True(hashtable is IEnumerable, "is IEnumerable should be true");
        }

        [Test]
        public void DefaultConstructorWorks()
        {
            var l = new Hashtable();
            Assert.AreEqual(0, l.Count);
        }

        [Test]
        public void ConstructorWithCapacityWorks()
        {
            var l = new Hashtable(8);
            Assert.AreEqual(0, l.Count);
        }

        [Test]
        public void ConstructorWithLoadFactorWorks()
        {
            var l = new Hashtable(12, 0.8f);
            Assert.AreEqual(0, l.Count);
        }

        [Test]
        public void AddWorks()
        {
            var l = new Hashtable() { { "a", "b" } };
            l.Add("c", "d");
            Assert.AreEqual("b", l["a"] );
            Assert.AreEqual("d", l["c"] );
        }

        [Test]
        public void DeleteWorks()
        {
            var l = new Hashtable() { { "a", "b" } };
            l.Add("c", "d");
            l.Remove("a");
            Assert.AreEqual(false, l.Contains("a"));
            Assert.AreEqual(true, l.Contains("c"));
        }

        [Test]
        public void ClearWorks()
        {
            var l = new Hashtable() { { "a", "b" } };
            l.Clear();
            Assert.AreEqual(l.Count, 0);
        }
    }
}