using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class IsColumnNotNullValueColumnContextHandlerUnitTests
    {
        IsColumnNotNullValueColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandlerNew = TemplateHandlerBuilder.Build(null);
            _tested = new IsColumnNotNullValueColumnContextHandler(templateHandlerNew);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.processContext(null));
        }

        [Test]
        public void ShouldThrowANExceptionWhenColumnModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.ColumnModel = null;
                _tested.processContext("Hello World !");
            });
        }

        [Test]
        public void ShouldReturnEmptyStringWhenColumnIsNullable()
        {
            _tested.ColumnModel = new ColumnModelForTest()
            {
                IsNotNull = false,
            };
            var result = _tested.processContext($"{_tested.StartContext}Should not present in output{_tested.EndContext}");
            Assert.AreEqual(String.Empty, result);
        }

        [Test]
        public void ShouldReturnContextContentWhenColumnIsNotNullable()
        {
            string expectedContent = "Should be present in output";
            _tested.ColumnModel = new ColumnModelForTest()
            {
                IsNotNull = true,
            };
            var result = _tested.processContext($"{_tested.StartContext}{expectedContent}{_tested.EndContext}");
            Assert.AreEqual(expectedContent, result);
        }
    }
}