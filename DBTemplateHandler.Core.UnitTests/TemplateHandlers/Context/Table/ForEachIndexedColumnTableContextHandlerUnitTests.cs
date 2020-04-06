﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Service.Contracts.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Table
{
    [TestFixture]
    public class ForEachIndexedColumnTableContextHandlerUnitTests
    {
        private ForEachIndexedColumnTableContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandlerNew = new TemplateHandlerNew(null);
            _tested = new ForEachIndexedColumnTableContextHandler(templateHandlerNew);
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

        public class TableModelForTest : ITableModel
        {
            public IList<IColumnModel> Columns { get; set; }
            public string Name { get; set; }
            public string Schema { get; set; }
            public IDatabaseModel ParentDatabase { get; set; }
            public IList<IForeignKeyConstraintModel> ForeignKeyConstraints { get; set; }
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
    }
}
