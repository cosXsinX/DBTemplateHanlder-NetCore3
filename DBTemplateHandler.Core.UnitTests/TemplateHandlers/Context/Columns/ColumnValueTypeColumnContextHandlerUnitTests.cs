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
    public class ColumnValueTypeColumnContextHandlerUnitTests
    {
        ColumnValueTypeColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new ColumnValueTypeColumnContextHandler(templateHandler);
        }


        [Test]
        public void ShouldReturnAccurateStartContextValue()
        {
            Assert.AreEqual("{:TDB:TABLE:COLUMN:FOREACH:CURRENT:TYPE", _tested.StartContext);
        }

        [Test]
        public void ShouldReturnAccurateEndContextValue()
        {
            Assert.AreEqual("::}", _tested.EndContext);
        }

        [Test]
        public void isStartContextAndEndContextAnEntireWordShouldReturnTrue()
        {
            Assert.IsTrue(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ShouldReturnAccurateContextActionDescription()
        {
            Assert.AreEqual("Is replaced by the current column database model type", _tested.ContextActionDescription);
        }


        [Test]
        public void ShouldThrowAnArgumentNullExceptionWhenDatabaseContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext("Hello world", null));
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnTrue()
        {
            Assert.IsTrue(_tested.isStartContextAndEndContextAnEntireWord);
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
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Column = null };
                _tested.ProcessContext("Hello World !",databaseContext);
            });
        }

        [Test]
        public void ShouldThrowAnExceptionWhenThereIsContentBetweenStartAndEndContext()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { } };
                _tested.ProcessContext($"{_tested.StartContext}I Should not be here{_tested.EndContext}",databaseContext);
            });
        }

        [Test]
        public void ShouldReturnColumnValueType()
        {
            var expected = "HelloWorldColumnValueType";
            var databaseContext = new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { Type = expected } };
            var actual = _tested.ProcessContext($"{_tested.Signature}",databaseContext);
            Assert.AreEqual(expected, actual);
        }
    }
}
