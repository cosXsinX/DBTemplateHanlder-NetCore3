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
    public class ColumnNameColumnContextHandlerUnitTests
    {
        ColumnNameColumnContextHandler _tested;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHanlder = TemplateHandlerBuilder.Build(null);
            _tested = new ColumnNameColumnContextHandler(templateHanlder);
        }

        [Test]
        public void ShouldReturnAccurateStartContextValue()
        {
            Assert.AreEqual("{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME", _tested.StartContext);
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
            Assert.AreEqual("Is replaced by the current column name from the iteration", _tested.ContextActionDescription);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.processContext(null));
        }

        [Test]
        public void ShouldThrowAnArgumentNullExceptionWhenDatabaseContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext("Hello world", null));
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
        public void ShouldThrowAnExceptionWhenThereIsContentBetweenStartAndEndContext()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.ColumnModel = new ColumnModelForTest();
                _tested.processContext($"{_tested.StartContext}I Should not be here{_tested.EndContext}");
            });
        }

        [Test]
        public void ShouldReturnColumnName()
        {
            var expected = "HelloWorldColumnName";
            _tested.ColumnModel = new ColumnModelForTest() { Name = expected };
            var actual = _tested.processContext($"{_tested.Signature}");
            Assert.AreEqual(expected, actual);
        }
    }
}
