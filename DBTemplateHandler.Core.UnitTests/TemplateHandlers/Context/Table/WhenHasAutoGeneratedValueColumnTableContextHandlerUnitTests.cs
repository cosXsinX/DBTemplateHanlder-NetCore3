﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Table
{
    [TestFixture]
    public class WhenHasAutoGeneratedValueColumnTableContextHandlerUnitTests
    {
        private WhenHasAutoGeneratedValueColumnTableContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new WhenHasAutoGeneratedValueColumnTableContextHandler(templateHandler);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Table = new TableModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }


        [Test]
        public void ShouldThrowAnExceptionWhenTableModelIsNull()
        {
            string StringContext = "Hello World !";
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = null }));
        }

        [Test]
        public void ShouldNotReturnContextContentWhenTheTableHasNotAutoColumn()
        {
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table =
                new TableModelForTest()
                {
                    Columns = new List<IColumnModel>()
                    {
                        new ColumnModelForTest() { IsAutoGeneratedValue = false, }
                    }
                }
            };
            var contextContent = "Hello World !";
            var result = _tested.ProcessContext($"{_tested.StartContext}{contextContent}{_tested.EndContext}", databaseContext);
            Assert.AreEqual(String.Empty, result);
        }

        [Test]
        public void ShouldReturnContextContentWhenTheTableHasAutoColumn()
        {
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table =
                new TableModelForTest()
                {
                    Columns = new List<IColumnModel>()
                    {
                        new ColumnModelForTest() { IsAutoGeneratedValue = true, }
                    }
                }
            };
            var contextContent = "Hello World !";
            var result = _tested.ProcessContext($"{_tested.StartContext}{contextContent}{_tested.EndContext}", databaseContext);
            Assert.AreEqual(contextContent, result);
        }
    }
}
