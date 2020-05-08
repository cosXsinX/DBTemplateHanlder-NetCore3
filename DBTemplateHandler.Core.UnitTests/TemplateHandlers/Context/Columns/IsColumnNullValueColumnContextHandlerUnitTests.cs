using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class IsColumnNullValueColumnContextHandlerUnitTests
    {
        IsColumnNullValueColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new IsColumnNullValueColumnContextHandler(templateHandler);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowANExceptionWhenColumnModelIsNull()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Column = null };
                _tested.ProcessContext("Hello World !",databaseContext);
            });
        }

        [Test]
        public void ShouldReturnContextContentWhenColumnIsNullable()
        {
            string expectedContent = "Should be present in output";
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = new ColumnModelForTest()
                {
                    IsNotNull = false,
                }
            };
            var result = _tested.ProcessContext($"{_tested.StartContext}{expectedContent}{_tested.EndContext}",databaseContext);
            Assert.AreEqual(expectedContent, result);
        }

        [Test]
        public void ShouldReturnEmptyStringWhenColumnIsNotNullable()
        {
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = new ColumnModelForTest()
                {
                    IsNotNull = true,
                }
            };
            var result = _tested.ProcessContext($"{_tested.StartContext}Should not be present in outpur{_tested.EndContext}",databaseContext);
            Assert.AreEqual(String.Empty, result);
        }
    }
}