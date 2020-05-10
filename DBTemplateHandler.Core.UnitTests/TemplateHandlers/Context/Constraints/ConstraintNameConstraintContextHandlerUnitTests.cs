using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Constraints;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Constraints
{
    [TestFixture]
    public class ConstraintNameConstraintContextHandlerUnitTests
    {
        ConstraintNameConstraintContextHandler _tested;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHanlder = TemplateHandlerBuilder.Build(null);
            _tested = new ConstraintNameConstraintContextHandler(templateHanlder);
        }

        [Test]
        public void ShouldReturnAccurateStartContextValue()
        {
            Assert.AreEqual("{:TDB:CONSTRAINT:CURRENT:NAME", _tested.StartContext);
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
            Assert.AreEqual("Is replaced by the current constraint name from the iteration", _tested.ContextActionDescription);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.ProcessContext(null, new ProcessorDatabaseContext() { ForeignKeyConstraint = new ForeignKeyConstraintModel() { } }));
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
                var databaseContext = new ProcessorDatabaseContext() { Column = null };
                _tested.ProcessContext("Hello World !", databaseContext);
            });
        }

        [Test]
        public void ShouldThrowAnExceptionWhenThereIsContentBetweenStartAndEndContext()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { ForeignKeyConstraint = new ForeignKeyConstraintModel() { } };
                _tested.ProcessContext($"{_tested.StartContext}I Should not be here{_tested.EndContext}", databaseContext);
            });
        }

        [Test]
        public void ShouldReturnColumnName()
        {
            var expected = "HelloWorldColumnName";
            var databaseContext = new ProcessorDatabaseContext() { ForeignKeyConstraint = new ForeignKeyConstraintModel() { ConstraintName = expected } };
            var actual = _tested.ProcessContext($"{_tested.Signature}", databaseContext);
            Assert.AreEqual(expected, actual);
        }
    }
}
