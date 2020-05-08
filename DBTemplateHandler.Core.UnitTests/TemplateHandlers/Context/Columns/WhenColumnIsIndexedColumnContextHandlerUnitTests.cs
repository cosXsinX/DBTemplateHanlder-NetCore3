using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class WhenColumnIsIndexedColumnContextHandlerUnitTests
    {
        private WhenColumnIsIndexedColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new WhenColumnIsIndexedColumnContextHandler(TemplateHandlerBuilder.Build(null));
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by the inner content when current column is indexed, instead it will be replaced by an empty string", _tested.ContextActionDescription);
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.ProcessContext(null,
                new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { } }));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnModelIsNull()
        {
            Assert.Throws<ArgumentException>(() => _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",
                new ProcessorDatabaseContext() { Column = null }));
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheColumnIsNotIndexed()
        {
            var databaseContext = new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { IsIndexed = false } };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}", databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnContextContentWhenTheColumnIsIndexed()
        {
            var databaseContext = new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { IsIndexed = true } };
            var expected = "HelloWorld";
            var actual = _tested.ProcessContext($"{_tested.StartContext}{expected}{_tested.EndContext}", databaseContext);
            Assert.AreEqual(expected, actual);
        }
    }
}
