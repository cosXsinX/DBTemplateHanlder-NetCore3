﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Table
{
    [TestFixture]
    public class WhenHasAutoGeneratedValueColumnTableContextHandlerUnitTests
    {
        private WhenHasAutoGeneratedValueColumnTableContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandlerNew = new TemplateHandlerNew(null);
            _tested = new WhenHasAutoGeneratedValueColumnTableContextHandler(templateHandlerNew);
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
                _tested.TableModel = null;
                _tested.processContext("Hello World !");
            });
        }

        [Test]
        public void ShouldNotReturnContextContentWhenTheTableHasNotAutoColumn()
        {
            _tested.TableModel = new TableModel(){ Columns = new List<IColumnModel>() { new ColumnModel() { IsAutoGeneratedValue = false, } } };
            var contextContent = "Hello World !";
            var result = _tested.processContext($"{_tested.StartContext}{contextContent}{_tested.EndContext}");
            Assert.AreEqual(String.Empty, result);
        }

        [Test]
        public void ShouldReturnContextContentWhenTheTableHasAutoColumn()
        {
            _tested.TableModel = new TableModel()
            {
                Columns = new List<IColumnModel>(){ new ColumnModel(){IsAutoGeneratedValue = true,} }
            };
            var contextContent = "Hello World !";
            var result = _tested.processContext($"{_tested.StartContext}{contextContent}{_tested.EndContext}");
            Assert.AreEqual(contextContent, result);
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
