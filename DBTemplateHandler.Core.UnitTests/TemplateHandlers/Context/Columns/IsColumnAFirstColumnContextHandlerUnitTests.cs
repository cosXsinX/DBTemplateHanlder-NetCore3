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
    public class IsColumnAFirstColumnContextHandlerUnitTests
    {
        IsColumnAFirstColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new IsColumnAFirstColumnContextHandler(templateHandler);
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by empty value or by the inner context when the current column is the first column from the current table column collection", _tested.ContextActionDescription);
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.ProcessContext(null,
                new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { } }));
        }

        [Test]
        public void ShouldThrowAnArgumentNullExceptionWhenDatabaseContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext("Hello world", null));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnModelIsNull()
        {
            Assert.Throws<ArgumentException>(() => _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",
                new ProcessorDatabaseContext() { Column = null }));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnModelHasSetParentTableToNull()
        {
            Assert.Throws<Exception>(() => _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",
                new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { }, Table = null }));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnsAreNotSetInColumnModelParentTable()
        {
            Assert.Throws<Exception>(() => _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",
                new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { }, Table = new TableModelForTest() { Columns = null } }));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnsInColumnModelParentTableIsAnEmptyList()
        {
            Assert.Throws<Exception>(() => _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",
                new ProcessorDatabaseContext()
                {
                    Column = new ColumnModelForTest() { },
                    Table = new TableModelForTest() { Columns = new List<IColumnModel>() }
                }));
        }

        [Test]
        public void ProcessContextShouldContextContentWhenTheColumnModelAloneInCollectionValue()
        {
            var columnModel = new ColumnModelForTest() { };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest() { Columns = new List<IColumnModel>() { columnModel } }
            };
            var expected = "HelloWorld";
            var actual = _tested.ProcessContext($"{_tested.StartContext}{expected}{_tested.EndContext}",databaseContext);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheColumnModelIsNotTheFirstColumnModel()
        {
            var columnModel = new ColumnModelForTest() { };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest() { Columns = new List<IColumnModel>() { new ColumnModelForTest() { Name = "Another Column" }, columnModel } }
            };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnContextContentWhenTheColumnModelIsTheFirstColumnModel()
        {
            var columnModel = new ColumnModelForTest() { };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest() { Columns = new List<IColumnModel>() { columnModel, new ColumnModelForTest() { Name = "Another Column" } } }
            };
            var expected = "HelloWorld";
            var actual = _tested.ProcessContext($"{_tested.StartContext}{expected}{_tested.EndContext}",databaseContext);
            Assert.AreEqual(expected, actual);
        }
    }
}
