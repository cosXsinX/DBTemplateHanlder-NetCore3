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
    public class IsNotIndexedColumnALastIndexedColumnContextHandlerUnitTests
    {
        private IsNotIndexedColumnALastIndexedColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new IsNotIndexedColumnALastIndexedColumnContextHandler(TemplateHandlerBuilder.Build(null));
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by the inner context when the current column is the last column from the iterated not indexed column collection", _tested.ContextActionDescription);
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
                new ProcessorDatabaseContext()
                {
                    Column = new ColumnModelForTest() { },
                    Table = new TableModelForTest() { Columns = new List<IColumnModel>() }
                }));
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenThereIsNoNotIndexedColumnInColumnList()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = false };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                {
                    Columns = new List<IColumnModel>() { new ColumnModelForTest() { IsIndexed = true } }
                }
            };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheColumnModelIsIndexed()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = true };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                {
                    Columns = new List<IColumnModel>() { columnModel }
                }
            };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}", databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheNotIndexedColumnModelIsNotTheLastNotIndexedColumnModel()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = false };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                {
                    Columns = new List<IColumnModel>() 
                        { columnModel, new ColumnModelForTest() { Name = "Another Not Indexed Column", IsIndexed = false } }
                }
            };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}", databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnContextStringValueWhenTheNotIndexedColumnModelIsTheLastNotIndexedColumnModel()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = false };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                {
                    Columns = new List<IColumnModel>() { new ColumnModelForTest() 
                        { Name = "Another Not Indexed Column", IsIndexed = false }, columnModel }
                }
            };
            var expected = "HelloWorld";
            var actual = _tested.ProcessContext($"{_tested.StartContext}{expected}{_tested.EndContext}", databaseContext);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ProcessContextShouldReturnAnContextStringValueWhenTheNotIndexedColumnModelIsTheLastNotIndexedColumnModelButNotTheLastElementOfColumn()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = false };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                {
                    Columns = new List<IColumnModel>() {new ColumnModelForTest() { Name = "Another Not Indexed Column", IsIndexed = false } ,
                        columnModel, new ColumnModelForTest() { Name = "Another column which is indexed", IsIndexed = true } }
                }
            };
            var expected = "HelloWorld";
            var actual = _tested.ProcessContext($"{_tested.StartContext}{expected}{_tested.EndContext}",databaseContext);
            Assert.AreEqual(expected, actual);
        }
    }
}
