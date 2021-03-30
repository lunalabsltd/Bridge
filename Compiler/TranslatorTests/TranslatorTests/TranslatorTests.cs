using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace Bridge.Translator.Tests
{
    /// <summary>
    /// The test here consists in ensuring Bridge.Translator.ReadStream()
    /// supports streams longer than 1MB in size.
    /// </summary>
    [TestFixture]
    internal class TranslatorTests
    {
        /// <summary>
        /// Wraps Bridge.Translator.ReadStream() in a local TestTranslator
        /// class.
        /// </summary>
        private class TestTranslator : Translator
        {
            public TestTranslator() : base("test") { }

            public byte[] TestReadStream(Stream stream) => base.ReadStream(stream);
        }

        /// <summary>
        /// Creates a 1.5MB stream and submits it to Translator's ReadStream
        /// method. If the final result matches, then the stream was not
        /// truncated.
        /// </summary>
        [Test]
        public void Issue3989()
        {
            var data = Enumerable.Range(0, (int)(1.5 * 1024 * 1024)).Select(i => (byte)(i % 255)).ToArray();
            byte[] dataFromStream;
            using (var stream = new MemoryStream(data))
            {
                dataFromStream = new TestTranslator().TestReadStream(stream);
            }

            var t = new TestTranslator()
            {
                SourceFiles = {""}
            };
            t.Translate();
            Assert.AreEqual(data, dataFromStream, "Bridge.Translator.ReadStream() supports reading 1.5MB large streams.");
        }


    }
}