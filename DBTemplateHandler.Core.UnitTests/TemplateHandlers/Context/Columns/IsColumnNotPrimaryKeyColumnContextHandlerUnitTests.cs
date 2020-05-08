using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class IsColumnNotPrimaryKeyColumnContextHandlerUnitTests
    {
        IsColumnNotPrimaryKeyColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new IsColumnNotPrimaryKeyColumnContextHandler(templateHandler);
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by the inner context when the current iteration column is not a primary key column", _tested.ContextActionDescription);
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.ProcessContext(null, new ProcessorDatabaseContext() { Column = new ColumnModelForTest()}));
        }

        [Test]
        public void ShouldThrowAnArgumentNullExceptionWhenDatabaseContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext("Hello world", null));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnModelIsNull()
        {
            Assert.Throws<ArgumentException>(() => 
                _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",
                    new ProcessorDatabaseContext() { Column = null }));
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyValueWhenColumnIsPrimaryKey()
        {
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",
                    new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { IsPrimaryKey = true }});
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnInnerContextValueWhenColumnIsNotPrimaryKey()
        {
            var expected = "HelloWorld";
            var actual = _tested.ProcessContext($"{_tested.StartContext}{expected}{_tested.EndContext}",
                new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { IsPrimaryKey = false } });
            Assert.AreEqual(expected, actual);
        }

    }
}
