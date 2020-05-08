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
    public class IsNotIndexedColumnNotALastIndexedColumnContextHandlerUnitTests
    {
        private IsNotIndexedColumnNotALastIndexedColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new IsNotIndexedColumnNotALastIndexedColumnContextHandler(TemplateHandlerBuilder.Build(null));
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by the inner context when the current column is not the last column from the iterated not indexed column collection", _tested.ContextActionDescription);
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
                new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { }, 
                    Table = new TableModelForTest() { Columns = new List<IColumnModel>() } }));
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenThereIsNoNotIndexedColumnInColumnList()
        {
            var column = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = false };
            var table = new TableModelForTest() { Columns = new List<IColumnModel>() { new ColumnModelForTest() { IsIndexed = true } } };
            var databaseContext = new ProcessorDatabaseContext() { Column = column, Table = table };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}", databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheColumnModelIsNoNotIndexed()
        {
            var column = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = true };
            var table = new TableModelForTest()
            {
                Columns = new List<IColumnModel>() { column }
            };
            var databaseContext = new ProcessorDatabaseContext() { Column = column, Table = table };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}", databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnContextContentWhenTheNotIndexedColumnModelIsNotTheLastNotIndexedColumnModel()
        {
            var column = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = false };
            var table = new TableModelForTest()
            {
                Columns = new List<IColumnModel>() { column, new ColumnModelForTest() { Name = "Another Not Indexed Column", IsIndexed = false } }
            };
            var databaseContext = new ProcessorDatabaseContext() { Column = column, Table = table };
            var expected = "HelloWorld";
            var actual = _tested.ProcessContext($"{_tested.StartContext}{expected}{_tested.EndContext}",databaseContext);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheNotIndexedColumnModelIsTheLastNotIndexedColumnModel()
        {
            var columnModel = new ColumnModelForTest() { IsIndexed = false };
            var table = new TableModelForTest()
            {
                Columns = new List<IColumnModel>() { new ColumnModelForTest() { Name = "Another Not Indexed Column", IsIndexed = false }, columnModel }
            };
            var databaseContext = new ProcessorDatabaseContext() { Column = columnModel, Table = table };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}", databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }


        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheNotIndexedColumnModelIsTheLastNotIndexedColumnModelButNotTheLastElementOfColumn()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = false };
            var table = new TableModelForTest()
            {
                Columns = new List<IColumnModel>() {new ColumnModelForTest() { Name = "Another Not Indexed Column", IsIndexed = false } ,
                columnModel, new ColumnModelForTest() { Name = "Another column which is indexed", IsIndexed = true } }
            };
            var databaseContext = new ProcessorDatabaseContext() {Column = columnModel, Table = table};
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }
    }
}
