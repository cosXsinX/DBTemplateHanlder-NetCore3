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
    public class IsIndexedColumnNotALastIndexedColumnContextHandlerUnitTests
    {
        private IsIndexedColumnNotALastIndexedColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new IsIndexedColumnNotALastIndexedColumnContextHandler(TemplateHandlerBuilder.Build(null));
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by the inner context when the current column is not the last column from the iterated indexed column collection", _tested.ContextActionDescription);
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
        public void ProcessContextShouldReturnAnEmptyStringWhenThereIsNoIndexedColumnInColumnList()
        {
            var columnModel = new ColumnModelForTest() { IsIndexed = true };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                {
                    Columns = new List<IColumnModel>() { new ColumnModelForTest() { IsIndexed = false } }
                }
            };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}", databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheColumnModelIsNotIndexed()
        {
            var columnModel = new ColumnModelForTest() { IsIndexed = false };
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
        public void ProcessContextShouldReturnAnContextContentWhenTheIndexedColumnModelIsNotTheLastIndexedColumnModel()
        {
            var columnModel = new ColumnModelForTest() { IsIndexed = true };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                {
                    Columns = new List<IColumnModel>() { columnModel, new ColumnModelForTest() { Name = "Another Indexed Column", IsIndexed = true } }
                }
            };
            var expected = "HelloWorld";
            var actual = _tested.ProcessContext($"{_tested.StartContext}{expected}{_tested.EndContext}", databaseContext);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheIndexedColumnModelIsTheLastIndexedColumnModel()
        {
            var columnModel = new ColumnModelForTest() { IsIndexed = true };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                { 
                    Columns = new List<IColumnModel>() { new ColumnModelForTest() { Name = "Another Indexed Column", IsIndexed = true }, columnModel } 
                }
            };
            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}",databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }


        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheIndexedColumnModelIsTheLastIndexedColumnModelButNotTheLastElementOfColumn()
        {
            var columnModel = new ColumnModelForTest() {  IsIndexed = true };
            var databaseContext = new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = new TableModelForTest()
                {
                    Columns = new List<IColumnModel>() {new ColumnModelForTest() { Name = "Another Indexed Column", IsIndexed = true } ,
                        columnModel, new ColumnModelForTest() { Name = "Another column which is not indexed" } }
                }
            };

            var actual = _tested.ProcessContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}", databaseContext);
            Assert.AreEqual(string.Empty, actual);
        }
    }
}
