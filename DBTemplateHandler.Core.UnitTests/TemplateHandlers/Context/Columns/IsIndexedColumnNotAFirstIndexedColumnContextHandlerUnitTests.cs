using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class IsIndexedColumnNotAFirstIndexedColumnContextHandlerUnitTests
    {
        private IsIndexedColumnNotAFirstIndexedColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new IsIndexedColumnNotAFirstIndexedColumnContextHandler(TemplateHandlerBuilder.Build(null));
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by the inner context when the current column is not the first column from the iterated indexed column collection", _tested.ContextActionDescription);
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.processContext(null));
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
            _tested.ColumnModel = new ColumnModelForTest() { ParentTable = null};
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
        public void ProcessContextShouldReturnAnEmptyStringWhenThereIsNoIndexedColumnInColumnList()
        {
            _tested.ColumnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest() { Columns = new List<IColumnModel>() { new ColumnModelForTest() {IsIndexed = false } } } , IsIndexed = true };
            var actual = _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}");
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheColumnModelIsNotIndexed()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = false };
            columnModel.ParentTable.Columns = new List<IColumnModel>() { columnModel };
            _tested.ColumnModel = columnModel;
            var actual = _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}");
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnContextStringValueWhenTheIndexedColumnModelIsNotTheFirstIndexedColumnModel()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = true };
            columnModel.ParentTable.Columns = new List<IColumnModel>() { new ColumnModelForTest() { Name = "Another Indexed Column", IsIndexed = true}, columnModel };
            _tested.ColumnModel = columnModel;
            var expected = "HelloWorld";
            var actual = _tested.processContext($"{_tested.StartContext}{expected}{_tested.EndContext}");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessContextShouldReturnAnEmpptyStringWhenTheIndexedColumnModelIsTheFirstIndexedColumnModel()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = true };
            columnModel.ParentTable.Columns = new List<IColumnModel>() { columnModel, new ColumnModelForTest() { Name = "Another Indexed Column", IsIndexed = true }  };
            _tested.ColumnModel = columnModel;
            var actual = _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}");
            Assert.AreEqual(string.Empty, actual);
        }


        [Test]
        public void ProcessContextShouldReturnAnEmptyStringWhenTheIndexedColumnModelIsTheFirstIndexedColumnModelButNotTheFirstElementOfColumn()
        {
            var columnModel = new ColumnModelForTest() { ParentTable = new TableModelForTest(), IsIndexed = true };
            columnModel.ParentTable.Columns = new List<IColumnModel>() { new ColumnModelForTest() { Name = "Another column which is not indexed" },
                columnModel, new ColumnModelForTest() { Name = "Another Indexed Column", IsIndexed = true } };
            _tested.ColumnModel = columnModel;
            var actual = _tested.processContext($"{_tested.StartContext}HelloWorld{_tested.EndContext}");
            Assert.AreEqual(string.Empty, actual);
        }

        public class ColumnModelForTest : IColumnModel
        {
            public bool IsAutoGeneratedValue { get; set; }
            public bool IsNotNull { get; set; }
            public bool IsPrimaryKey { get; set; }
            public bool IsIndexed { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public int ValueMaxSize { get; set; }
            public ITableModel ParentTable { get; set; }
        }

        public class TableModelForTest : ITableModel
        {
            public IList<IColumnModel> Columns { get; set; }
            public string Name { get; set; }
            public string Schema { get; set; }
            public IDatabaseModel ParentDatabase { get; set; }
            public IList<IForeignKeyConstraintModel> ForeignKeyConstraints { get; set; }
        }
    }
}
