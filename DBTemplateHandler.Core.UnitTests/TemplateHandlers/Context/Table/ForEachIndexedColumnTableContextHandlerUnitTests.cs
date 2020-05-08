using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Table
{
    [TestFixture]
    public class ForEachIndexedColumnTableContextHandlerUnitTests
    {
        private ForEachIndexedColumnTableContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new ForEachIndexedColumnTableContextHandler(templateHandler);
        }

        [Test]
        public void ShouldReturnAccurateStartContextValue()
        {
            Assert.AreEqual("{:TDB:TABLE:COLUMN:INDEXED:FOREACH[", _tested.StartContext);
        }

        [Test]
        public void ShouldReturnAccurateEndContextValue()
        {
            Assert.AreEqual("]::}", _tested.EndContext);
        }

        [Test]
        public void isStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ShouldReturnAccurateContextActionDescription()
        {
            Assert.AreEqual("Is replaced by the intern context as many time as there is indexed column in the table", _tested.ContextActionDescription);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Table = new TableModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowANExceptionWhenTableModelIsNull()
        {
            string StringContext = "Hello World !";
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = null }));
        }

        [Test]
        public void ShouldThrowAnExceptionWhenColumnModelListIsNullInTableModel()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Table = new TableModel() { Columns = null } };
                _tested.ProcessContext("Hello World !", databaseContext);
            });
        }

        [Test]
        public void ShouldThrowAnExceptionWhenAtLeastOneColumnModelInColumnsFromTableModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Table = 
                    new TableModel() { Columns = new List<IColumnModel>() { new ColumnModelForTest() { }, null } } };
                _tested.ProcessContext("Hello World !", databaseContext);
            });
        }

        [Test]
        public void ShouldReturnEmptyStringWhenThereIsNoColumnModelWhichAreIndexed()
        {
            var columns = Enumerable.Repeat(1,10).Select((_,i)=> new ColumnModelForTest() { Name = $"Column {i}",IsIndexed = false}).Cast<IColumnModel>().ToList();
            var databaseContext = new ProcessorDatabaseContext() { Table =
                    new TableModelForTest() { Columns = columns }
            };
            var containedContent = "Hello World !";
            var actualResult = _tested.ProcessContext($"{_tested.StartContext}{containedContent}{_tested.EndContext}", databaseContext);
            var expectedResult = string.Empty;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void ShouldReturnRepeatedContainedContentAsMuchAsThereIsColumnModelWichAreIndexed()
        {
            var indexedColumnNumber = 10;
            var columns = Enumerable.Repeat(1, indexedColumnNumber).Select((_, i) => new ColumnModelForTest() { Name = $"Column {i}", IsIndexed = true }).Cast<IColumnModel>().ToList();
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table =
                    new TableModelForTest() { Columns = columns }
            };
            var containedContent = "Hello World !";
            var actualResult = _tested.ProcessContext($"{_tested.StartContext}{containedContent}{_tested.EndContext}", databaseContext);
            var expectedResult = string.Join(string.Empty,Enumerable.Repeat(containedContent,indexedColumnNumber));
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void ShouldReturnColumnNamesOfColumnWhichAreIndexed()
        {
            var indexedColumnNumber = 10;
            var columns = Enumerable.Repeat(1, indexedColumnNumber).Select((_, i) => new ColumnModelForTest() { Name = $"Column {i}", IsIndexed = true }).Cast<IColumnModel>().ToList();
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table =
                    new TableModelForTest() { Columns = columns }
            }; 
            var actualResult = _tested.ProcessContext($"{_tested.StartContext}{{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME::}}{_tested.EndContext}",databaseContext);
            var expectedResult = string.Join(string.Empty, columns.Select(m => m.Name));
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
