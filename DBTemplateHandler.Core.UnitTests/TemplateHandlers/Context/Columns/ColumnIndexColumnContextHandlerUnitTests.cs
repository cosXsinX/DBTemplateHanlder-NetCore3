﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class ColumnIndexColumnContextHandlerUnitTests
    {
        private ColumnIndexColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandlerNew = new TemplateHandlerNew(null);
            _tested = new ColumnIndexColumnContextHandler(templateHandlerNew);
        }

        [Test]
        public void DefaultIndexShouldBeMinus1Value()
        {
            Assert.AreEqual(-1, _tested.DefaultIndex);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.processContext(null));
        }


        [Test]
        public void ShouldThrowANExceptionWhenColumnModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.ColumnModel = null;
                _tested.processContext("Hello World !");
            });
        }

        [Test]
        public void ShouldThrowAnExceptionWhenThereIsContentBetweenStartAndEndContext()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.ColumnModel = new ColumnModel();
                _tested.processContext($"{_tested.StartContext}I Should not be here{_tested.EndContext}");
            });
        }

        [Test]
        public void ShouldReturnDefaultIndexWhenParentTableColumnReferenceIsNull()
        {
            var columnModel = new ColumnModel()
            {
                IsAutoGeneratedValue = true
            };

            TableModel tableModel = new TableModel
            {
                Columns = null
            };
            columnModel.ParentTable = tableModel;

            _tested.ColumnModel = columnModel;
            var processedContext = _tested.processContext($"{_tested.StartContext}{_tested.EndContext}");
            Assert.AreEqual($"{_tested.DefaultIndex}", processedContext);
        }

        [Test]
        public void ProcessedContextShouldBe2ValueIndexWhenThereIsTwoColumnBefore()
        {
            var columnModel = new ColumnModel()
            {
                IsAutoGeneratedValue = true
            };

            TableModel tableModel = new TableModel
            {
                Columns = new List<IColumnModel>()
                {
                    new ColumnModel(),
                    new ColumnModel(),
                    columnModel,
                    new ColumnModel(),
                }
            };
            columnModel.ParentTable = tableModel;

            _tested.ColumnModel = columnModel;
            var processedContext = _tested.processContext($"{_tested.StartContext}{_tested.EndContext}");
            Assert.AreEqual($"{2}", processedContext);
        }


        [Test]
        public void ProcessedContextShouldBeDefaultAutoIndexWhenParentTableIsNull()
        {
            var columnModel = new ColumnModel()
            {
                IsAutoGeneratedValue = true
            };
            columnModel.ParentTable = null;

            _tested.ColumnModel = columnModel;
            var processedContext = _tested.processContext($"{_tested.StartContext}{_tested.EndContext}");
            Assert.AreEqual($"{_tested.DefaultIndex}", processedContext);
        }

        [Test]
        public void ProcessedContextShouldBeDefaultAutoIndexWhenColumnIsNotInTheParentTable()
        {
            var columnModel = new ColumnModel()
            {
                IsAutoGeneratedValue = true
            };

            TableModel tableModel = new TableModel
            {
                Columns = new List<IColumnModel>()
                {
                    new ColumnModel
                    {
                        IsAutoGeneratedValue = true,
                    },
                    new ColumnModel
                    {
                        IsAutoGeneratedValue = false,
                    },
                    new ColumnModel
                    {
                        IsAutoGeneratedValue = true,
                    }
                }
            };
            columnModel.ParentTable = tableModel;

            _tested.ColumnModel = columnModel;
            var processedContext = _tested.processContext($"{_tested.StartContext}{_tested.EndContext}");
            Assert.AreEqual($"{_tested.DefaultIndex}", processedContext);
        }



        public class TableModel : ITableModel
        {
            public IList<IColumnModel> Columns { get; set; }
            public string Name { get; set; }
            public IDatabaseModel ParentDatabase { get; set; }
        }

        public class ColumnModel : IColumnModel
        {
            public bool IsAutoGeneratedValue { get; set; }
            public bool IsNotNull { get; set; }
            public bool IsPrimaryKey { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public ITableModel ParentTable { get; set; }
        }
    }
}
