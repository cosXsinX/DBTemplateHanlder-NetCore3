using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using NUnit.Framework;
using System;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class ColumnValueConvertTypeColumnContextHandlerUnitTests
    {
        private ColumnValueConvertTypeColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new ColumnValueConvertTypeColumnContextHandler();
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.processContext(StringContext), $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenColumnModelIsNull()
        {
            string StringContext = "nianiania";
            _tested.ColumnModel = null;
            Assert.Throws<Exception>(() => _tested.processContext(StringContext), $"The { nameof(_tested.ColumnModel)} is not set");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenInternalContextIsEmpty()
        {
            string StringContext = $"{_tested.StartContext}{_tested.EndContext}";
            _tested.ColumnModel = new ColumnModel();
            Assert.Throws<Exception>(() => _tested.processContext(StringContext), $"There is a problem with the function provided in template '{StringContext}' -> The value parameter cannot be empty");
        }

        [TestCase("INT", "JAVA", "int")]
        [TestCase("BIGINT", "JAVA", "long")]
        [TestCase("BOOLEAN", "JAVA", "boolean")]
        [TestCase("CHAR", "JAVA", "char")]
        [TestCase("DATE", "JAVA", "Date")]
        [TestCase("DATETIME", "JAVA", "Date")]
        [TestCase("DECIMAL", "JAVA", "double")]
        [TestCase("INTEGER", "JAVA", "int")]
        [TestCase("INT", "JAVA", "int")]
        [TestCase("NUMERIC", "JAVA", "double")]
        [TestCase("REAL", "JAVA", "double")]
        [TestCase("STRING", "JAVA", "String")]
        [TestCase("TEXT", "JAVA", "String")]
        [TestCase("TIME", "JAVA", "Date")]
        [TestCase("VARCHAR", "JAVA", "String")]
        [TestCase("BLOB", "JAVA", "BLOB")]
        [TestCase("BLOB", "OTHER_UNKNOWN", "CONVERT:UNKNOWN(OTHER_UNKNOWN)")]
        public void ShouldReturnAccurateMappedValue(string columnType,string destinationTypeSet, string expectedOutput)
        {
            _tested.ColumnModel = new ColumnModel() { Type = columnType, };
            var result = _tested.processContext($"{_tested.StartContext}{destinationTypeSet}{_tested.EndContext}");
            Assert.AreEqual(expectedOutput, result);
        }
    }
}
