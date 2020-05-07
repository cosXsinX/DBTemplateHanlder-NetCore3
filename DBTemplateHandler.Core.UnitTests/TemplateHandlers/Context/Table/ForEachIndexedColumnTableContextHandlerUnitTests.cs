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
            Assert.Throws<Exception>(() => _tested.processContext(null));
        }

        [Test]
        public void ShouldThrowANExceptionWhenTableModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.TableModel = null;
                _tested.processContext("Hello World !");
            });
        }

        [Test]
        public void ShouldThrowAnExceptionWhenColumnModelListIsNullInTableModel()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.TableModel = new TableModel() { Columns = null };
                _tested.processContext("Hello World !");
            });
        }

        [Test]
        public void ShouldThrowAnExceptionWhenAtLeastOneColumnModelInColumnsFromTableModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.TableModel = new TableModel() { Columns = new List<IColumnModel>() { new ColumnModelForTest() { }, null } };
                _tested.processContext("Hello World !");
            });
        }

        [Test]
        public void ShouldReturnEmptyStringWhenThereIsNoColumnModelWhichAreIndexed()
        {
            var columns = Enumerable.Repeat(1,10).Select((_,i)=> new ColumnModelForTest() { Name = $"Column {i}",IsIndexed = false}).Cast<IColumnModel>().ToList();
            _tested.TableModel = new TableModelForTest() { Columns = columns };
            var containedContent = "Hello World !";
            var actualResult = _tested.processContext($"{_tested.StartContext}{containedContent}{_tested.EndContext}");
            var expectedResult = string.Empty;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void ShouldReturnRepeatedContainedContentAsMuchAsThereIsColumnModelWichAreIndexed()
        {
            var indexedColumnNumber = 10;
            var columns = Enumerable.Repeat(1, indexedColumnNumber).Select((_, i) => new ColumnModelForTest() { Name = $"Column {i}", IsIndexed = true }).Cast<IColumnModel>().ToList();
            _tested.TableModel = new TableModelForTest() { Columns = columns };
            var containedContent = "Hello World !";
            var actualResult = _tested.processContext($"{_tested.StartContext}{containedContent}{_tested.EndContext}");
            var expectedResult = string.Join(string.Empty,Enumerable.Repeat(containedContent,indexedColumnNumber));
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void ShouldReturnColumnNamesOfColumnWhichAreIndexed()
        {
            var indexedColumnNumber = 10;
            var columns = Enumerable.Repeat(1, indexedColumnNumber).Select((_, i) => new ColumnModelForTest() { Name = $"Column {i}", IsIndexed = true }).Cast<IColumnModel>().ToList();
            _tested.TableModel = new TableModelForTest() { Columns = columns };
            var actualResult = _tested.processContext($"{_tested.StartContext}{{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME::}}{_tested.EndContext}");
            var expectedResult = string.Join(string.Empty, columns.Select(m => m.Name));
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
