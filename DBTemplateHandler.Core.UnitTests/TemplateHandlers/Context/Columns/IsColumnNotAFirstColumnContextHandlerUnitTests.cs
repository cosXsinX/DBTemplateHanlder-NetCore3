using DBTemplateHandler.Core.Database;
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
    public class IsColumnNotAFirstColumnContextHandlerUnitTests
    {
        IsColumnNotAFirstColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new IsColumnNotAFirstColumnContextHandler(templateHandler);
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by the inner context when the current column is not the first column from the iterated column collection", _tested.ContextActionDescription);
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.processContext(null));
        }

        [Test]
        public void ShouldThrowAnArgumentNullExceptionWhenDatabaseContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext("Hello world", null));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnModelIsNull()
        {
            _tested.ColumnModel = null;
            Assert.Throws<Exception>(() => _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}"));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnModelHasSetParentTableToNull()
        {
            _tested.ColumnModel = new ColumnModelForTest() { ParentTable = null };
            Assert.Throws<Exception>(() => _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}"));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnsAreNotSetInColumnModelParentTable()
        {
            _tested.ColumnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest() { Columns = null } };
            Assert.Throws<Exception>(() => _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}"));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnsInColumnModelParentTableIsAnEmptyList()
        {
            _tested.ColumnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest() { Columns = new List<IColumnModel>() } };
            Assert.Throws<Exception>(() => _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}"));
        }

        [Test]
        public void ProcessContextShouldReturnEmptyStringWhenTheColumnModelAloneInCollectionValue()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest() };
            columnModel.ParentTable.Columns = new List<IColumnModel>() { columnModel };
            _tested.ColumnModel = columnModel;
            var actual = _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}");
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnContextContentStringWhenTheColumnModelIsNotTheFirstColumnModel()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest() };
            columnModel.ParentTable.Columns = new List<IColumnModel>() { new ColumnModelForTest() { Name = "Another Column" }, columnModel };
            _tested.ColumnModel = columnModel;
            var expected = "HelloWorld";
            var actual = _tested.processContext($"{_tested.StartContext}{expected}{_tested.EndContext}");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringContentWhenTheColumnModelIsTheFirstColumnModel()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest() };
            columnModel.ParentTable.Columns = new List<IColumnModel>() { columnModel, new ColumnModelForTest() { Name = "Another Column" } };
            _tested.ColumnModel = columnModel;
            var actual = _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}");
            Assert.AreEqual(string.Empty, actual);
        }
    }
}
